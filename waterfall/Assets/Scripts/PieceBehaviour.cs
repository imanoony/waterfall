using UnityEngine;

// 각 Piece 오브젝트에 붙어 선택, 이동, 스킬 사용 등을 실행한다.
public class PieceBehaviour : MonoBehaviour
{
    public Piece Type; // Position, Type, Owner 정보 모두 저장됨.

    private void Awake()
    {
        if (Type != null) Type.OnPosChanged += OnPieceMoved;
    }

    private void OnDestroy()
    {
        if (Type != null) Type.OnPosChanged -= OnPieceMoved; // 메모리 누수 방지
    }

    private void OnPieceMoved(Vector2Int newPos) => UpdatePiece();

    // Event OnPosChanged에 의해 Type의 Pos가 변화하면 자동으로 호출된다.
    // 이동 이후 Piece 오브젝트의 상태를 갱신한다.
    private void UpdatePiece()
    {
        transform.position = Utils.PosToIso(Type.Pos);
        GetComponent<SpriteRenderer>().sortingOrder = Utils.PosToLayer(Type.Pos);
    }

    // 이 Piece를 움직이겠다는 선택 감지
    void OnMouseDown()
    {
        if (GameManager.Instance.currentPiece == null) GameManager.Instance.selectPiece(Type);
    }
}
