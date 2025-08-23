using System;
using UnityEngine;

public class ClickPiece : MonoBehaviour
{
    public GameObject Area;

    public void OnMouseDown()
    {
        Area.SetActive(true);
    }
}
