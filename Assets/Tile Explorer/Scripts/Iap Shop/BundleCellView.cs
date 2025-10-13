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
    public Image iconCoin;
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
            iconCoin.sprite = bundleItemData.icon;
             itemNameText.text =(bundleItemData.title) + " Bundle";
             priceText.text = !bundleItemData.isPurchase ? "₫" + bundleItemData.price.ToString("F") : "Purchased";
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
            if (!isPurchased)
            {
                isPurchased = true;
                UIManager.Instance.GetUI<ShopScreen>().SetStatusValues(bundleData.coin, bundleData.undo, bundleData.magicWand, bundleData.shuffle);
                UIManager.Instance.GetUI<PlayScreen>().SetBoosterValues(bundleData.undo, bundleData.magicWand, bundleData.shuffle);
                bundleData.isPurchase = isPurchased;
            }
            else
            {
                
                //priceText.text = "Purchased";
                
            }
        }
    }
}