using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;
public class UIManager : MonoBehaviour
{
	public CameraControl control; // 시점 변경을 위한 카메라
	public GameObject pawnPanel;
	public GameObject godPanel;
	public GameObject tooltipPanel;
	public List<TooltipData> tooltips;
	public Dictionary<string, TooltipData> tooltipDict= new Dictionary<string, TooltipData>();
	public TMP_Text name_Text;
	public TMP_Text explainingText;
	/// <summary>
	/// 카메라를 전체 카메라로 변경
	/// </summary>
	public void MainCameraMode()
	{
		GameManager.Instance.winText.gameObject.SetActive(true);
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
		tooltipPanel.SetActive(false);
	}
/// <summary>
/// 카메라를 대상 카메라로 변경
/// </summary>
/// <param name="selected"></param>
	public void PieceMode(Piece selected)
	{
		GameManager.Instance.winText.gameObject.SetActive(false);
		control.SetCamera(2f, Utils.PosToIso(selected.Pos));
		pawnPanel.SetActive(false);
		godPanel.SetActive(false);
		tooltipPanel.SetActive(true);
		if (selected is Pawn)
		{
			showUI("시민");
		}
		else if (selected is AdultPawn)
		{
			showUI("보병");
		}
		else if (selected is Bishop)
		{
			showUI("사제");
		}
		else if (selected is Jump)
		{
			showUI("무법자");
		}
		else if (selected is Knight)
		{
			showUI("기사");
		}
		else if (selected is God)
		{
			showUI("거인");
		}

	}
	/// <summary>
	/// 스킬 UI 띄우는 함수
	/// </summary>
	/// <param name="selected"></param>
	public void SkillMode(Piece selected)
	{
		GameManager.Instance.winText.gameObject.SetActive(false);
		control.SetCamera(2f, Utils.PosToIso(selected.Pos));
		if (selected is God)
		{
			StartCoroutine(openGodPanel());
		}
		else if(selected is Pawn pawn)
		{
			if (pawn.Step >= 2)
			{
				StartCoroutine(openPawnPanel((Pawn)selected));
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
	public void showUI(string name)
	{
		TooltipData data = tooltipDict[name];
		name_Text.text = "현재 직업: "+data.Name;
		explainingText.text = data.explainingText +"\n"+data.description;

	}
	// pawn panel의 자식 순서: A, B, J, K, G
	IEnumerator openPawnPanel(Pawn selected)
	{
		yield return new WaitForSeconds(0.5f);
		pawnPanel.SetActive(true);
		godPanel.SetActive(false);

		int[] thresholds = new int[]
		{
			Utils.A_THRESHOLD,
			Utils.B_THRESHOLD,
			Utils.J_THRESHOLD,
			Utils.K_THRESHOLD,
			Utils.G_THRESHOLD
		};

		for (int i = 0; i < thresholds.Length; i++)
		{
			float alpha = (selected.Step >= thresholds[i]) ? Utils.ALPHA_HIGH : Utils.ALPHA_LOW;
			pawnPanel.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = alpha;
		}
		tooltipPanel.SetActive(false);
	}
	IEnumerator openGodPanel()
	{
		yield return new WaitForSeconds(0.5f);
		pawnPanel.SetActive(false);
		godPanel.SetActive(true);
		tooltipPanel.SetActive(false);
	}
}