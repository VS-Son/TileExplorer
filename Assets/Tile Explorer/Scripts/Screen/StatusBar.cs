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
    public int coin;
    [Header("Button")]
    public Button home;
    public Button setting;
    public Button shop;
    [Header("Screen")]
    public SettingsScreen settingsScreen;
    public HomeScreen homeScreen;
    public PlayScreen playScreen;
    public ShopScroller shopScreen;
    public GameObject statusCoin;
    public TMP_Text textLevel;
    [SerializeField] private TMP_Text textCoin;

    public int currentCoin
    {
        get => coin;
        set
        {
            coin = value;
            textCoin.text = FormatCoin(coin);
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        home.onClick.AddListener((OnHome));
        setting.onClick.AddListener((OnSetting));
        shop.onClick.AddListener(OnShop);
        textCoin.text = FormatCoin(coin);

    }

    private void OnShop()
    {
        shopScreen.gameObject.SetActive(true);
        ShopManager.Instance.valueCoin.text = FormatCoin(currentCoin);
    }
    private void OnHome()
    {
        homeScreen.gameObject.SetActive(true);
        TileManager.Instance.gamePlayTransform.gameObject.SetActive(false);
        home.gameObject.SetActive(false);
        textLevel.gameObject.SetActive(false);
       // statusCoin.SetActive(false);
        playScreen.boosters.SetActive(false);
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
}

public enum TypeScreen
{
    HomeScreen,
    PlayScreen,
    NextScreen,
    ReviveScreen
}
