using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Project.Scripts.Tile;
using Project.Scripts.UI.Screen;
using Tile_Explorer.Scripts.Tile;
using UnityEngine;

public enum GameState { HomeScreen, PlayScreen, GameOver, ReviveScreen, Setting, Shop, StatusBar, NextLevel }

public class GameManager : Singleton<GameManager>
{
    private static GameState _gameState;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private BoardTileCollector boardTileCollector;

    private void Awake()
    {
        //tranh viec nguoi choi cham da diem vao man hinh
        Input.multiTouchEnabled = false;
        //target frame rate ve 60 fps
        Application.targetFrameRate = 60;
        //tranh viec tat man hinh
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //xu tai tho
        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
    }

    // private void Start()
    // {
    //     ChangeState(GameState.HomeScreen);
    //     ChangeState(GameState.StatusBar);
    // }               

    public static void ChangeState(GameState state)
    {
        _gameState = state;
        Instance.OnGameStateChanged(state);
    }

    public static bool IsState(GameState state) => _gameState == state;
    private void OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.HomeScreen:
                UIManager.Instance.OpenUI<HomeScreen>();
                UIManager.Instance.CloseUI<PlayScreen>();
                tileManager.gameObject.SetActive(false);
                boardTileCollector.gameObject.SetActive(false);
                break;
            case GameState.PlayScreen:
                UIManager.Instance.OpenUI<PlayScreen>();
                UIManager.Instance.CloseUI<HomeScreen>();
                tileManager.gameObject.SetActive(true);
                boardTileCollector.gameObject.SetActive(true);
                UIManager.Instance.GetUI<StatusBar>().home.gameObject.SetActive(true);
                break;
            case GameState.Shop:
                UIManager.Instance.OpenUI<ShopScreen>();
                break;
            case GameState.Setting:
                UIManager.Instance.OpenUI<SettingsScreen>();
                break;
            case GameState.StatusBar:
                UIManager.Instance.OpenUI<StatusBar>(); 
                break;
            case GameState.NextLevel:
                UIManager.Instance.OpenUI<NextScreen>();
                BoardTileCollector.Instance.gameObject.SetActive(false);
                UIManager.Instance.CloseUI<PlayScreen>();
                break;
            case GameState.ReviveScreen:
                UIManager.Instance.OpenUI<ReviveScreen>();
                break;
            case GameState.GameOver:
                UIManager.Instance.OpenUI<GameOverScreen>();
                break;
        }
    }
}

