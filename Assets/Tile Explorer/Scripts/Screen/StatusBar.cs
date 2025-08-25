using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public static StatusBar Instance;
    public static event Action<int> CurrentLevel;
    public int currentCoin;
    public Button home;
    public Button setting;
    public Button shop;
    public TMP_Text scoreCoin;
    public SettingsScreen settingsScreen;
    public HomeScreen homeScreen;
    public PlayScreen playScreen;
    public ShopScroller shopScreen;
    public GameObject statusCoin;
    public TMP_Text textLevel;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        home.onClick.AddListener((OnHome));
        setting.onClick.AddListener((OnSetting));
        shop.onClick.AddListener(OnShop);

    }

    private void OnShop()
    {
        shopScreen.gameObject.SetActive(true);
    }
    private void OnHome()
    {
        homeScreen.gameObject.SetActive(true);
        TileManager.Instance.gameObject.SetActive(false);
        home.gameObject.SetActive(false);
        textLevel.gameObject.SetActive(false);
       // statusCoin.SetActive(false);
        playScreen.gameObject.SetActive(false);
        BoardTileCollector.Instance.gameObject.SetActive(false);
        CurrentLevel?.Invoke(TileManager.Instance.currentLevel);
        AudioManager.Instance.StopBgm();
        AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
    }
    private void OnSetting()
    {
        settingsScreen.gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
    }
}

public enum TypeScreen
{
    HomeScreen,
    PlayScreen,
    NextScreen,
    ReviveScreen
}
