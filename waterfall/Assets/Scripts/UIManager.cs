using UnityEngine;
using UnityEngine.Pool;
public class UIManager : MonoBehaviour
{
	public Camera mainCamera; // 시점 변경을 위한 카메라
	/// <summary>
	/// 카메라를 전체 카메라로 변경
	/// </summary>
	public void MainCameraMode()
	{
		foreach (tile tile in GameManager.Instance.battleManager.Map)
		{
			if (tile != null && tile.piece != null && GameManager.Instance.currentPlayer == tile.piece.Owner)
			{
				// piece에게 이펙트 호출
			}
		}
	}
/// <summary>
/// 카메라를 대상 카메라로 변경
/// </summary>
/// <param name="selected"></param>
	public void PieceMode(Piece selected)
	{

	}
	/// <summary>
	/// 스킬 UI 띄우는 함수
	/// </summary>
	/// <param name="selected"></param>
	public void SkillMode(Piece selected)
	{

	}
}