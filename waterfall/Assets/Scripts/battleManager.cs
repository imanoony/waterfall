using System.Collections.Generic;
using UnityEngine;

public class battleManager : MonoBehaviour
{
	Vector2Int size = new Vector2Int(8, 8);
	tile[,] Map;

	public List<Vector2Int> getPossiblePosition(piece selectedPiece)
	{
		List<Vector2Int> possiblePos = new List<Vector2Int>();
		if (selectedPiece is Jump)
		{
			foreach (Vector2Int Delta in selectedPiece.Moves)
			{
				if (selectedPiece.position.x + Delta.x < size.x && selectedPiece.position.x + Delta.x >= 0)
					if (selectedPiece.position.y + Delta.y < size.y && selectedPiece.position.y + Delta.y >= 0)
						if (Map[selectedPiece.position.x + Delta.x, selectedPiece.position.y + Delta.y].Piece == null)
						{
							possiblePos.Add(new Vector2Int(selectedPiece.position.x + Delta.x,
								selectedPiece.position.y + Delta.y));
						}

				if (selectedPiece.position.x + 2 * Delta.x < size.x &&
				    selectedPiece.position.x + 2 * Delta.x >= 0)
					if (selectedPiece.position.y + 2 * Delta.y < size.y &&
					    selectedPiece.position.y + 2 * Delta.y >= 0)
						if (Map[selectedPiece.position.x + 2 * Delta.x,
							    selectedPiece.position.y + 2 * Delta.y].Piece == null &&
						    Map[selectedPiece.position.x + Delta.x, selectedPiece.position.y + Delta.y]
							    .Piece != null)
						{
							possiblePos.Add(new Vector2Int(selectedPiece.position.x + 2 * Delta.x,
								selectedPiece.position.y + 2 * Delta.y));
						}

				if (selectedPiece.position.x + 3 * Delta.x < size.x &&
				    selectedPiece.position.x + 3 * Delta.x >= 0)
					if (selectedPiece.position.y + 3 * Delta.y < size.y &&
					    selectedPiece.position.y + 3 * Delta.y >= 0)
						if (Map[selectedPiece.position.x + 3 * Delta.x,
							    selectedPiece.position.y + 3 * Delta.y].Piece == null &&
						    Map[selectedPiece.position.x + 2 * Delta.x,
							    selectedPiece.position.y + 2 * Delta.y].Piece != null &&
						    Map[selectedPiece.position.x + 1 * Delta.x,
							    selectedPiece.position.y + 1 * Delta.y].Piece == null)
						{
							possiblePos.Add(new Vector2Int(
								selectedPiece.position.x + 3 * Delta.x,
								selectedPiece.position.y + 3 * Delta.y));
						}
			}
		}
		else 
		{
			foreach (Vector2Int delta in selectedPiece.Moves)
			{
				if (selectedPiece.position.x + delta.x <= size.x &&
				    selectedPiece.position.x + delta.x > 0)
					if (selectedPiece.position.y + delta.y <= size.y &&
					    selectedPiece.position.y + delta.y > 0)
						if (Map[selectedPiece.position.x + delta.x,
							    selectedPiece.position.y + delta.y].Piece == null)
						{
							possiblePos.Add(new Vector2Int(
								selectedPiece.position.x + delta.x,
								selectedPiece.position.y + delta.y));

						}
			}
		}

		return possiblePos;
	}
}
