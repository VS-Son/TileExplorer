using System;
using System.Collections;
using System.Collections.Generic;
using Tile_Explorer.Scripts.Tile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        GameManager.ChangeState(GameState.Shop);
        UIManager.Instance.GetUI<ShopScreen>().FormatCoin(coin);
    }

    public void OnHome()
    {
        GameManager.ChangeState(GameState.HomeScreen);
        home.gameObject.SetActive(false);
        textLevel.gameObject.SetActive(false);
        CurrentLevel?.Invoke(TileManager.Instance.currentLevel);
        AudioManager.Instance.StopBgm();
        AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
    }

    public void OnSetting()
    {
        GameManager.ChangeState(GameState.Setting);
        AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
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
