using UnityEngine;

// 각 Piece 오브젝트에 붙어 선택, 이동, 스킬 사용 등을 실행한다.
public class PieceBehaviour : MonoBehaviour
{
    public Piece piece; // Position, Type, Owner 정보 모두 저장됨.

    public void Init(Piece piece)
    {
        if (this.piece != null)
        {
            this.piece.OnPosChanged -= OnPieceMoved;
        }
        this.piece = piece;
        if (this.piece != null) this.piece.OnPosChanged += OnPieceMoved;
        UpdatePiece();
    }

    private void OnDestroy()
    {
        if (piece != null) piece.OnPosChanged -= OnPieceMoved; // 메모리 누수 방지
    }

    private void OnPieceMoved(Vector2Int newPos) => UpdatePiece();

    // Event OnPosChanged에 의해 Type의 Pos가 변화하면 자동으로 호출된다.
    // 이동 이후 Piece 오브젝트의 상태를 갱신한다.
    private void UpdatePiece()
    {
        transform.position = Utils.PosToIso(piece.Pos);
        GetComponent<SpriteRenderer>().sprite = GameManager.Instance.GetSprite(piece);
        GetComponent<SpriteRenderer>().sortingOrder = Utils.PosToLayer(piece.Pos);
    }

    // 이 Piece를 움직이겠다는 선택 감지
    void OnMouseDown()
    {
        if (GameManager.Instance.isGameOver == true)
        {
            return;
        }
        if (GameManager.Instance.currentPiece == null && GameManager.Instance.currentPlayer == piece.Owner) 
            if(piece.Pos.x < Utils.SizeX && piece.Pos.y < Utils.SizeY)
                GameManager.Instance.selectPiece(piece, this);
    }
}
