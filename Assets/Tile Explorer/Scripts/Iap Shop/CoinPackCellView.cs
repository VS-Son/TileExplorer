using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinPackCellView : EnhancedScrollerCellView,IShopCellView
{
    public Image iconCoin;
    public TMP_Text quantityCoinText;
    public TMP_Text priceText;
    
    public void SetData(DataBase data, int index)
    {
        if (data is CoinPackData coinPackData)
        {
            iconCoin.sprite = coinPackData.icon;
            quantityCoinText.text = "x" + coinPackData.coin; 
            priceText.text = "đ" + coinPackData.price.ToString("N0");
            
        }
      
    }
}