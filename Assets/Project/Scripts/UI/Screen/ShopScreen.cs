using Project.Scripts.Game;
using Project.Scripts.Iap_Shop;
using Project.Scripts.UI.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Screen
{
   public class ShopScreen : UICanvas
   {

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
      private GameState _gameState;

  
      public void OnRefund()
      {
         var shopScroll = FindObjectOfType<ShopScroller>();
         if (shopScroll != null)
         {
            shopScroll.RefundPurchased();
         }
      }
      public void OnBack()
      {
         UIManager.Instance.CloseUI<ShopScreen>();
      }
   
      public void SetStatusValues(int coin, int undo , int magicWand , int shuffle)
      {
         UIManager.Instance.GetUI<StatusBar>().UpdateCoin(coin);
         valueCoin.text =  UIManager.Instance.GetUI<StatusBar>().FormatCoin( UIManager.Instance.GetUI<StatusBar>().currentCoin);

         _undoCount += undo;
         _magicWandCount += magicWand;
         _shuffleCount += shuffle;

         valueUndo.text =_undoCount.ToString("");
         valueMagicWand.text =_magicWandCount.ToString("");
         valueShuffle.text = _shuffleCount.ToString("");
      }

      public void FormatCoin(int coin)
      {
         valueCoin.text = UIManager.Instance.GetUI<StatusBar>().FormatCoin(coin);
      }
   }
}
