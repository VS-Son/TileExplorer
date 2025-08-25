using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnhancedUI.EnhancedScroller;
using UnityEngine.Serialization;

[Serializable]
public class BundleItemData:IDataBase
{
    public string title;
    public string id;
    public int price;
    public bool isPurchase;
    public int saleOff;
    public Sprite icon;
    public int coin;
    public int undo;
    public int magicWand;
    public int shuffle;
    
    public string Id => id;
    public bool IsPurchase => isPurchase;
    public ShopTypeItem itemType => ShopTypeItem.Bundle;
}

[Serializable]
public class RemoveAdsData:IDataBase
{
    public string id;
    public string title;  
    public int price;
    public bool isPurchase;

    public string Id => id;
    public bool IsPurchase => isPurchase;
    public ShopTypeItem itemType => ShopTypeItem.RemoveAds;
   
}

[Serializable]
public class CoinPackData:IDataBase
{
    public string id;
    public int coin;
    public Sprite icon;
    public int price;
    public bool isPurchase;

    public string Id => id;
    public bool IsPurchase => isPurchase;    public ShopTypeItem itemType => ShopTypeItem.Coin;
    
}

[Serializable]
[CreateAssetMenu(fileName = "ShopDataConfig", menuName = "Iap shop")]
public class ShopItemData : ScriptableObject
{
   public List<BundleItemData> bundleData;
   public List<RemoveAdsData> removeAdsData;
   public List<CoinPackData> coinPackData;
   
   public List<IDataBase> GetAllItems()
   {
       List<IDataBase> allItems = new List<IDataBase>();
       allItems.AddRange(bundleData);
       allItems.AddRange(removeAdsData);
       allItems.AddRange(coinPackData);
       return allItems;
   }
}

