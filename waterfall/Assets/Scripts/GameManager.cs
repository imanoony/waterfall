using System;
using UnityEngine;

public enum Player { White = 0, Black = 1 }

public static class Utils
{
    public static int SizeX = 8;
    public static int SizeY = 8;
    public static Vector2 BasePosition = new(0, -1.75f);
    public static int A_THRESHOLD = 2; // AdultPawn 형태 변환에 대한 역치
    public static int G_THRESHOLD = 4; // God 형태 변화에 대한 역치
    public static int B_THRESHOLD = 2; // Bishop 형태 변화에 대한 역치
    public static int K_THRESHOLD = 4; // Knight 형태 변화에 대한 역치
    public static int J_THRESHOLD = 3; // Jump 형태 변화에 대한 역치
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