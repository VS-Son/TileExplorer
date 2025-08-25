using System;
using EnhancedUI.EnhancedScroller;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BundleCellView : EnhancedScrollerCellView,IShopCellView
{
    public string id;
    public bool isPurchased;
    public Image coin;
    public TMP_Text itemNameText;
    public TMP_Text priceText;
    public TMP_Text saleOffText;
    public TMP_Text quantityCoinText;
    public TMP_Text quantityUndoText;
    public TMP_Text quantityMagicWandText;
    public TMP_Text quantityShuffleText;
    private BundleItemData _bundleItemData;
    public BundleItemData data;
    public Action<IShopCellView> onClick { get; set; }


    public void SetData(IDataBase data, int index)
    {
        if (data is BundleItemData bundleItemData)
        {
            id = bundleItemData.id;
            isPurchased = bundleItemData.isPurchase;
            this.data = bundleItemData;
            coin.sprite = bundleItemData.icon;
             itemNameText.text =(bundleItemData.title) + " Bundle";
             priceText.text = ("₫" + bundleItemData.price.ToString("F"));
             saleOffText.text =(bundleItemData.saleOff > 0 ? bundleItemData.saleOff + "% OFF" : "");
             quantityCoinText.text =("x" + bundleItemData.coin);
            quantityUndoText.text =("x" + bundleItemData.undo);
            quantityMagicWandText.text =("x" + bundleItemData.magicWand);
             quantityShuffleText.text =("x" + bundleItemData.shuffle);        }
    }

    public void OnClick()
    {
        onClick.Invoke(this);
    }

    public void OnPurchase(IDataBase data)
    {
        if (data is BundleItemData bundleData)
        {
            if (!id.Equals(bundleData.id)) return;
            Debug.Log("dung id " + id);
            if (!isPurchased)
            {
                Debug.Log("chua mua");
                isPurchased = true;
                bundleData.isPurchase = isPurchased;
            }
            else
            {
                Debug.Log("da mua");

            }
        }
    }
}