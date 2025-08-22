using UnityEngine;

// 각 Piece 오브젝트에 붙어 선택, 이동, 스킬 사용 등을 실행한다.
public class PieceBehaviour : MonoBehaviour
{
    public Piece Type; // Position, Type, Owner 정보 모두 저장됨.

    // 이동 이후 Piece 오브젝트의 상태를 갱신한다.
    private void UpdatePiece()
    {
        transform.position = PosToIso(Type.Pos);
        GetComponent<SpriteRenderer>().sortingOrder = PosToLayer(Type.Pos);
    }

    // pos 정보를 확인하고 Piece가 위치해야 할 위치 벡터를 반환한다.
    // pos가 invalid한 경우 Type 속 Pos (기물의 위치)를 바탕으로 위치 벡터를 반환한다.
    private Vector2 PosToIso(Vector2Int pos)
    {
        Vector2Int mypos = pos;

        // invalid한 pos이므로 Type 속 Pos에 대한 위치 벡터를 반환한다.
        if (pos.x < 0 || pos.y < 0 || pos.x > Utils.SizeX || pos.y > Utils.SizeY) mypos = Type.Pos;

        float isoX = mypos.x * Utils.ISO_STEP + mypos.y * (-1) * Utils.ISO_STEP + Utils.BASE_POSITION.x;
        float isoY = (mypos.x + mypos.y) * Utils.ISO_STEP + Utils.BASE_POSITION.y;

        Debug.Log($"PosToIso 결과: ({pos.x}, {pos.y}) -> ({isoX}, {isoY})");
        return new(isoX, isoY);
    }

    // pos 정보를 확인하고 Piece가 위치해야 할 레이어를 반환한다.
    // pos가 invalid한 경우 Type 속 Pos (기물의 위치)를 바탕으로 레이어를 반환한다.
    private int PosToLayer(Vector2Int pos)
    {
        Vector2Int mypos = pos;

        // invalid한 pos이므로 Type 속 Pos에 대한 위치 벡터를 반환한다.
        if (pos.x < 0 || pos.y < 0 || pos.x > Utils.SizeX || pos.y > Utils.SizeY) mypos = Type.Pos;

        int layer = Utils.BASE_LAYER - (mypos.x + mypos.y);
        Debug.Log($"PosToLayer 결과: ({pos.x}, {pos.y}) -> {layer}");
        return layer;
    }

    // 이 Piece를 움직이겠다는 선택 감지
    void OnMouseDown()
    {
        
    }
}
