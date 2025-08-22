using System.Collections.Generic;
using UnityEngine;

public abstract class Piece
{
    public Vector2Int Pos { get; private set; }
    public Player Owner { get; private set; }
    public List<Vector2Int> Offsets { get; protected set; }

    public Piece(Vector2Int initpos, Player owner) { SetPos(initpos); SetOwner(owner); }

    // Piece의 새로운 위치를 설정하는 함수.
    // 불가능한 newpos가 들어올 시 false를 반환한다.
    // 성공하면 Pos를 업데이트하고 true를 반환한다.
    public bool SetPos(Vector2Int newpos)
    {
        if (newpos.x < 0 || newpos.y < 0) return false;
        if (newpos.x == Utils.SizeX && Owner != Player.White) return false; // White만이 우측 경계를 넘을 수 있다.
        if (newpos.y == Utils.SizeY && Owner != Player.Black) return false; // Black만이 좌측 경계를 넘을 수 있다.
        if (newpos.x >= Utils.SizeX || newpos.y >= Utils.SizeY) return false;

        Pos = newpos;
        return true;
    }

    public void SetOwner(Player player) => Owner = player;
}

public class Pawn : Piece
{
    public int Step { get; private set; }
    public void AddStep() => Step++;
    public Pawn(Vector2Int initpos, Player owner) : base(initpos, owner)
    {
        Offsets = owner == Player.White ? new() { new(1, 0) } : new() { new(0, 1) };
    }
}

public class AdultPawn : Piece
{
    public AdultPawn(Vector2Int initpos, Player owner) : base(initpos, owner)
    {
        if (owner == Player.White) Offsets = new() { new(1, 0), new(2, 0) };
        else Offsets = new() { new(0, 1), new(0, 2) };
    }
}

public class God : Piece
{
    public List<Vector2Int> Impacts = new()
    {
        new(1,0), new(1,1), new(1,-1),
        new(0,1), new(0,-1),
        new(-1,0), new(-1,1), new(-1,-1)
    };
    public God(Vector2Int initpos, Player owner) : base(initpos, owner)
    {
        Offsets = new()
        {
            new(1,0), new(2,0),
            new(0,1), new(0,2),
            new(-1,0), new(-2,0),
            new(0,-1), new(0,-2)
        };
    }
}

public class Bishop : Piece
{
    public Bishop(Vector2Int initpos, Player owner) : base(initpos, owner)
    {
        Offsets = new()
        {
            new(1,1), new(2,2),
            new(1,-1), new(2,-2),
            new(-1,1), new(-2,2),
            new(-1,-1), new(-2,-2)
        };
    }
}

public class Knight : Piece
{
    public Knight(Vector2Int initpos, Player owner) : base(initpos, owner)
    {
        Offsets = new() { new(1, 2), new(-1, 2), new(1, -2), new(-1, -2) };
    }
}

public class Jump : Piece
{
    public Jump(Vector2Int initpos, Player owner) : base(initpos, owner)
    {
        Offsets = new() { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
    }
}