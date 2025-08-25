using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnhancedUI.EnhancedScroller;
using UnityEngine.Serialization;

public enum ShopTypeItem
{
    Bundle,
    RemoveAds,
    Coin , 
}
[Serializable]
public class BundleItemData:DataBase
{
    public string title;
    public int id;
    public int price;
    public int saleOff; 
    public int coin;
    public int undo;
    public int magicWand;
    public int shuffle;
    
    public int itemId => id;
    public ShopTypeItem itemType => ShopTypeItem.Bundle;
}

[Serializable]
public class RemoveAdsData:DataBase
{
    public int id;
    public string title;  
    public int price;

    public int itemId => id;
    public ShopTypeItem itemType => ShopTypeItem.RemoveAds;
   
}

[Serializable]
public class CoinPackData:DataBase
{
    public int id;
    public int coin;
    public Sprite icon;
    public int price;

    public int itemId => id;
    public ShopTypeItem itemType => ShopTypeItem.Coin;
    
}

[Serializable]
[CreateAssetMenu(fileName = "ShopDataConfig", menuName = "Iap shop")]
public class ShopItemData : ScriptableObject
{
   public List<BundleItemData> bundleData;
   public List<RemoveAdsData> removeAdsData;
   public List<CoinPackData> coinPackData;
   
   public List<DataBase> GetAllItems()
   {
       List<DataBase> allItems = new List<DataBase>();
       allItems.AddRange(bundleData);
       allItems.AddRange(removeAdsData);
       allItems.AddRange(coinPackData);
       return allItems;
   }
}

