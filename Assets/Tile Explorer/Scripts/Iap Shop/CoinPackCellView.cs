using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;


public class CoinPackCellView : EnhancedScrollerCellView,IShopCellView
{
    public string id;
    public bool isPurchased;
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
            isPurchased = coinPackData.isPurchase;
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
            Debug.Log("dung id " + id);
            if (!isPurchased)
            {
                Debug.Log("chua mua");
                isPurchased = true;
                coinData.isPurchase = isPurchased;
            }
            else
            {
                Debug.Log("da mua");

            }
        }
    }
}