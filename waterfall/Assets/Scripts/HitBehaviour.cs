using UnityEngine;

public class HitBehaviour : MonoBehaviour
{
    public Vector2Int Pos; // Hit이 위치한 좌표
    public void Init(Vector2Int pos)
    {
        Pos = pos;
        transform.position = Utils.PosToIso(Pos);
        gameObject.SetActive(true);
    }

    // 이 Hit가 있는 곳에 Piece를 이동시키겠다는 선택 감지
    void OnMouseDown()
    {
        if (GameManager.Instance.currentPiece != null) GameManager.Instance.selectPosition(Pos);
        gameObject.SetActive(false);
    }
}
