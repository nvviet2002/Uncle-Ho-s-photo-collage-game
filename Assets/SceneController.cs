using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;
    public static SceneController Instance { get => instance; set => instance = value; }

    private int puzzlePieceCounter;
    public string sceneName;
    public int sceneIndex;
    public int sceneGoal;
    public bool isPlaying;
    public int point;
    public float currentTime;

    private List<PuzzlePiece> puzzlePieces;

    private void Awake()
    {
        Instance = this;
        puzzlePieces = new List<PuzzlePiece>();
    }

    private void Start()
    {
        puzzlePieceCounter = 0;
        point = 3;
        isPlaying = false;
        currentTime = 0f;
    }

    private void Update()
    {
        CaculateTime();
    }

    private void CaculateTime()
    {
        if (isPlaying == false) return;
        currentTime += Time.deltaTime;
        if(currentTime < GameController.Instance.gameInfo.starTime)
        {
            point = 3;
        }else if(currentTime >= GameController.Instance.gameInfo.starTime && 
            currentTime < GameController.Instance.gameInfo.starTime * 2)
        {
            point = 2;
        }
        else
        {
            point = 1;
        }
        UIController.Instance.ChangeValueTimer(currentTime);
    }

    public void AddPuzzlePieceForList(PuzzlePiece _piece)
    {
        puzzlePieces.Add(_piece);
    }

    public void AddPuzzlePieceUnit(int _add)
    {
        puzzlePieceCounter += _add;
        CheckMaxCounter();
        GameController.Instance.EnableSnapSound(true);
    }

    private void CheckMaxCounter()
    {
        if (puzzlePieceCounter >= sceneGoal)
        {
            GameController.Instance.Win(sceneName,point);
        }
    }

    public void StartScene()
    {
        isPlaying = true;
        foreach(PuzzlePiece item in puzzlePieces)
        {
            item.MoveToSpawn();
        }
    }

}
