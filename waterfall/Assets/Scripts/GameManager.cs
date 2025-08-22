using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public enum Player { White = 0, Black = 1 }

public static class Utils
{
    public static int SizeX = 8;
    public static int SizeY = 8;
    public static Vector2 BASE_POSITION = new(0, -1.75f);
    public static int A_THRESHOLD = 2; // AdultPawn 형태 변환에 대한 역치
    public static int G_THRESHOLD = 4; // God 형태 변화에 대한 역치
    public static int B_THRESHOLD = 2; // Bishop 형태 변화에 대한 역치
    public static int K_THRESHOLD = 4; // Knight 형태 변화에 대한 역치
    public static int J_THRESHOLD = 3; // Jump 형태 변화에 대한 역치
    public static float ISO_STEP = 0.5f; // 좌표 <-> 위치 벡터 변환을 위한 값
    public static int BASE_LAYER = 15; // Piece가 위치할 기준 레이어 (최대)

    // pos 정보를 확인하고 Piece가 위치해야 할 위치 벡터를 반환한다.
    public static Vector2 PosToIso(Vector2Int pos)
    {
        Assert.IsTrue(pos.x < 0 || pos.y < 0 || pos.x > Utils.SizeX || pos.y > Utils.SizeY);

        float isoX = pos.x * Utils.ISO_STEP + pos.y * (-1) * Utils.ISO_STEP + Utils.BASE_POSITION.x;
        float isoY = (pos.x + pos.y) * Utils.ISO_STEP + Utils.BASE_POSITION.y;

        Debug.Log($"PosToIso 결과: ({pos.x}, {pos.y}) -> ({isoX}, {isoY})");
        return new(isoX, isoY);
    }

    // pos 정보를 확인하고 Piece가 위치해야 할 레이어를 반환한다.
    public static int PosToLayer(Vector2Int pos)
    {
        Assert.IsTrue(pos.x < 0 || pos.y < 0 || pos.x > Utils.SizeX || pos.y > Utils.SizeY);

        int layer = Utils.BASE_LAYER - (pos.x + pos.y);
        Debug.Log($"PosToLayer 결과: ({pos.x}, {pos.y}) -> {layer}");
        return layer;
    }
}

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            // 인스턴스 없으면 씬에서 찾아보기
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                
                // 없으면 새로 생성
                if (instance == null)
                {
                    GameObject singletonObj = new GameObject(nameof(GameManager));
                    instance = singletonObj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }
    public Player currentPlayer = Player.White; // 현재 플레이어의 턴 판단
    public UIManager uiManager; // 
    public Piece currentPiece;
    public battleManager battleManager;

    // 저장 순서는 P, A, G, B, K, J
    [SerializeField] private Sprite[] whiteSprites = new Sprite[6];
    [SerializeField] private Sprite[] blackSprites = new Sprite[6];
    public Sprite GetSprite(Piece piece)
    {
        Sprite[] target = piece.Owner == Player.White ? whiteSprites : blackSprites;
        if (piece is Pawn) return target[0];
        if (piece is AdultPawn) return target[1];
        if (piece is God) return target[2];
        if (piece is Bishop) return target[3];
        if (piece is Knight) return target[4];
        return target[5]; 
    }

    public void Start()
    {
        uiManager.MainCameraMode();
    }

    /// <summary>
    /// 특정 piece를 선택할 경우 시점을 그 piece로 맞추어주는 함수
    /// </summary>
    /// <param name="selected"></param>
    public void selectPiece(Piece selected){
        currentPiece = selected;
        uiManager.PieceMode(selected);
    }
    /// <summary>
    /// 특정 위치를 선택할 경우 그 위치로 piece를 이동하는 함수
    /// </summary>
    /// <param name="position"></param>
    public void selectPosition(Vector2Int position){
        if(currentPiece != null){
            currentPiece.SetPos(position);
        }
        uiManager.SkillMode(currentPiece);
    }
    /// <summary>
    /// 특정 스킬을 사용할 경우 발동하는 함수
    /// </summary>
    public void selectSkill(){
        
    }
    /// <summary>
    /// 현재 턴을 다음 차례에게 넘긴다.
    /// </summary>
    public void endTurn(){
        if (currentPlayer == Player.White)
        {
            currentPlayer = Player.Black;
        }
        else
        {
            currentPlayer = Player.White;
        }
        uiManager.MainCameraMode();
    }
}