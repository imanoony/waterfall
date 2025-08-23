using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

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
    public static int MAX_HIT = 8; // 한번에 생길 수 있는 최대 Hit 수
    public static float ALPHA_HIGH = 1; // 불투명하게 만들 때 alpha 값
    public static float ALPHA_LOW = 0.2f; // 반투명하게 만들 때 alpha 값

    // pos 정보를 확인하고 Piece가 위치해야 할 위치 벡터를 반환한다.
    public static Vector2 PosToIso(Vector2Int pos)
    {
        Assert.IsFalse(pos.x < 0 || pos.y < 0 || pos.x > Utils.SizeX || pos.y > Utils.SizeY);

        float isoX = pos.x * Utils.ISO_STEP + pos.y * (-1) * Utils.ISO_STEP + Utils.BASE_POSITION.x;
        float isoY = (pos.x + pos.y) * Utils.ISO_STEP / 2 + Utils.BASE_POSITION.y;

        Debug.Log($"PosToIso 결과: ({pos.x}, {pos.y}) -> ({isoX}, {isoY})");
        return new(isoX, isoY);
    }

    // pos 정보를 확인하고 Piece가 위치해야 할 레이어를 반환한다.
    public static int PosToLayer(Vector2Int pos)
    {
        Assert.IsFalse(pos.x < 0 || pos.y < 0 || pos.x > Utils.SizeX || pos.y > Utils.SizeY);

        int layer = Utils.BASE_LAYER - (pos.x + pos.y);
        Debug.Log($"PosToLayer 결과: ({pos.x}, {pos.y}) -> {layer}");
        return layer;
    }
}

public class GameManager : MonoBehaviour
{
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
    private PieceBehaviour pieceBehaviour;
    public battleManager battleManager;

    private bool isSelectingPiece =false; // 클릭 취소 가능 여부 확인
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
        currentPiece = null;

        // for test
        InitGame();
    }

    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private GameObject hitPrefab;
    private GameObject[] hits = new GameObject[8];

    // 각각 7개의 White Pawn, Block Pawn을 알맞은 위치에 스폰한다.
    // 스테이지 시작 시 호출된다. 
    public void InitGame()
    {
        // battleManager Tilemap 세팅
        for (int i = 0; i < Utils.SizeX+1; i++)
        {
            for (int j = 0; j < Utils.SizeY+1; j++)
            {
                battleManager.Map[i, j] = new tile();
            }
        }
        // 루프를 두 번 돌 필요는 없지만 X, Y의 분류에 대한 통일성을 위해 두개로
        for (int i = 1; i < Utils.SizeX; i++)
        {
            Pawn pawn = new(new(0, i), Player.White);
            GameObject obj = Instantiate(piecePrefab);
            obj.GetComponent<PieceBehaviour>().Init(pawn);
            battleManager.Map[pawn.Pos.x, pawn.Pos.y].piece = pawn;
        }
        for (int i = 1; i < Utils.SizeY; i++)
        {
            Pawn pawn = new(new(i, 0), Player.Black);
            GameObject obj = Instantiate(piecePrefab);
            obj.GetComponent<PieceBehaviour>().Init(pawn);
            battleManager.Map[pawn.Pos.x, pawn.Pos.y].piece = pawn;
        }

        // Hit 오브젝트 풀 생성
        for (int i = 0; i < Utils.MAX_HIT; i++)
        {
            hits[i] = Instantiate(hitPrefab);
            hits[i].SetActive(false);
        }

        foreach (TooltipData data in uiManager.tooltips)
        {
            uiManager.tooltipDict.Add(data.Name, data);
        }
    }

    /// <summary>
    /// 특정 piece를 선택할 경우 시점을 그 piece로 맞추어주는 함수
    /// </summary>
    /// <param name="selected"></param>
    public void selectPiece(Piece selected, PieceBehaviour pieceBehaviour)
    {
        this.pieceBehaviour = pieceBehaviour;
        currentPiece = selected;
        uiManager.PieceMode(selected);
        PlaceHits(battleManager.getPossiblePosition(selected));
        isSelectingPiece = true;
    }

    public void StopSelecting()
    {
        if (!isSelectingPiece)
        {
            return;
        }
        currentPiece = null;
        pieceBehaviour = null;
        uiManager.MainCameraMode();
        isSelectingPiece = false;
        RemoveHits();
    }
    /// <summary>
    /// 특정 위치를 선택할 경우 그 위치로 piece를 이동하는 함수
    /// </summary>
    /// <param name="position"></param>
    public void selectPosition(Vector2Int position)
    {
        isSelectingPiece = false;
        if (currentPiece != null)
        {
            battleManager.Map[currentPiece.Pos.x, currentPiece.Pos.y].piece = null;
            battleManager.Map[position.x, position.y].piece = currentPiece;
            currentPiece.SetPos(position);
        }

        if (currentPiece is Pawn pawn)
        {
            pawn.AddStep();
        }
        uiManager.SkillMode(currentPiece);
        RemoveHits();
    }
    /// <summary>
    /// 특정 스킬을 사용할 경우 발동하는 함수
    /// 폰 기준 주위에 있는 UI를 선택할 시 호출되며, 폰을 특정 직업으로 전직시켜준다.
    /// </summary>
    public void selectPawn(int Type)
    {
        if (currentPiece != null && currentPiece is Pawn pawn)
        {
            Type newType;
            switch (Type)
            {
                case 0:
                    newType = typeof(AdultPawn);
                    break;
                case 1:
                    newType = typeof(Bishop);
                    break;
                case 2:
                    newType = typeof(Jump);
                    break;
                case 3:
                    newType = typeof(Knight);
                    break;
                case 4:
                    newType = typeof(God);
                    break;
                default:
                    newType = typeof(Pawn);
                    break;
            }

            if (newType == typeof(Pawn))
            {
                endTurn();
                return;
            }
            Piece newPiece = pawn.Transform(newType);
            if (newPiece != null)
            {
                battleManager.Map[pawn.Pos.x, pawn.Pos.y].piece = newPiece;
                pieceBehaviour.Init(newPiece);
                endTurn();
            }
        }
    }

    public void selectGod(int Pos)
    {
        if (currentPiece != null && currentPiece is God god)
        {
            Vector2Int deltaPos;
            switch (Pos)
            {
                case 0: // 위
                    deltaPos = new Vector2Int(0, -1);
                    break;
                case 1: // 좌상
                    deltaPos = new Vector2Int(-1, -1);
                    break;
                case 2: // 좌
                    deltaPos = new Vector2Int(-1, 0);
                    break;
                case 3: // 좌하
                    deltaPos = new Vector2Int(-1, 1);
                    break;
                case 4: // 아래
                    deltaPos = new Vector2Int(0, 1);
                    break;
                case 5: // 우하
                    deltaPos = new Vector2Int(1, 1);
                    break;
                case 6: // 우
                    deltaPos = new Vector2Int(1, 0);
                    break;
                case 7: // 우상
                    deltaPos = new Vector2Int(1, -1);
                    break;
                default:
                    god.Target = null;
                    god.skillPhase = 0;
                    endTurn();
                    return;
            }

            if (currentPiece.CheckPos(currentPiece.Pos.x + deltaPos.x, currentPiece.Pos.y + deltaPos.y))
            {
                if (god.skillPhase == 0)
                {
                    if (battleManager.Map[currentPiece.Pos.x + deltaPos.x, currentPiece.Pos.y + deltaPos.y].piece != null
                        && battleManager.Map[currentPiece.Pos.x + deltaPos.x, currentPiece.Pos.y + deltaPos.y].piece.Owner != god.Owner)
                    {
                        god.Target = battleManager.Map[currentPiece.Pos.x + deltaPos.x, currentPiece.Pos.y + deltaPos.y]
                            .piece;
                        god.skillPhase = 1;
                    }
                }
                else if (god.skillPhase == 1)
                {
                    if (battleManager.Map[currentPiece.Pos.x + deltaPos.x, currentPiece.Pos.y + deltaPos.y].piece ==
                        null && god.Target != null)
                    {
                        battleManager.Map[god.Target.Pos.x,god.Target.Pos.y].piece = null;
                        battleManager.Map[currentPiece.Pos.x + deltaPos.x, currentPiece.Pos.y + deltaPos.y].piece =
                            god.Target;
                        god.Target.SetPos(new Vector2Int(currentPiece.Pos.x + deltaPos.x, currentPiece.Pos.y + deltaPos.y));
                        god.Target = null;
                        god.skillPhase = 0;
                        endTurn();
                    }
                }
            }
        }

    }
    /// <summary>
    /// 현재 턴을 다음 차례에게 넘긴다.
    /// </summary>
    public void endTurn()
    {
        if (currentPlayer == Player.White)
        {
            currentPlayer = Player.Black;
        }
        else
        {
            currentPlayer = Player.White;
        }

        currentPiece = null;
        uiManager.MainCameraMode();
    }

    private void PlaceHits(List<Vector2Int> posList)
    {
        for (int i = 0; i < posList.Count; i++)
        {
            hits[i].GetComponent<HitBehaviour>().Init(posList[i]);
        }
    }

    private void RemoveHits()
    {
        for (int i = 0; i < hits.Length; i++) hits[i].SetActive(false);
    }
}