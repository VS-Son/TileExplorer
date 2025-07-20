using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using Button = UnityEngine.UI.Button;
using Object = System.Object;
using Slider = UnityEngine.UI.Slider;

public class NextScreen : MonoBehaviour
{
   [Header("Button")]
   [SerializeField] private Button buttonNextLevel;
   [SerializeField] private Button buttonSetting;
   [Header("Text")]
   [SerializeField] private TMP_Text textCompleteLevel;
   [SerializeField] private TMP_Text textNextLevel;
   [SerializeField] private TMP_Text textProvence;
   [SerializeField] private TMP_Text textCoin;
   [Header("Screen")]
   [SerializeField] private SettingsScreen settingsScreen;
   [SerializeField] private PlayScreen playScreen;
   [Header("Object Locked")]
   [SerializeField] private GameObject lockedUndo, lockedMagicWand, lockedShuffle;
   [Header("Progression")]
   [SerializeField] private Slider slider;

   [SerializeField] private Animator animator;

   [SerializeField]
   private RectTransform iconRewards;

   private int _countProgression = 0;
   
   private int _nextLevel;

   private void Start()
   {
      buttonNextLevel.onClick.AddListener((OnNextLevel));
      buttonSetting.onClick.AddListener(OnSetting);
   }
   private void OnEnable()
   {
      BoardTileCollector.completeLevel += CompleteLevel;
   }
   private void OnDisable()
   {
      BoardTileCollector.completeLevel -= CompleteLevel;
   }
   
   private void CompleteLevel(int level)
   {
      _nextLevel = level;
      textCompleteLevel.text = "Level " + _nextLevel;
      textNextLevel.text = "Level " + (_nextLevel + 1);
   }

   private void OnNextLevel()
   {
      playScreen.ShowStatusBar(_nextLevel + 1);
      if (_nextLevel + 1 > 1)
      {
          lockedUndo.SetActive(false);
      }
      if (_nextLevel + 1 > 2)
      {
          lockedShuffle.SetActive(false);  
          var color = playScreen.shuffle.image.color;
          color.a = 1;
          playScreen.shuffle.image.color = color;
          var colorIcon = playScreen.iconShuffle.color;
          colorIcon.a = 1;
          playScreen.iconShuffle.color = colorIcon;
      }
      if (_nextLevel + 1 > 3)
      {
          lockedMagicWand.SetActive(false);
          var color = playScreen.magicWand.image.color;
          color.a = 1;
          playScreen.magicWand.image.color = color;
          var colorIcon = playScreen.iconMagicWand.color;
          colorIcon.a = 1;
          playScreen.iconMagicWand.color = colorIcon;
      }
      BoardTileCollector.Instance.gameObject.SetActive(true);
      playScreen.gameObject.SetActive(true);
      this.gameObject.SetActive(false);
      TileManager.Instance.NextLevel();
   }

   private void OnSetting()
   {
      settingsScreen.gameObject.SetActive(true);
   }
   public void ProgressionRewards()
   {
      _countProgression++;
      textProvence.text = $"Provence {_countProgression}/4";

      var count = slider.maxValue / 4f;
      float targetValue = Mathf.Min(slider.value + count, slider.maxValue);

      StartCoroutine(FillSlider(slider.value, targetValue, 0.5f)); 
      
      Debug.LogError(count);
   }

   IEnumerator FillSlider(float from, float to, float duration)
   {
      float elapsed = 0f;
      while (elapsed < duration)
      {
         elapsed += Time.deltaTime;
         slider.value = Mathf.Lerp(from, to, elapsed / duration);
         yield return null;
      }
      slider.value = to;
      if (to >= slider.maxValue)
      {
         slider.gameObject.SetActive(false);
         StartCoroutine(MoveRewards());
         slider.value = 0;
         _countProgression = 0;
         textProvence.text = $"Provence {_countProgression}/4";
      }
   }

   IEnumerator MoveRewards()
   {
      animator.enabled = false;
      var elapsed = 0f;
      var duration = 0.6f;
     // iconRewards.localScale = new Vector3(1f,1f);

      while (elapsed < duration)
      {
        // iconRewards.localScale = new Vector3(1f,1f);
         elapsed += Time.deltaTime;
         var t = Mathf.Clamp01(elapsed / duration);
         var pos = Vector2.Lerp(iconRewards.transform.position, new Vector2(-540, 15), t);
         pos.y -= 100f * 4 * t * (1 - t);
         iconRewards.anchoredPosition = pos;
         yield return null;
      }

      iconRewards.anchoredPosition = new Vector2(-540, 15);
   }
}
