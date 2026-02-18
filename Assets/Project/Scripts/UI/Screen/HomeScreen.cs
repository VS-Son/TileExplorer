using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Scripts.Game;
using Project.Scripts.Sound;
using Project.Scripts.UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Screen
{
    public class HomeScreen : UICanvas
    {
        [SerializeField] private Transform onPlay;
        [SerializeField] private TMPro.TMP_Text textCurrentLevel;
        [SerializeField] private List<RectTransform> tileEffect = new List<RectTransform>();
        [SerializeField] private Image bannerExplorer;
        private int _currentLevel = 1;
        private void Start()
        {
            foreach (var tileIndex in tileEffect)
            {
                tileIndex.transform.localScale = new Vector3(0, 0);
            }
            onPlay.localScale = Vector3.zero;
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
            tile.DOScale(new Vector3(1.23f, 1.23f), 0.25f)
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
            onPlay.DOScale(1, 0.4f);
        }

        public void OnPlay()
        {
            UIManager.Instance.GetUI<StatusBar>().SetActiveStatus(true);
            UIManager.Instance.GetUI<StatusBar>().textLevel.text = "Level" + _currentLevel;
            GameState.ChangeState(StateUI.PlayScreen);
            DOVirtual.DelayedCall(0.3f, () => { AudioManager.Instance.PlayBgm(AudioConstants.BGM, 3f); });
            AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
        }

        private void OnEnable()
        {
            StatusBar.CurrentLevel += CurrentLevel;
        }
        private void OnDisable()
        {
            StatusBar.CurrentLevel -= CurrentLevel;
        }
   
        private void CurrentLevel(int level)
        {
            _currentLevel = level;
            textCurrentLevel.text = "Level " + _currentLevel;
        }

    }
}
