using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class tile
{
	public Piece piece;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
        
	}

	// Update is called once per frame
	void Update()
	{
        
	}
}
public class battleManager : MonoBehaviour
{
	public tile[,] Map = new tile[Utils.SizeX+1, Utils.SizeY+1];
	
	public List<Vector2Int> getPossiblePosition(Piece selectedPiece)
	{
		HashSet<Vector2Int> possiblePos = new();
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
				if(selectedPiece.CheckPos(selectedPiece.Pos.x + 2*Delta.x, selectedPiece.Pos.y + 2*Delta.y))
					if (Map[selectedPiece.Pos.x + 2*Delta.x,
						    selectedPiece.Pos.y + 2*Delta.y].piece == null)
					{
						possiblePos.Add(new Vector2Int(
							selectedPiece.Pos.x + 2*Delta.x,
							selectedPiece.Pos.y + 2*Delta.y));

					}
			}
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
}
