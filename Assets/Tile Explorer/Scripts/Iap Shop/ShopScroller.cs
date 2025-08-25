using System;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
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
    private List<IDataBase> _allItems = new List<IDataBase>();
    

    void Start()
    {
        InitializeShop();
        UpdatePurchased();
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

        _allItems = shopDataConfig.GetAllItems();
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

        IDataBase item = _allItems[dataIndex];
        
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

        IDataBase item = _allItems[dataIndex];
        EnhancedScrollerCellView cellView = null;
        if (!_prefabDict.TryGetValue(item.itemType, out var prefab))
        {
            return null;
        } 
        cellView = scroller.GetCellView(prefab);

        if (cellView is IShopCellView dataHandler)
        {
            dataHandler.SetData(item, dataIndex);
            dataHandler.onClick = null;
            dataHandler.onClick += OnClick;
        }

        return cellView;
        
    }

    private void OnClick(IShopCellView cellView)
    {
        switch (cellView)
        {
            case BundleCellView bundleCellView:
                bundleCellView.OnPurchase(bundleCellView.data);
                break;
            case CoinPackCellView coinPackCellView:
                Debug.Log(coinPackCellView.data);
                coinPackCellView.OnPurchase(coinPackCellView.data);
                break;
            case RemoveAdsCellView removeAdsCellView:
                Debug.Log(removeAdsCellView.data);
                removeAdsCellView.OnPurchase(removeAdsCellView.data);
                break;
        
        }
     
        UpdatePurchased();
    }

    private void UpdatePurchased()
    {
        
        var notPurchase = _allItems.Where(item => !(item.IsPurchase)).ToList();
        var purchased = _allItems.Where(item => item.IsPurchase).ToList();
        _allItems = notPurchase.Concat(purchased).ToList();
        scroller.ReloadData();
        
    }
}