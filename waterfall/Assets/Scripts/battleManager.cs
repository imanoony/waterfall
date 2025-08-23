using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class tile
{
	public Piece piece;
}
public class battleManager : MonoBehaviour
{
	public tile[,] Map = new tile[Utils.SizeX + 1, Utils.SizeY + 1];

	public List<Vector2Int> getPossiblePosition(Piece selectedPiece)
	{
		HashSet<Vector2Int> possiblePos = new();
		if (selectedPiece is Jump)
		{
			foreach (Vector2Int Delta in selectedPiece.Offsets)
			{
				if (selectedPiece.CheckPos(selectedPiece.Pos.x + Delta.x, selectedPiece.Pos.y + Delta.y))
					if (Map[selectedPiece.Pos.x + Delta.x, selectedPiece.Pos.y + Delta.y].piece == null)
					{
						possiblePos.Add(new Vector2Int(selectedPiece.Pos.x + Delta.x,
							selectedPiece.Pos.y + Delta.y));
					}

				if (selectedPiece.CheckPos(selectedPiece.Pos.x + 2 * Delta.x, selectedPiece.Pos.y + 2 * Delta.y))
					if (Map[selectedPiece.Pos.x + 2 * Delta.x,
							selectedPiece.Pos.y + 2 * Delta.y].piece == null &&
						Map[selectedPiece.Pos.x + Delta.x, selectedPiece.Pos.y + Delta.y]
							.piece != null)
					{
						possiblePos.Add(new Vector2Int(selectedPiece.Pos.x + 2 * Delta.x,
							selectedPiece.Pos.y + 2 * Delta.y));
					}

				if (selectedPiece.CheckPos(selectedPiece.Pos.x + 3 * Delta.x, selectedPiece.Pos.y + 3 * Delta.y))
					if (Map[selectedPiece.Pos.x + 3 * Delta.x,
							selectedPiece.Pos.y + 3 * Delta.y].piece == null &&
						Map[selectedPiece.Pos.x + 2 * Delta.x,
							selectedPiece.Pos.y + 2 * Delta.y].piece != null &&
						Map[selectedPiece.Pos.x + 1 * Delta.x,
							selectedPiece.Pos.y + 1 * Delta.y].piece == null)
					{
						possiblePos.Add(new Vector2Int(
							selectedPiece.Pos.x + 3 * Delta.x,
							selectedPiece.Pos.y + 3 * Delta.y));
					}
			}
		}
		else if (selectedPiece is AdultPawn || selectedPiece is Bishop)
		{
			foreach (Vector2Int Delta in selectedPiece.Offsets)
			{
				if (selectedPiece.CheckPos(selectedPiece.Pos.x + Delta.x, selectedPiece.Pos.y + Delta.y))
					if (Map[selectedPiece.Pos.x + Delta.x,
							selectedPiece.Pos.y + Delta.y].piece == null)
					{
						possiblePos.Add(new Vector2Int(
							selectedPiece.Pos.x + Delta.x,
							selectedPiece.Pos.y + Delta.y));

					}
					else
					{
						continue;
					}
				else continue;
				if (selectedPiece.CheckPos(selectedPiece.Pos.x + 2 * Delta.x, selectedPiece.Pos.y + 2 * Delta.y))
					if (Map[selectedPiece.Pos.x + 2 * Delta.x,
							selectedPiece.Pos.y + 2 * Delta.y].piece == null)
					{
						possiblePos.Add(new Vector2Int(
							selectedPiece.Pos.x + 2 * Delta.x,
							selectedPiece.Pos.y + 2 * Delta.y));

					}
			}
		}
		else if (selectedPiece is God)
		{
			foreach (Vector2Int Delta in selectedPiece.Offsets)
			{
				if (selectedPiece.CheckPos(selectedPiece.Pos.x + Delta.x, selectedPiece.Pos.y + Delta.y))
					if (Map[selectedPiece.Pos.x + Delta.x,
							selectedPiece.Pos.y + Delta.y].piece == null)
					{
						possiblePos.Add(new Vector2Int(
							selectedPiece.Pos.x + Delta.x,
							selectedPiece.Pos.y + Delta.y));

					}
					else
					{
						continue;
					}
				else continue;
				if (selectedPiece.CheckPos(selectedPiece.Pos.x + 2 * Delta.x, selectedPiece.Pos.y + 2 * Delta.y))
					if (Map[selectedPiece.Pos.x + 2 * Delta.x,
							selectedPiece.Pos.y + 2 * Delta.y].piece == null)
					{
						possiblePos.Add(new Vector2Int(
							selectedPiece.Pos.x + 2 * Delta.x,
							selectedPiece.Pos.y + 2 * Delta.y));

					}
			}
			possiblePos.Add(new Vector2Int(
							selectedPiece.Pos.x,
							selectedPiece.Pos.y));
		}

		foreach (Vector2Int Delta in selectedPiece.Offsets)
		{
			if (selectedPiece.CheckPos(selectedPiece.Pos.x + Delta.x, selectedPiece.Pos.y + Delta.y))
				if (Map[selectedPiece.Pos.x + Delta.x,
						selectedPiece.Pos.y + Delta.y].piece == null)
				{
					possiblePos.Add(new Vector2Int(
						selectedPiece.Pos.x + Delta.x,
						selectedPiece.Pos.y + Delta.y));

				}
				else if (selectedPiece is God &&
				Map[selectedPiece.Pos.x + Delta.x, selectedPiece.Pos.y + Delta.y].piece == selectedPiece)
				{
					possiblePos.Add(new Vector2Int(
						selectedPiece.Pos.x + Delta.x,
						selectedPiece.Pos.y + Delta.y));
				}
		}

		return possiblePos.ToList();
	}

	// God이 움직일 수 있는 piece의 좌표를 모두 반환한다.
	// God의 스킬 사용 범위 중 상대방의 piece가 위치한 좌표를 모두 반환.
	public List<Vector2Int> GetGodTargetPiece(God god)
	{
		HashSet<Vector2Int> result = new();
		foreach (Vector2Int offset in god.Impacts)
		{
			if (offset.Equals(new(0, 0))) continue;
			Vector2Int curr = new(god.Pos.x + offset.x, god.Pos.y + offset.y);
			if (Map[curr.x, curr.y].piece == null) continue;
			if (Map[curr.x, curr.y].piece.Owner == god.Owner) continue;
			result.Add(curr);
		}

		return result.ToList();
	}

	// God이 piece를 이동시킬 수 있는 타일의 좌표를 모두 반환한다.
	// God의 스킬 사용 범위 중 아무런 piece가 놓여 있지 않은 좌표를 모두 반환.
	public List<Vector2Int> GetGodTargetPos(God god)
	{
		HashSet<Vector2Int> result = new();
		foreach (Vector2Int offset in god.Impacts)
		{
			if (offset.Equals(new(0, 0))) continue;
			Vector2Int curr = new(god.Pos.x + offset.x, god.Pos.y + offset.y);
			Debug.Log($"Curr: {curr}");
			if (Map[curr.x, curr.y].piece != null) continue;
			result.Add(curr);
		}

		return result.ToList();
	}	
}
