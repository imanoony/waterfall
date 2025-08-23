using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece
{
    private Vector2Int pos;
    public Vector2Int Pos { get => pos; set { pos = value; OnPosChanged?.Invoke(pos); } }
    public event Action<Vector2Int> OnPosChanged; // 구독자에게 알림
    public Player Owner { get; private set; }
    public List<Vector2Int> Offsets { get; protected set; }

    public Piece(Vector2Int initpos, Player owner) { SetPos(initpos); SetOwner(owner); }

    // Piece의 새로운 위치를 설정하는 함수.
    // 불가능한 newpos가 들어올 시 false를 반환한다.
    // 성공하면 Pos를 업데이트하고 true를 반환한다.
    public virtual bool CheckPos(int x,int y)
    {
        if (x < 0 || y < 0) return false;
        if (x == Utils.SizeX && Owner != Player.White) return false; // White만이 우측 경계를 넘을 수 있다.
        if (y == Utils.SizeY && Owner != Player.Black) return false; // Black만이 좌측 경계를 넘을 수 있다.
        if (x >= Utils.SizeX || y >= Utils.SizeY) return false;
        return true;
    }
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

    // Step이 부족한데 Transform 시도를 했으면 null을 반환한다.
    // Step이 충분한데 Transform 시도를 했다면 목표로 하는 새로운 객체를 반환한다.
    public Piece Transform(Type type)
    {
        if (type == typeof(AdultPawn) && Step >= Utils.A_THRESHOLD) return new AdultPawn(Pos, Owner);
        if (type == typeof(God) && Step >= Utils.G_THRESHOLD) return new God(Pos, Owner);
        if (type == typeof(Bishop) && Step >= Utils.B_THRESHOLD) return new Bishop(Pos, Owner);
        if (type == typeof(Knight) && Step >= Utils.K_THRESHOLD) return new Knight(Pos, Owner);
        if (type == typeof(Jump) && Step >= Utils.J_THRESHOLD) return new Jump(Pos, Owner);

        Debug.LogError("형태 변환을 시도했으나 걸음수가 부족하다.");
        return null;
    }
}

public class AdultPawn : Piece
{
    public AdultPawn(Vector2Int initpos, Player owner) : base(initpos, owner)
    {
        if (owner == Player.White) Offsets = new() { new(1, 0) };
        else Offsets = new() { new(0, 1) };
    }
}

public class God : Piece
{
    public int skillPhase=0;
    public Piece Target;
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
    public override bool CheckPos(int x,int y)
    {
        if (x < 0 || y < 0) return false;
        if (x >= Utils.SizeX || y >= Utils.SizeY) return false;
        return true;
    }
}

public class Bishop : Piece
{
    public Bishop(Vector2Int initpos, Player owner) : base(initpos, owner)
    {
        Offsets = new()
        {
            new(1,1),
            new(1,-1),
            new(-1,1), 
            new(-1,-1)
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