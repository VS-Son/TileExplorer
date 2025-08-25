using System;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class ShopScroller : MonoBehaviour, IEnhancedScrollerDelegate
{
    [Header("Scroller References")]
    public EnhancedScroller scroller;
    [Header("Item Prefabs")]
    public ListItemPrefabConfig cellConfigs;
    [FormerlySerializedAs("shopDatabase")] [Header("Shop Data")]
    public ShopItemData shopDataConfig;
    private Dictionary<ShopTypeItem, EnhancedScrollerCellView> _prefabDict = new Dictionary<ShopTypeItem, EnhancedScrollerCellView>();
    private List<DataBase> _allItems = new List<DataBase>();

    void Start()
    {
        InitializeShop();
    }

    private void InitializeShop()
    {
        scroller.Delegate = this;
        foreach (var config in cellConfigs.cellPrefab)
        {
            if (config.prefab != null && !_prefabDict.ContainsKey(config.type))
            {
                _prefabDict[config.type] = config.prefab;
            }
        }
        _allItems = new List<DataBase>();
        _allItems.AddRange(shopDataConfig.bundleData);
        _allItems.AddRange(shopDataConfig.removeAdsData);
        _allItems.AddRange(shopDataConfig.coinPackData);
        scroller.ReloadData();
    }
    

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _allItems.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        if (dataIndex < 0 || dataIndex >= _allItems.Count)
            return 200f;

        DataBase item = _allItems[dataIndex];
        
        switch (item.itemType)
        {
            case ShopTypeItem.Bundle:
                return 414;
            case ShopTypeItem.RemoveAds:
                return 200;
            case ShopTypeItem.Coin:
                return 200f;
            default:
                return 200f;
        }
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        if (dataIndex < 0 || dataIndex >= _allItems.Count)
            return null;

        DataBase item = _allItems[dataIndex];
        EnhancedScrollerCellView cellView = null;
        if (!_prefabDict.TryGetValue(item.itemType, out var prefab))
        {
            return null;
        } 
        cellView = scroller.GetCellView(prefab);

        if (cellView is IShopCellView dataHandler)
        {
            dataHandler.SetData(item, dataIndex);
        }

        return cellView;
        
    }
    
}