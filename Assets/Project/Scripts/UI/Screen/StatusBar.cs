using System;
using Project.Scripts.Game;
using Project.Scripts.Sound;
using Project.Scripts.Tile;
using Project.Scripts.UI.Manager;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.Screen
{
    public class StatusBar : UICanvas
    {
        public static event Action<int> CurrentLevel;
        public int coin;
        [Header("Button")] public GameObject home;
        public TMP_Text textLevel;
        public TMP_Text textCoin;
        public Transform iconCoin;
        public int currentCoin
        {
            get => coin;
            set
            {
                coin = value;
                textCoin.text = FormatCoin(coin);
            }
        }

  

        private void Start()
        {
            textCoin.text = FormatCoin(coin);
        }

        public void OnShop()
        {
            GameState.ChangeState(StateUI.Shop);
            UIManager.Instance.GetUI<ShopScreen>().FormatCoin(coin);
        }

        public void OnHome()
        {
            GameState.ChangeState(StateUI.HomeScreen);
            home.gameObject.SetActive(false);
            textLevel.gameObject.SetActive(false);
            CurrentLevel?.Invoke(TileManager.Instance.currentLevel);
            AudioManager.Instance.StopBgm();
            AudioManager.Instance.PlaySfx(AudioConstants.HighPitchDefault);
        }

        public void OnSetting()
        {
            GameState.ChangeState(StateUI.Setting);
            AudioManager.Instance.PlaySfx(AudioConstants.HighPitchDefault);
        }

        public void UpdateCoin(int coinCount)
        {
            currentCoin += coinCount;
        }

        public string FormatCoin(int value)
        {
            if (value >= 1000)
            {
                return (value / 1000) + "K";
            }

            return value.ToString();
        }

        public void SetActiveStatus(bool isActive)
        {
            home.SetActive(isActive);
            textLevel.gameObject.SetActive(isActive);
        }
    }
}
