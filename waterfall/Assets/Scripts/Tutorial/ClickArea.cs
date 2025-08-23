using System;
using UnityEngine;

public class ClickArea : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public GameObject beforeMan;
    public GameObject nextMan;
    public string eventName;
    public void OnMouseDown()
    {
        beforeMan.SetActive(false);
        nextMan.SetActive(true);
        StartCoroutine(dialogueManager.ExternalEvent(eventName));
        this.transform.localScale = new Vector2(0,0);
    }
}
