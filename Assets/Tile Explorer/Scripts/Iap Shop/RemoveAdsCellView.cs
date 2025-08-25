using EnhancedUI.EnhancedScroller;
using TMPro;
using System;
using UnityEngine;

public class RemoveAdsCellView : EnhancedScrollerCellView,IShopCellView
{public string id;
    public bool isPurchased;
    public TMP_Text priceText;
    public TMP_Text titleText;
    public Action<IShopCellView> onClick { get; set; }
    public RemoveAdsData data;


    public void SetData(IDataBase data, int index)
    {
        if (data is RemoveAdsData removeAdsData)
        {
            this.data = removeAdsData;
            id = removeAdsData.id;
            isPurchased = removeAdsData.isPurchase;
             priceText.text = "đ" + removeAdsData.price.ToString("F");
             titleText.text = removeAdsData.title ?? "Remove Ads";
             
        }
    }
    public void OnClick()
    {
        onClick.Invoke(this);
    }

    public void OnPurchase(IDataBase data)
    {
        if (data is RemoveAdsData removeAdsData)
        {
            if (!id.Equals(removeAdsData.id)) return;
            Debug.Log("dung id " + id);
            if (!isPurchased)
            {
                Debug.Log("chua mua");
                isPurchased = true;
                removeAdsData.isPurchase = isPurchased;
            }
            else
            {
                Debug.Log("da mua");

            }
        }
    }
}