using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	public Dictionary<string, TooltipData> tooltipDict = new Dictionary<string, TooltipData>();
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
			StartCoroutine(openGodPanel((God)selected));
		}
		else if (selected is Pawn pawn)
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
		name_Text.text = "현재 직업: " + data.Name;
		explainingText.text = data.explainingText + "\n" + data.description;

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
	IEnumerator openGodPanel(God selected)
	{
		yield return new WaitForSeconds(0.5f);
		pawnPanel.SetActive(false);
		godPanel.SetActive(true);
		tooltipPanel.SetActive(false);

		GodPanelSelectPiece(selected);
	}

	private List<Vector2Int> godHits = new()
	{
		new(0,0), new(0,-1), new(-1,-1), new(-1,0),
		new(-1,1), new(0,1), new(1,1), new(1,0),  new(1,-1)
	};

	// God Panel이 active 상태일 때 움직일 piece 고르기 단계에서 호출된다.
	// 움직일 수 있는 piece에 대한 hit만 표시한다.
	public void GodPanelSelectPiece(God selected)
	{
		GodPanelInit();

		List<Vector2Int> posList = GameManager.Instance.battleManager.GetGodTargetPiece(selected);
		List<Vector2Int> newHits = godHits.Select(offset => selected.Pos + offset).ToList();
		foreach (Vector2Int pos in posList)
		{
			int i = newHits.IndexOf(pos);
			Debug.Log($"Get God Target Piece의 결과 {i}: {pos}");
			if (i < 0) continue;
			godPanel.transform.GetChild(i).gameObject.SetActive(true);
		}

		// 움직일 수 있는 piece가 존재하지 않는 경우 바로 턴을 넘긴다.
		if (posList.Count == 0)
		{
			GameManager.Instance.endTurn();
		}
	}

	// God Panel이 active 상태일 때 움직일 pos 고르기 단계에서 호출된다.
	// 이동시킬 수 있는 위치에서 대한 hit만 표시한다.
	public void GodPanelSelectPos(God selected)
	{
		GodPanelInit();
		
		List<Vector2Int> posList = GameManager.Instance.battleManager.GetGodTargetPos(selected);
		List<Vector2Int> newHits = godHits.Select(offset => selected.Pos + offset).ToList();
		foreach (Vector2Int pos in posList)
		{
			int i = newHits.IndexOf(pos);
			Debug.Log($"Get God Target Pos의 결과 {i}: {pos}");
			if (i < 0) continue;
			godPanel.transform.GetChild(i).gameObject.SetActive(true);
		}
	}

	// God Panel의 모든 자식 오브젝트 끄기 함수
	private void GodPanelInit()
	{
		for (int i = 0; i < 9; i++) godPanel.transform.GetChild(i).gameObject.SetActive(false);
	}
}