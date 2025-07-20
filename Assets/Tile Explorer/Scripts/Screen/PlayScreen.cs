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

   [Header("Button")]
   [SerializeField] private Button home;

   [SerializeField] private Button setting;
   public Button undo;
   public Button magicWand;
   public Button shuffle;
   [Header("Icon")]
   public Image iconUndo;
   public Image iconMagicWand;
   public Image iconShuffle;
   [Header("Screen")]
   [SerializeField] private HomeScreen homeScreen;
   [SerializeField] private SettingsScreen settingsScreen;
   [Header("Text")]
   [SerializeField] private TMP_Text currentLevel;
   [SerializeField] private TMP_Text totalCoin;
   [SerializeField] private TMP_Text content;
   [Header("Object")]
   [SerializeField] private RectTransform statusBar;
   [SerializeField] private RectTransform popupNotification;
   [SerializeField] private GameObject lockedUndo, lockedMagicWand, lockedShuffle;
   
   private Tween _moveTween, _fadeTween1,_fadeTween2, _scaleTween1, _scaleTween2;

   private void Start()
   {
      home.onClick.AddListener((OnHome));
      undo.onClick.AddListener(OnUndo);
      magicWand.onClick.AddListener(MagicWand);
      shuffle.onClick.AddListener((OnShuffle));
      setting.onClick.AddListener(OnSetting);
   }

   private void OnSetting()
   {
      settingsScreen.gameObject.SetActive(true);
      AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
   }
   private void MagicWand()
   {
      if (TileManager.Instance.currentLevel >= 4)
      {
         TileManager.Instance.CollectSameFruitTiles();
         lockedMagicWand.SetActive(false);
      }
      else
      {
         ShowPopupRequirement(4, NotificationType.Level );
      }
      AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
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
               BoardTileCollector.Instance.RevertTiles(1);
               break;
         }
         if(BoardTileCollector.Instance.collectedTiles.Count == 0)
         {
            var color = undo.image.color;
            color.a = 0.3f;
            undo.image.color = color;
            var colorIcon = iconUndo.color;
            colorIcon.a = 0.3f;
            iconUndo.color = colorIcon;
            //ShowPopupRequirement(2);
         }
      }
      else
      {
         ShowPopupRequirement(2, NotificationType.Level);
      }
      AudioManager.Instance.PlaySfx("Button_HighPitch_Default");

   }

   private void OnShuffle()
   {
      //  Debug.Log("undo");
      AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
      if (TileManager.Instance.currentLevel >= 3)
      {
         TileManager.Instance.ShuffleTiles();
         lockedShuffle.SetActive(false); 
      }
      else
      {
         ShowPopupRequirement(3, NotificationType.Level);
         
      }
   }

   private void OnHome()
   {
      homeScreen.gameObject.SetActive(true);
      this.gameObject.SetActive(false);
      TileManager.Instance.gameObject.SetActive(false);
      BoardTileCollector.Instance.gameObject.SetActive(false);
      CurrentLevel?.Invoke(TileManager.Instance.currentLevel);
      statusBar.DOAnchorPos(new Vector2(0, 85),0.6f).SetEase(Ease.OutCubic);
      AudioManager.Instance.StopBgm();
      //AudioManager.Instance.PlayBgm("bgm");
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
   
   public void ShowStatusBar(int level)
   {
      gameObject.SetActive(true);
      statusBar.DOAnchorPos(new Vector2(0, -85),0.6f).SetEase(Ease.OutCubic);
      currentLevel.text = "Level " + level;
   }
}
