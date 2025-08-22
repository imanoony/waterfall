using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;
public class UIManager : MonoBehaviour
{
	public CameraControl control; // 시점 변경을 위한 카메라
	public GameObject pawnPanel;
	public GameObject godPanel;
	/// <summary>
	/// 카메라를 전체 카메라로 변경
	/// </summary>
	public void MainCameraMode()
	{
		control.SetCamera(3f,Vector2.zero);
		foreach (tile tile in GameManager.Instance.battleManager.Map)
		{
			if (tile != null && tile.piece != null && GameManager.Instance.currentPlayer == tile.piece.Owner)
			{
				// piece에게 이펙트 호출
			}
		}
		pawnPanel.SetActive(false);
		godPanel.SetActive(false);
	}
/// <summary>
/// 카메라를 대상 카메라로 변경
/// </summary>
/// <param name="selected"></param>
	public void PieceMode(Piece selected)
	{
		control.SetCamera(2f, Utils.PosToIso(selected.Pos));
	}
	/// <summary>
	/// 스킬 UI 띄우는 함수
	/// </summary>
	/// <param name="selected"></param>
	public void SkillMode(Piece selected)
	{
		control.SetCamera(2f, Utils.PosToIso(selected.Pos));
		if (selected is God)
		{
			StartCoroutine(openGodPanel());
		}
		else if(selected is Pawn pawn)
		{
			if (pawn.Step >= 2)
			{
				StartCoroutine(openPawnPanel());
			}
			else
			{
				GameManager.Instance.endTurn();
			}
		}
		else
		{
			GameManager.Instance.endTurn();
		}
	}

	IEnumerator openPawnPanel()
	{
		yield return new WaitForSeconds(0.5f);
		pawnPanel.SetActive(true);
		godPanel.SetActive(false);
	}
	IEnumerator openGodPanel()
	{
		yield return new WaitForSeconds(0.5f);
		pawnPanel.SetActive(false);
		godPanel.SetActive(true);
	}
}