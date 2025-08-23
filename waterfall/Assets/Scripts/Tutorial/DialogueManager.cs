using System.Collections;
using System.Net;
using UnityEngine;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine.Rendering;

public class DialogueManager : MonoBehaviour
{
    public AudioManager audioManager;
    [SerializeField] private AudioClip tutorial;
    public TMP_Text dialogueText;     // TextMeshPro UI
    public string jsonFileName = "Dialogue/test_Dialogue";       // 예: "Dialogues/tutorial" (확장자 없이)
    public CameraControl camControl;
    private DialogueLine[] lines;
    private int index;
    private bool waitingForClick;
    private string waitingForEvent;
    private Coroutine currentTyping;
    private bool isTyping = false;
    public GameObject firstMan;
    public GameObject secondMan;
    void Start()
    {
        LoadDialogueData(jsonFileName);
        firstMan.SetActive(false);
        secondMan.SetActive(false);
        ShowLine();

        audioManager.PlayBGM(tutorial);
    }

    void Update()
    {
        if (waitingForClick && Input.GetMouseButtonDown(0))
        {
            waitingForClick = false;
            if (isTyping) SkipTyping();
            else NextLine();
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
        //if (currentTyping != null)
        //{
        //    StopCoroutine(currentTyping);
        //    isTyping = false;
        //}

        currentTyping = StartCoroutine(_typing());
        waitingForClick = line.waitForClick;
        waitingForEvent = line.triggerEvent;
        if (!string.IsNullOrEmpty(waitingForEvent) && waitingForEvent.StartsWith("B_"))
        {
            StartCoroutine(ExternalEvent(waitingForEvent));
        }
        // 클릭도 아니고 이벤트도 없으면 자동으로 바로 다음으로 진행
        if (!waitingForClick && string.IsNullOrEmpty(waitingForEvent))
        {
            Invoke(nameof(NextLine), 0.5f);
        }
    }

    // _typing 코루틴이 실행되고 있을 때면 코루틴을 종료하고 
    // 버퍼 속 남은 문자열을 한꺼번에 바로 출력한다.
    private void SkipTyping()
    {
        if (!isTyping) return;
        if (currentTyping == null) return;

        StopCoroutine(currentTyping);
        dialogueText.text = lines[index].text;
        waitingForClick = lines[index].waitForClick;
        waitingForEvent = lines[index].triggerEvent;
        isTyping = false;
    }

    void NextLine()
    {
        index++;
        ShowLine();
    }
    public IEnumerator ExternalEvent(string eventName)
    {
        if (eventName == "B_firstMove")
        {
             camControl.SetCamera(2f,Utils.PosToIso(new Vector2Int(0, 7)));
            yield return new WaitForSeconds(3f);
            firstMan.SetActive(true);
            OnExternalEvent("B_firstMove");
        }
        else if (eventName == "firstMove")
        {
            camControl.SetCamera(3f,new(0, 0.7f));
            yield return new WaitForSeconds(0.5f);
            OnExternalEvent("firstMove");
        }
        else if (eventName == "B_secondMove")
        {
            camControl.SetCamera(2f,Utils.PosToIso(new Vector2Int(1, 7)));
            yield return new WaitForSeconds(3f);
            firstMan.SetActive(false);
            secondMan.SetActive(true);
            OnExternalEvent("B_secondMove");
        }
        else if (eventName == "secondMove")
        {
            camControl.SetCamera(3f, new(0, 0.7f));
            yield return new WaitForSeconds(2f);
            OnExternalEvent("secondMove");
        }
        else if (eventName == "B_GameEnd")
        {
            SceneChanger sceneChanger = FindObjectOfType<SceneChanger>();
            if (sceneChanger != null)
            {
                sceneChanger.changeScene("FrontScene");
            }
            else
            {
                Debug.LogWarning("SceneChanger not found in the scene!");
            }
        }

    }
    // 외부에서 이벤트를 발생시킬 때 호출
    public void OnExternalEvent(string eventName)
    {
        Debug.LogError(waitingForEvent);
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
        isTyping = true;
        yield return new WaitForSeconds(0.3f);

        int start = 0, end = lines[index].text.Length;
        if (lines[index].text[0] == '<')
        {
            start = lines[index].text.IndexOf('>') + 1;
            end = lines[index].text.LastIndexOf('<') - 1;
        }

        for (int i = start; i <= end; i++)
        {
            dialogueText.text = i != end ? lines[index].text.Substring(0, i) : lines[index].text;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
    }
}