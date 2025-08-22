using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;     // TextMeshPro UI
    public string jsonFileName = "Dialogue/test_Dialogue";       // 예: "Dialogues/tutorial" (확장자 없이)

    private DialogueLine[] lines;
    private int index;
    private bool waitingForClick;
    private string waitingForEvent;
    private Coroutine currentTyping;
    void Start()
    {
        LoadDialogueData(jsonFileName);
        ShowLine();
    }

    void Update()
    {
        if (waitingForClick && Input.GetMouseButtonDown(0))
        {
            waitingForClick = false;
            NextLine();
        }
    }

    // JSON 파일 불러오기
    void LoadDialogueData(string filePath)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(filePath);
        if (jsonFile == null)
        {
            Debug.LogError("Dialogue JSON 파일을 찾을 수 없음: " + filePath);
            return;
        }

        DialogueDataWrapper wrapper = JsonUtility.FromJson<DialogueDataWrapper>(jsonFile.text);
        lines = wrapper.lines;
    }

    void ShowLine()
    {
        if (index >= lines.Length)
        {
            EndDialogue();
            return;
        }
        var line = lines[index];
        if (currentTyping != null)
        {
            StopCoroutine(currentTyping);
        }

        currentTyping = StartCoroutine(_typing());
        waitingForClick = line.waitForClick;
        waitingForEvent = line.triggerEvent;

        // 클릭도 아니고 이벤트도 없으면 자동으로 바로 다음으로 진행
        if (!waitingForClick && string.IsNullOrEmpty(waitingForEvent))
        {
            Invoke(nameof(NextLine), 0.5f);
        }
    }

    void NextLine()
    {
        index++;
        ShowLine();
    }

    // 외부에서 이벤트를 발생시킬 때 호출
    public void OnExternalEvent(string eventName)
    {
        if (!string.IsNullOrEmpty(waitingForEvent) && waitingForEvent == eventName)
        {
            waitingForEvent = null;
            NextLine();
        }
    }
    void EndDialogue()
    {
        Debug.Log("대사 종료");
        dialogueText.text = "";
        gameObject.SetActive(false);
    }

    IEnumerator _typing()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i <= lines[index].text.Length; i++)
        {
            dialogueText.text = lines[index].text.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }
    }
}