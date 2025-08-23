using System;

[Serializable]
public class DialogueLine
{
    public string text;           // 출력할 대사
    public bool waitForClick;     // 클릭 대기 여부
    public string triggerEvent;   // 외부 이벤트 이름 (예: "ClickFirstCitizen")
}

[Serializable]
public class DialogueDataWrapper
{
    public DialogueLine[] lines;  // 대사 배열
}
