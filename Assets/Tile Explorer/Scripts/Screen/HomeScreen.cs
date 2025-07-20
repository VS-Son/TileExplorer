using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class HomeScreen : MonoBehaviour
{
    [SerializeField] private PlayScreen gamePlay;
    [SerializeField] private Button starGame;
    [SerializeField] private TMPro.TMP_Text textCurrentLevel;
    [SerializeField] private GameObject tileManager;
    [SerializeField] private GameObject tileCollector;
    [SerializeField] private Button setting;
    [SerializeField] private SettingsScreen settingsScreen;
    [SerializeField] private List<RectTransform> tileEffect = new List<RectTransform>();
    [SerializeField] private Image bannerExplorer;
    private int currentLevel = 1;
    private void Start()
    {
        starGame.onClick.AddListener((OnStart));
        setting.onClick.AddListener(OnSettings);

        foreach (var tileIndex in tileEffect)
        {
            tileIndex.transform.localScale = new Vector3(0, 0);
        }
    
        StartCoroutine(ScaleTilesSequentially());
    }

    IEnumerator ScaleTilesSequentially(int index = 0)
    {
        if (index >= tileEffect.Count)
        {
            StartCoroutine(OnBanner());
            yield break;
        }

        var tile = tileEffect[index];
        tile.DOScale(new Vector3(1.23f, 1.23f), 0.1f)
            .OnComplete(() => { StartCoroutine(ScaleTilesSequentially(index + 1)); });

        yield return null;
    }

    IEnumerator OnBanner()
    {
        var duration = 0.005f;
        while (bannerExplorer.fillAmount < 1f)
        {
            bannerExplorer.fillAmount += 0.01f;
            float clampedTime = Mathf.Clamp01(bannerExplorer.fillAmount);
            bannerExplorer.fillAmount = clampedTime;
            yield return new WaitForSeconds(duration);
        }
        //starGame.transform.DOScale(1, 0.4f);
    }

    private void OnSettings()
    {
        settingsScreen.gameObject.SetActive(true);
    }

    private void OnStart()
    {
        gamePlay.ShowStatusBar(currentLevel);
        tileManager.gameObject.SetActive(true);
        tileCollector.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        DOVirtual.DelayedCall(0.3f, () =>
        {
            AudioManager.Instance.PlayBgm("bgm",3f);

        });
        AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
    }

    private void OnEnable()
    {
        PlayScreen.CurrentLevel += CurrentLevel;
    }
    private void OnDisable()
    {
        PlayScreen.CurrentLevel -= CurrentLevel;
    }
   
    private void CurrentLevel(int level)
    {
        currentLevel = level;
        textCurrentLevel.text = "Level " + currentLevel;
    }

}
