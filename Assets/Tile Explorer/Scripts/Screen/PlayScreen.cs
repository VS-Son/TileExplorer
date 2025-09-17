using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;
public class PlayScreen : MonoBehaviour
{
   public static event Action<int> CurrentLevel;
   public static PlayScreen Instance;
   [Header("Features")] public GameObject boosters;
   [Header("Level Unlock")]
   [SerializeField] private int levelUnlockUndo;
   [SerializeField] private int levelUnlockMagicWand;
   [SerializeField] private int levelUnlockShuffle;
   [Header("Button")]
   public Button undo;
   public Button magicWand;
   public Button shuffle;
   [Header("Icon")]
   public Image iconUndo;
   public Image iconMagicWand;
   public Image iconShuffle;
   [Header("Text")]
   [SerializeField] private TMP_Text textUndoCount;
   [SerializeField] private TMP_Text textMagicWandCount;
   [SerializeField] private TMP_Text textShuffleCount;
   public TMP_Text textCoinUndo;
   public TMP_Text textCoinMagicWand;
   public TMP_Text textCoinShuffle;
   [SerializeField] private TMP_Text content;
   [Space]
   [SerializeField] private RectTransform popupNotification;
   [Header("Locked")]
   [SerializeField] private GameObject lockedUndo;
   [SerializeField] private GameObject lockedMagicWand;
   [SerializeField] private GameObject lockedShuffle;
   [Header("Value")]
   [SerializeField] public GameObject valueUndo; 
   [SerializeField] public GameObject valueMagicWand; 
   [SerializeField] public GameObject valueShuffle;
   [Header("Coin")] 
   public GameObject coinUndo;
   public GameObject coinMagicWand;
   public GameObject coinShuffle;
   [Header("Ads")]
   public GameObject adsUndo;
   public GameObject adsMagicWand;
   public GameObject adsShuffle;
   [Header("Canvas Group")]
   public CanvasGroup undoGroup;
   public CanvasGroup magicWandGroup;
   public CanvasGroup shuffleGroup;
   [Header("Count")]
   public int currentUndoCount;
   public int currentMagicWandCount;
   public int currentShuffleCount;

   private Tween _moveTween, _fadeTween1,_fadeTween2, _scaleTween1, _scaleTween2;

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      undo.onClick.AddListener(OnUndo);
      magicWand.onClick.AddListener(MagicWand);
      shuffle.onClick.AddListener((OnShuffle));
   }


   private void OnUndo()
   {
      if (TileManager.Instance.currentLevel > 1)
      {
         switch (BoardTileCollector.Instance.collectedTiles.Count)
         {
            case 0:
               ShowPopupRequirement(2, NotificationType.TileMissing);
               break;
            case > 0:
            {
               if (currentUndoCount >= 1)
               {
                  valueUndo.SetActive(true);
                  currentUndoCount--;
                  BoardTileCollector.Instance.UndoTiles(1);
                  textUndoCount.text = currentUndoCount.ToString();
               }

               if (adsUndo.activeSelf)
               {
                  BoardTileCollector.Instance.UndoTiles(1);

               }

               if (coinUndo.activeSelf )
               {
                  StatusBar.Instance.currentCoin -= 100;
                  BoardTileCollector.Instance.UndoTiles(1);
                  if (StatusBar.Instance.currentCoin < 100)
                  {
                     coinUndo.SetActive(false);
                     adsUndo.SetActive(true);
                  }if (StatusBar.Instance.currentCoin < 200)
                  {
                     coinShuffle.SetActive(false);
                     adsShuffle.SetActive(true);
                  }if (StatusBar.Instance.currentCoin < 300)
                  {
                     coinMagicWand.SetActive(false);
                     adsMagicWand.SetActive(true);
                  }
                  
               }


               if (currentUndoCount < 1)
               {
                  valueUndo.SetActive(false);
                  if (StatusBar.Instance.currentCoin >= 100)
                  {
                     coinUndo.SetActive(true);
                     textCoinUndo.text = 100.ToString();
                  }
               }

               break;
            }
         }

         if(BoardTileCollector.Instance.collectedTiles.Count == 0)
         {
          
            undoGroup.alpha = 0.3f;
            if (currentUndoCount < 1)
            {
               adsUndo.SetActive(false);
            }
            
         }
      }
      else
      {
         ShowPopupRequirement(2, NotificationType.Level);
      }
      AudioManager.Instance.PlaySfx("Button_HighPitch_Default");

   }

   private void MagicWand()
   {
      if (TileManager.Instance.currentLevel >= 3)
      {
         if (currentMagicWandCount >= 1)
         {
            currentMagicWandCount--;
             TileManager.Instance.CollectSameFruitTiles();
             textMagicWandCount.text = currentMagicWandCount.ToString();
         }

         if (adsMagicWand.activeSelf)
         {
            TileManager.Instance.CollectSameFruitTiles();
         }

         if (coinMagicWand.activeSelf)
         {
            StatusBar.Instance.currentCoin -= 300;
            TileManager.Instance.CollectSameFruitTiles();
            if (StatusBar.Instance.currentCoin < 100)
            {
               coinUndo.SetActive(false);
               adsUndo.SetActive(true);
            }if (StatusBar.Instance.currentCoin < 200)
            {
               coinShuffle.SetActive(false);
               adsShuffle.SetActive(true);
            }if (StatusBar.Instance.currentCoin < 300)
            {
               coinMagicWand.SetActive(false);
               adsMagicWand.SetActive(true);
            }
         }
         if(currentMagicWandCount < 1)
         {
            valueMagicWand.SetActive(false);
            if (StatusBar.Instance.currentCoin >= 300)
            {
               coinMagicWand.SetActive(true);
               textCoinMagicWand.text = 300.ToString();
            }

            if (StatusBar.Instance.currentCoin < 300)
            {
               adsMagicWand.SetActive(true);
               coinMagicWand.SetActive(false);
            }
         }
      }
      else
      {
         ShowPopupRequirement(4, NotificationType.Level );
      }
      AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
   }

   private void OnShuffle()
   {
      if (TileManager.Instance.currentLevel >= 3)
      {
         if (currentShuffleCount >= 1)
         {
            currentShuffleCount--;
            TileManager.Instance.ShuffleTiles();
            textShuffleCount.text = currentShuffleCount.ToString();
         }

         if (adsShuffle.activeSelf)
         {
            TileManager.Instance.ShuffleTiles();
         }

         if (coinShuffle.activeSelf)
         {
            StatusBar.Instance.currentCoin -= 200;
            textCoinShuffle.text = 200.ToString();
            TileManager.Instance.ShuffleTiles();
            if (StatusBar.Instance.currentCoin < 100)
            {
               coinUndo.SetActive(false);
               adsUndo.SetActive(true);
            }if (StatusBar.Instance.currentCoin < 200)
            {
               coinShuffle.SetActive(false);
               adsShuffle.SetActive(true);
            }if (StatusBar.Instance.currentCoin < 300)
            {
               coinMagicWand.SetActive(false);
               adsMagicWand.SetActive(true);
            }

         }
         if(currentShuffleCount < 1)
         {
            valueShuffle.SetActive(false);
            if (StatusBar.Instance.currentCoin >= 200)
            {
               coinShuffle.SetActive(true);
               textCoinShuffle.text = 200.ToString();
            }

            if (StatusBar.Instance.currentCoin < 200)
            {
               adsShuffle.SetActive(true);
               coinShuffle.SetActive(false);
            }
         }
      }
      else
      {
         ShowPopupRequirement(3, NotificationType.Level);
         
      }

      AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
   }

   
   private void ShowPopupRequirement(int level, NotificationType notificationType)
   {
      _moveTween?.Kill();
      _fadeTween1?.Kill();
      _fadeTween2?.Kill();
      _scaleTween1?.Kill();
      _scaleTween2?.Kill();

      popupNotification.anchoredPosition = new Vector2(0, 0);
      var color = popupNotification.GetComponent<Image>().color;
      color.a = 1f;
      popupNotification.GetComponent<Image>().color = color;
      Color colorText = content.color;
      colorText.a = 1f;               
      content.color = colorText;
      popupNotification.transform.localScale = Vector3.one;

      popupNotification.gameObject.SetActive(true);

      _scaleTween1 = popupNotification.transform.DOScale(popupNotification.transform.localScale + new Vector3(0.2f, 0.2f), 0.2f).OnComplete(() =>
      {
         _scaleTween2 = popupNotification.transform.DOScale(1f, 0.2f).OnComplete(() =>
         {
            DOVirtual.DelayedCall(1f, () =>
            {
               _fadeTween1 = popupNotification.GetComponent<Image>().DOFade(0f, 0.2f);
               _fadeTween2 = content.DOFade(0f, 0.2f);

               _moveTween = popupNotification.DOAnchorPosY(400f, 0.4f).OnComplete(() =>
               {
                  popupNotification.gameObject.SetActive(false);
               });
            });
         });
      });

      switch (notificationType)
      {
         case NotificationType.Level:
            content.text = "New Prop will be unlocked at Level " + level;
            break;
         case NotificationType.TileMissing:
            content.text = "Oops, no tiles to undo ";
            break;
      }
   }

   private void SetAlphaImage(Color color, Image changeAlpha)
   {
      color.a = 0.3f;
      changeAlpha.color = color;
   }
   public void UnlockFeature(int nextLevel)
   {
      if (nextLevel >= levelUnlockUndo)
      {
         lockedUndo.SetActive(false);
         undoGroup.alpha = 0.3f;
         textUndoCount.text = currentUndoCount.ToString();

      }
      if (nextLevel >= levelUnlockShuffle)
      {
         lockedShuffle.SetActive(false);
         shuffleGroup.alpha = 1;
         textShuffleCount.text = currentShuffleCount.ToString();

      }
      if (nextLevel >= levelUnlockMagicWand)
      {
         lockedMagicWand.SetActive(false);
         magicWandGroup.alpha = 1;
         textMagicWandCount.text = currentMagicWandCount.ToString();

      }
   }

   public void SetFeatureValues(int undoCount, int magicWandCount, int shuffleCount)
   {
      if (StatusBar.Instance.currentCoin >= 100 && currentUndoCount < 1)
      {
         coinUndo.SetActive(true);
         textCoinUndo.text = 100.ToString();

      }
      if (StatusBar.Instance.currentCoin >= 300 && currentMagicWandCount < 1 )
      {
          coinMagicWand.SetActive(true);
          textCoinMagicWand.text = 300.ToString();
      }
        
      if (StatusBar.Instance.currentCoin >= 200 && currentShuffleCount < 1)
      {
         coinShuffle.SetActive(true);
         textCoinShuffle.text = 200.ToString();
      }

      if (undoCount >= 1)
      {
         valueUndo.SetActive(true);
         coinUndo.SetActive(false);
      }

      if (magicWandCount >= 1)
      {
         coinMagicWand.SetActive(false);
         valueMagicWand.SetActive(true);

      }

      if (shuffleCount >= 1)
      {
         valueShuffle.SetActive(true);
         coinShuffle.SetActive(false);
      }
      
      adsUndo.SetActive(false);
      adsMagicWand.SetActive(false);
      adsShuffle.SetActive(false);
      
      currentUndoCount += undoCount;
      currentMagicWandCount += magicWandCount;
      currentShuffleCount += shuffleCount;
      
      textUndoCount.text = currentUndoCount.ToString();
      textMagicWandCount.text = currentMagicWandCount.ToString();
      textShuffleCount.text = currentShuffleCount.ToString();
   }

  
}
