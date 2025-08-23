using UnityEngine;
using UnityEngine.UI;

public class AlphaCheck : MonoBehaviour
{
    public Image img;

    void Start()
    {
        if (img == null)
            img = GetComponent<Image>();

        // alphaHitTestMinimumThreshold 값을 1로 설정
        img.alphaHitTestMinimumThreshold = 0.7f;
    }
}