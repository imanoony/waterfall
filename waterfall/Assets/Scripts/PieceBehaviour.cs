using UnityEngine;

// 각 Piece 오브젝트에 붙어 선택, 이동, 스킬 사용 등을 실행한다.
public class PieceBehaviour : MonoBehaviour
{
    public Piece Type; // Position, Type, Owner 정보 모두 저장됨.

    // pos 정보를 확인하고 Piece가 위치해야 할 위치 벡터를 반환한다.
    // pos가 invalid한 경우 Type 속 Pos (기물의 위치)를 바탕으로 위치 벡터를 반환한다.
    private Vector2 PosToIso(Vector2Int pos)
    {
        // invalid한 pos이므로 Type 속 Pos에 대한 위치 벡터를 반환한다.
        if (pos.x < 0 || pos.y < 0 || pos.x > Utils.SizeX || pos.y > Utils.SizeY)
        {
            return new(0, 0);
        }
        return new(0, 0);
    }

}
