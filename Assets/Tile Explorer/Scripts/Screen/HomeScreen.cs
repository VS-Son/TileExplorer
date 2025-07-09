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
    private int currentLevel = 1;
    private void Start()
    {
        starGame.onClick.AddListener((() => ButtonStart()));
//        textCurrentLevel.text = "Level " + TileManager.Instance.currentLevel;

    }

    private void ButtonStart()
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
