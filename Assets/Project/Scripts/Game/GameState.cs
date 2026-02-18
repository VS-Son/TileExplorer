using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Project.Scripts.Manager;
using Project.Scripts.Tile;
using Project.Scripts.UI.Manager;
using Project.Scripts.UI.Screen;
using UnityEngine;

namespace Project.Scripts.Game
{
    public enum StateUI { HomeScreen, PlayScreen, GameOver, ReviveScreen, Setting, Shop, StatusBar, NextLevel }

    public class GameState : Singleton<GameState>
    {
        private static StateUI s_GameState;
        [SerializeField] private GameObject gameplay, boardTile;
        
        private void Awake()
        {
            Input.multiTouchEnabled = false;
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            int maxScreenHeight = 1280;
            float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
            if (Screen.currentResolution.height > maxScreenHeight)
            {
                Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
            }
            ChangeState(StateUI.HomeScreen);
        }
        public static void ChangeState(StateUI state)
        {
            s_GameState = state;
            Instance.OnGameStateChanged(state);
        }

        public static bool IsState(StateUI state) => s_GameState == state;
        private void OnGameStateChanged(StateUI newState)
        {
            switch (newState)
            {
                case StateUI.HomeScreen:
                    UIManager.Instance.OpenUI<HomeScreen>();
                    UIManager.Instance.CloseUI<PlayScreen>();
                    SetActiveObject(false);
                    break;
                case StateUI.PlayScreen:
                    UIManager.Instance.OpenUI<PlayScreen>();
                    UIManager.Instance.CloseUI<HomeScreen>();
                    SetActiveObject(true);
                    UIManager.Instance.GetUI<StatusBar>().home.gameObject.SetActive(true);
                    break;
                case StateUI.Shop:
                    UIManager.Instance.OpenUI<ShopScreen>();
                    break;
                case StateUI.Setting:
                    UIManager.Instance.OpenUI<SettingsScreen>();
                    break;
                case StateUI.StatusBar:
                    UIManager.Instance.OpenUI<StatusBar>(); 
                    break;
                case StateUI.NextLevel:
                    UIManager.Instance.OpenUI<NextScreen>();
                    UIManager.Instance.GetUI<NextScreen>().transform.SetAsFirstSibling();

                    SetActiveObject(false);
                    UIManager.Instance.CloseUI<PlayScreen>();
                    break;
                case StateUI.ReviveScreen:
                    UIManager.Instance.OpenUI<ReviveScreen>();
                    break;
                case StateUI.GameOver:
                    UIManager.Instance.OpenUI<GameOverScreen>();
                    break;
            }
        }

        private void SetActiveObject(bool isActive)
        {
            gameplay.gameObject.SetActive(isActive);
            boardTile.gameObject.SetActive(isActive);
        }
    }
}