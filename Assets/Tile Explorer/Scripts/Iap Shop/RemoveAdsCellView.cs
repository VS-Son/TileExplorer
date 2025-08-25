using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;

public class RemoveAdsCellView : EnhancedScrollerCellView,IShopCellView
{
    public TMP_Text priceText;
    public TMP_Text titleText;

    
    public void SetData(DataBase data, int index)
    {
        if (data is RemoveAdsData removeAdsData)
        {
            if (priceText != null) priceText.text = "đ" + removeAdsData.price.ToString("N0");
            if (titleText != null) titleText.text = removeAdsData.title ?? "Remove Ads";        }
       
        
    }
}