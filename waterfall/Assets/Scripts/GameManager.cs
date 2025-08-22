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