using System.Collections.Generic;
using UnityEngine;

public class battleManager : MonoBehaviour
{
	public tile[,] Map = new tile[Utils.SizeX, Utils.SizeY];

	public List<Vector2Int> getPossiblePosition(Piece selectedPiece)
	{
		List<Vector2Int> possiblePos = new List<Vector2Int>();
		if (selectedPiece is Jump)
		{
			foreach (Vector2Int Delta in selectedPiece.Offsets)
			{
				if(selectedPiece.CheckPos(selectedPiece.Pos.x + Delta.x, selectedPiece.Pos.y + Delta.y))
						if (Map[selectedPiece.Pos.x + Delta.x, selectedPiece.Pos.y + Delta.y].piece == null)
						{
							possiblePos.Add(new Vector2Int(selectedPiece.Pos.x + Delta.x,
								selectedPiece.Pos.y + Delta.y));
						}

				if(selectedPiece.CheckPos(selectedPiece.Pos.x + 2 * Delta.x, selectedPiece.Pos.y + 2 * Delta.y))
						if (Map[selectedPiece.Pos.x + 2 * Delta.x,
							    selectedPiece.Pos.y + 2 * Delta.y].piece == null &&
						    Map[selectedPiece.Pos.x + Delta.x, selectedPiece.Pos.y + Delta.y]
							    .piece != null)
						{
							possiblePos.Add(new Vector2Int(selectedPiece.Pos.x + 2 * Delta.x,
								selectedPiece.Pos.y + 2 * Delta.y));
						}

				if(selectedPiece.CheckPos(selectedPiece.Pos.x + 3 * Delta.x, selectedPiece.Pos.y + 3 * Delta.y))
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
		else 
		{
			foreach (Vector2Int Delta in selectedPiece.Offsets)
			{
				if(selectedPiece.CheckPos(selectedPiece.Pos.x + Delta.x, selectedPiece.Pos.y + Delta.y))
						if (Map[selectedPiece.Pos.x + Delta.x,
							    selectedPiece.Pos.y + Delta.y].piece == null)
						{
							possiblePos.Add(new Vector2Int(
								selectedPiece.Pos.x + Delta.x,
								selectedPiece.Pos.y + Delta.y));

						}
			}
		}

		return possiblePos;
	}
}
