using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera cam;              // 바꿀 카메라
    private float targetSize = 5f;   // 목표 사이즈
    private float lerpSpeed = 5f;    // 보간 속도 (값이 클수록 빠르게 따라감)
    private Vector3 targetPos = new Vector3(0,0.7f,-10);
    private void Start()
    {
        this.targetSize = 3f;
        cam.orthographicSize = 3f;
        cam.gameObject.transform.position = new Vector3(0,0.7f,-20);
    }

    public void SetCamera(float targetSize, Vector2 targetPos)
    {
        this.targetSize = targetSize;
        this.targetPos = new Vector3(targetPos.x,targetPos.y,-20);
    }

    void Update()
    {
        // 부드럽게 크기 변경 (Time.deltaTime 고려)
        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize, 
            targetSize, 
            Time.deltaTime * lerpSpeed
        );
        cam.transform.position = Vector3.Lerp(
            cam.transform.position, targetPos, Time.deltaTime * lerpSpeed);
        
    }
}