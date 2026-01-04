using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class TestList : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroller;
    [SerializeField] private float itemSize = 150f;
    [SerializeField] private TestItemView itemPrefab;

    private List<TestItemData> listItemData = new List<TestItemData>();

    private void Start()
    {
        scroller.Delegate = this;
        for (int i = 0; i < 100; i++)
        {
            listItemData.Add(new TestItemData { title = $"item # {i}"});
        }
        scroller.ReloadData();
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return listItemData.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return itemSize;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var itemData = listItemData[dataIndex];
        var item = scroller.GetCellView(itemPrefab);
        if (item is TestItemView itemBaseView)
        {
            itemBaseView.SetData(itemData);
            itemBaseView.onClick = OnItemClick;
        }

        return item;
    }

    private void OnItemClick(TestItemView item)
    {
        Debug.LogWarning($"Click on Item :{item.itemData.title}");
    }
}
