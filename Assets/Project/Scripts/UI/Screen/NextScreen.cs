using System.Collections;
using DG.Tweening;
using Project.Scripts.Tile;
using Tile_Explorer.Scripts.Tile;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;
using Image = UnityEngine.UI.Image;

namespace Project.Scripts.UI.Screen
{
   public class NextScreen : UICanvas
   {
      [Header("Button")]
      [SerializeField] private Button buttonNextLevel;
      [Header("Text")]
      [SerializeField] private TMP_Text textCompleteLevel;
      [SerializeField] private TMP_Text textNextLevel;
      [SerializeField] private TMP_Text textProvence;
      [SerializeField] private TMP_Text quantityText;                  
      [Header("Progression")]
      [SerializeField] private Slider slider;
      [Header("Animator")]
      [SerializeField] private Animator animator;

      [SerializeField] private Image iconRewards;
      [SerializeField]
      private RectTransform effectCoin;

      [Header("Status bar")] [SerializeField]
      private int scoreQuantity = 30;
      private int _countProgression = 0;
      private int _nextLevel;
   
      private void OnEnable()
      {
         BoardTileCollector.completeLevel += CompleteLevel;
         CoinEffect.OnCompleteGoal += OnCompleteGoal;
      }
      private void OnDisable()
      {
         BoardTileCollector.completeLevel -= CompleteLevel;
         CoinEffect.OnCompleteGoal -= OnCompleteGoal;

      }
   
      private void CompleteLevel(int level)
      {
         _nextLevel = level;
         textCompleteLevel.text = "Level " + _nextLevel;
         textNextLevel.text = "Level " + (_nextLevel + 1);
      }

      public void OnNextLevel()
      {
         AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
         UIManager.Instance.GetUI<StatusBar>().textLevel.text = "Level" + (_nextLevel + 1);
         UIManager.Instance.GetUI<PlayScreen>().UnlockFeature(_nextLevel + 1);
         GameManager.ChangeState(GameState.PlayScreen);
         CloseDirectly();
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
         UpdateProgressionData();
         UpdateProgressionUI();
         // Debug.LogError(count);
      }

      private void UpdateProgressionData()
      {
         _countProgression = Mathf.Min(_countProgression + 1, 4);
         var count = slider.maxValue / 4f;
         float targetValue = Mathf.Min(slider.value + count, slider.maxValue);
         StartCoroutine(FillSlider(slider.value, targetValue, 0.5f));
      }

      private void UpdateProgressionUI()
      {
         textProvence.text = $"Provence {_countProgression}/4";
         buttonNextLevel.gameObject.SetActive(false);
         buttonNextLevel.transform.localScale = Vector2.zero;
         buttonNextLevel.enabled = false;
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
            FindObjectOfType<CoinEffect>().RewardCoin(scoreQuantity);

         }));
      }

      private void OnCompleteGoalCoin()
      {
         buttonNextLevel.gameObject.SetActive(true);
         buttonNextLevel.transform.DOScale(1, 0.6f).OnComplete(()=>
         {
            buttonNextLevel.enabled = true;
         });
      }


  
   }
}
