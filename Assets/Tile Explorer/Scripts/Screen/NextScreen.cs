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
using UnityEngine.Playables;
using Image = UnityEngine.UI.Image;

public class NextScreen : MonoBehaviour
{
   [Header("Button")]
   [SerializeField] private Button buttonNextLevel;
   [SerializeField] private Button buttonSetting;
   [Header("Text")]
   [SerializeField] private TMP_Text textCompleteLevel;
   [SerializeField] private TMP_Text textNextLevel;
   [SerializeField] private TMP_Text textProvence;
   [SerializeField] private TMP_Text quantityText;                  
   [Header("Screen")]
   [SerializeField] private PlayScreen playScreen;
   [Header("Object Locked")]
   [SerializeField] private GameObject lockedUndo, lockedMagicWand, lockedShuffle;
   [Header("Progression")]
   [SerializeField] private Slider slider;
   [Header("Animator")]
   [SerializeField] private Animator animator;

   [SerializeField] private Image iconRewards;
   [SerializeField]
   private RectTransform effectCoin;

   [Header("Status bar")] [SerializeField]
   private int scoreQuantity = 30;
   private int _countProgression = 2;
   private int _nextLevel;

   private void Start()
   {
      buttonNextLevel.onClick.AddListener((OnNextLevel));
   }
   private void OnEnable()
   {
      BoardTileCollector.completeLevel += CompleteLevel;
      CoinEffect.onCompleteGoal += OnCompleteGoal;
   }
   private void OnDisable()
   {
      BoardTileCollector.completeLevel -= CompleteLevel;
      CoinEffect.onCompleteGoal -= OnCompleteGoal;

   }
   
   private void CompleteLevel(int level)
   {
      _nextLevel = level;
      textCompleteLevel.text = "Level " + _nextLevel;
      textNextLevel.text = "Level " + (_nextLevel + 1);
   }

   private void OnNextLevel()
   {
      StatusBar.Instance.textLevel.text = "Level" + (_nextLevel + 1);
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
      StatusBar.Instance.home.gameObject.SetActive(true);
      StatusBar.Instance.statusCoin.SetActive(true);
      StatusBar.Instance.textLevel.gameObject.SetActive(true);
      TileManager.Instance.NextLevel();
      if (_countProgression.Equals(0))
      {
         slider.gameObject.SetActive(true);
         iconRewards.gameObject.SetActive(true);
         iconRewards.rectTransform.anchoredPosition = new Vector3(-225,15);
         Sprite sprite = Resources.Load<Sprite>("Icon/Gift");
         iconRewards.sprite = sprite;
         iconRewards.transform.localScale = new Vector3(1,1);
         iconRewards.rectTransform.sizeDelta = new Vector2(130, 130);
      }
      
   }
   
   public void ProgressionRewards()
   {
      _countProgression++;
      textProvence.text = $"Provence {_countProgression}/4";
      buttonNextLevel.gameObject.SetActive(false);
         buttonNextLevel.transform.localScale = Vector2.zero;
         buttonNextLevel.enabled = false;
         var count = slider.maxValue / 4f;
      float targetValue = Mathf.Min(slider.value + count, slider.maxValue);

      StartCoroutine(FillSlider(slider.value, targetValue, 0.5f)); 
      
     // Debug.LogError(count);
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
      if (_countProgression <= 3)
      {
         OnCompleteGoal();
      }
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
      var elapsed = 0f;
      var duration = 0.6f;

      while (elapsed < duration)
      {
        iconRewards.transform.DOScale(1.5f, duration);
         elapsed += Time.deltaTime;
         var t = Mathf.Clamp01(elapsed / duration);
         var pos = Vector2.Lerp(iconRewards.transform.position, new Vector2(-540, 15), t);
         pos.y += 100f * 4 * t * (1 - t);
         iconRewards.rectTransform.anchoredPosition = pos;
         yield return null;
      }

      iconRewards.rectTransform.anchoredPosition = new Vector2(-540, 15);
      animator.enabled = true;
      StartCoroutine(CheckRewards());
   }

   IEnumerator CheckRewards()
   {
      while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || animator.IsInTransition(0))
      {
         yield return null;
      }
      animator.enabled = false;
      iconRewards.gameObject.SetActive(false);
      effectCoin.gameObject.SetActive(true);
      quantityText.text =  scoreQuantity.ToString();
      DOVirtual.DelayedCall(0.5f, (() =>
      {
         effectCoin.gameObject.SetActive(false);
         FindObjectOfType<CoinEffect>().RewardPlayer(scoreQuantity);

      }));
   }

   private void OnCompleteGoal()
   {
      buttonNextLevel.gameObject.SetActive(true);
      buttonNextLevel.transform.DOScale(1, 0.6f).OnComplete(()=>
      {
         buttonNextLevel.enabled = true;
      });
   }

   
}
