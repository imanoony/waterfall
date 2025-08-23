using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Piece
{
    private Vector2Int pos;
    public Vector2Int Pos { get => pos; set { pos = value; OnPosChanged?.Invoke(pos); } }
    public event Action<Vector2Int> OnPosChanged; // 구독자에게 알림
    public Player Owner { get; private set; }
    public List<Vector2Int> Offsets { get; protected set; }
    public virtual float PosOffset { get; protected set; } = 0f;

    public Piece(Vector2Int initpos, Player owner) { SetPos(initpos); SetOwner(owner); }

    // Piece의 새로운 위치를 설정하는 함수.
    // 불가능한 newpos가 들어올 시 false를 반환한다.
    // 성공하면 Pos를 업데이트하고 true를 반환한다.
    public virtual bool CheckPos(int x,int y)
    {
        if (x < 0 || y < 0) return false;
        if (x == Utils.SizeX && Owner != Player.White) return false; // White만이 우측 경계를 넘을 수 있다.
        if (y == Utils.SizeY && Owner != Player.Black) return false; // Black만이 좌측 경계를 넘을 수 있다.
        if (x > Utils.SizeX || y > Utils.SizeY) return false;
        if (Utils.FORBIDDEN.Contains(new(x, y))) return false;
        return true;
    }
    public bool SetPos(Vector2Int newpos)
    {
        if (newpos.x < 0 || newpos.y < 0) return false;
        if (newpos.x == Utils.SizeX && Owner != Player.White) return false; // White만이 우측 경계를 넘을 수 있다.
        if (newpos.y == Utils.SizeY && Owner != Player.Black) return false; // Black만이 좌측 경계를 넘을 수 있다.
        if (newpos.x > Utils.SizeX || newpos.y > Utils.SizeY) return false;
        if (Utils.FORBIDDEN.Contains(newpos)) return false;
        if (newpos.x == Utils.SizeX)
        {
            GameManager.Instance.GetPoint(Owner);
        }

        if (newpos.y == Utils.SizeY)
        {
            GameManager.Instance.GetPoint(Owner);
        }
        Pos = newpos;
        return true;
    }

    public void SetOwner(Player player) => Owner = player;
}

public class Pawn : Piece
{
    public int Step { get; private set; }
    public override float PosOffset { get; protected set; } = 0.15f;
    public void AddStep()
    {
        Step++;
        Debug.Log($"[Pawn {RuntimeHelpers.GetHashCode(this)}] 현재 Step: {Step}");
    }
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
    public override float PosOffset { get; protected set; } = 0.2f;
    public AdultPawn(Vector2Int initpos, Player owner) : base(initpos, owner)
    {
        if (owner == Player.White) Offsets = new() { new(1, 0) };
        else Offsets = new() { new(0, 1) };
    }
}

public class God : Piece
{
    public override float PosOffset { get; protected set; } = 0.23f;
    public int skillPhase = 0;
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
            new(1,0),
            new(0,1),
            new(-1,0),
            new(0,-1)
        };
    }
    public override bool CheckPos(int x,int y)
    {
        if (x == 0 && y == 0) return false;
        if (x < 0 || y < 0) return false;
        if (x >= Utils.SizeX || y >= Utils.SizeY) return false;
        return true;
    }
}

public class Bishop : Piece
{
    public override float PosOffset { get; protected set; } = 0.21f;
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
    public override float PosOffset { get; protected set; } = 0.22f;
    public Knight(Vector2Int initpos, Player owner) : base(initpos, owner)
    {
        Offsets = new()
        {
            new(1,2), new(-1,2), new(1,-2), new(-1,-2),
            new(2,1), new(2,-1), new(-2,1), new(-2,-1)
        };
    }
}

public class Jump : Piece
{
    public override float PosOffset { get; protected set; } = 0.23f;
    public Jump(Vector2Int initpos, Player owner) : base(initpos, owner)
    {
        Offsets = new() { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
    }
}