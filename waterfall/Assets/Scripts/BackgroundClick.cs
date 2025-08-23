using UnityEngine;

public class BackgroundClick : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("stopped");
        GameManager.Instance.StopSelecting();
    }
}
