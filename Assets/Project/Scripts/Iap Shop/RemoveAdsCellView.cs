using EnhancedUI.EnhancedScroller;
using TMPro;
using System;
using Project.Scripts.UI.Manager;
using Project.Scripts.UI.Screen;
using UnityEngine;

public class RemoveAdsCellView : EnhancedScrollerCellView,IShopCellView
{public string id;
    public bool isPurchased;
    public TMP_Text priceText;
    public TMP_Text titleText;
    public TMP_Text quantityCoinText;
    public Action<IShopCellView> onClick { get; set; }
    public RemoveAdsData data;


    public void SetData(IDataBase data, int index)
    {
        if (data is RemoveAdsData removeAdsData)
        {
            this.data = removeAdsData;
            quantityCoinText.text = "+ " + removeAdsData.coin;
            id = removeAdsData.id;
            isPurchased = removeAdsData.isPurchase;
            priceText.text = !removeAdsData.isPurchase ? "₫" + removeAdsData.price.ToString("F") : "Purchased";
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
            Debug.Log("id " + id);
            if (!isPurchased)
            {
                Debug.Log("not purchased");
                isPurchased = true;
                UIManager.Instance.GetUI<ShopScreen>().SetStatusValues(removeAdsData.coin, 0,0,0);
                removeAdsData.isPurchase = isPurchased;
            }
            else
            {
                Debug.Log("has purchased");

            }
        }
    }
}