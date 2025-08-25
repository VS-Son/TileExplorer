using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;

public class BundleCellView : EnhancedScrollerCellView,IShopCellView
{
    public TMP_Text itemNameText;
    public TMP_Text priceText;
    public TMP_Text saleOffText;
    public TMP_Text quantityCoinText;
    public TMP_Text quantityUndoText;
    public TMP_Text quantityMagicWandText;
    public TMP_Text quantityShuffleText;
    private BundleItemData _bundleItemData;
    
    
    public void SetData(DataBase data, int index)
    {
        if (data is BundleItemData bundleItemData)
        {
             itemNameText.SetText(bundleItemData.title ?? "No Title");
             priceText.SetText("đ" + bundleItemData.price.ToString("F"));
             saleOffText.SetText(bundleItemData.saleOff > 0 ? bundleItemData.saleOff + "% OFF" : "");
             quantityCoinText.SetText("x" + bundleItemData.coin);
            quantityUndoText.SetText("x" + bundleItemData.undo);
            quantityMagicWandText.SetText("x" + bundleItemData.magicWand);
             quantityShuffleText.SetText("x" + bundleItemData.shuffle);        }
    }
}