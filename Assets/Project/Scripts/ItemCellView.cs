using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class ItemCellView : EnhancedScrollerCellView
    {
        [Header("Common References")]
        public TMP_Text nameText;
        public TMP_Text priceText;
        public GameObject purchasedIndicator;
    
        [Header("Bundle References")]
        public GameObject bundleSection;
        public Button purchase;
        public TMP_Text coinsText;
        public TMP_Text item1Text;
        public TMP_Text item2Text;
        public TMP_Text item3Text;
    
        [Header("Remove Ads References")]
        public GameObject removeAdsSection;
        public TMP_Text bonusCoinsText;
    
        [Header("Coin Pack References")]
        public GameObject coinPackSection;
        public TMP_Text coinPackAmountText;
    
        private IDataBase _current;
    
        public System.Action<IDataBase> onPurchase;
    
        public void SetData(IDataBase data)
        {
            _current = data;
        
            bundleSection.SetActive(false);
            removeAdsSection.SetActive(false);
            coinPackSection.SetActive(false);
        
            switch (data.itemType)
            {
                case ShopTypeItem.Bundle:
                    SetupBundleData((BundleItemData)data);
                    break;
                case ShopTypeItem.RemoveAds:
                    SetupRemoveAdsData((RemoveAdsData)data);
                    break;
                case ShopTypeItem.Coin:
                    SetupCoinPackData((CoinPackData)data);
                    break;
            }
        
            UpdatePurchaseStatus();
        }
    
        private void SetupBundleData(BundleItemData data)
        {
            bundleSection.SetActive(true);
            coinsText.text = "x" + data.coin;
        }
    
        private void SetupRemoveAdsData(RemoveAdsData data)
        {
            removeAdsSection.SetActive(true);
        }
    
        private void SetupCoinPackData(CoinPackData data)
        {
            coinPackSection.SetActive(true);
        }
    
        private void UpdatePurchaseStatus()
        {
            // if (_current.IsPurchased)
            // {
            //     purchaseButton.gameObject.SetActive(false);
            //     purchasedIndicator.SetActive(true);
            // }
            // else
            // {
            //     purchaseButton.gameObject.SetActive(true);
            //     purchasedIndicator.SetActive(false);
            // }
        }
    
  
    
        public void ResetCell()
        {
            _current = null;
            nameText.text = "";
            priceText.text = "";
            bundleSection.SetActive(false);
            removeAdsSection.SetActive(false);
            coinPackSection.SetActive(false);
            purchasedIndicator.SetActive(false);
        }
    }
}
