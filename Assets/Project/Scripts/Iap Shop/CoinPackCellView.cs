using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Project.Scripts.UI.Manager;
using Project.Scripts.UI.Screen;


public class CoinPackCellView : EnhancedScrollerCellView,IShopCellView
{
    public string id;
    public Image iconCoin;
    public TMP_Text quantityCoinText;
    public TMP_Text priceText;
    //public Action<CoinPackCellView> onClick;
    public CoinPackData data;

    public Action<IShopCellView> onClick { get; set; }

    public void SetData(IDataBase data, int index)
    {
        if (data is CoinPackData coinPackData)
        {
            id = coinPackData.id;
            this.data = coinPackData;
            iconCoin.sprite = coinPackData.icon;
            quantityCoinText.text = "x" + coinPackData.coin; 
            priceText.text = "đ" + coinPackData.price.ToString("F");

        }
      
    }
    public void OnClick()
    {
        onClick.Invoke(this);
    }

    public void OnPurchase(IDataBase data)
    {
        if (data is CoinPackData coinData)
        {
            if (!id.Equals(coinData.id)) return;
            UIManager.Instance.GetUI<ShopScreen>().SetStatusValues(coinData.coin, 0, 0 ,0);
            UIManager.Instance.GetUI<PlayScreen>().SetBoosterValues(0,0,0);
        }
    }
}