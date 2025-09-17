using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
   public static ShopManager Instance;
   public Button back;

   [Header("Play Screen")]
   [SerializeField] private PlayScreen playScreen;
   [Header("Status Bar")]
   public TMP_Text valueCoin;
   public TMP_Text valueUndo;
   public TMP_Text valueMagicWand;
   public TMP_Text valueShuffle;
   [Header("Refund")] 
   [SerializeField] private Button refund;
   private int _coinCount;
   private int _undoCount;
   private int _magicWandCount;
   private int _shuffleCount;
   private void Awake()
   {
      Instance = this;
   }
   private void Start()
   {
      back.onClick.AddListener(OnBack);
      refund.onClick.AddListener(Refund);
   }

   private void Refund()
   {
      var shopScroll = FindObjectOfType<ShopScroller>();
      if (shopScroll != null)
      {
         shopScroll.RefundPurchased();
      }
   }
   private void OnBack()
   {
      gameObject.SetActive(false);
      
   }

   public void SetStatusValues(int coin, int undo , int magicWand , int shuffle)
   {
      StatusBar.Instance.UpdateCoin(coin);
      valueCoin.text = StatusBar.Instance.FormatCoin(StatusBar.Instance.currentCoin);

      _undoCount += undo;
      _magicWandCount += magicWand;
      _shuffleCount += shuffle;

      valueUndo.text =_undoCount.ToString("");
      valueMagicWand.text =_magicWandCount.ToString("");
      valueShuffle.text = _shuffleCount.ToString("");
   }
  
}
