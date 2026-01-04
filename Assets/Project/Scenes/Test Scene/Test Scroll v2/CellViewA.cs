using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;

public class CellViewA : EnhancedScrollerCellView,ICellDataHandler
{
    public TMP_Text label;
    //public string cellIdentifier = "CellTypeA"; // Có thể set tự động

    private int _dataIndex;
    private void Awake()
    {
        // // Tự động set identifier nếu cần
        // if (string.IsNullOrEmpty(cellIdentifier))
        //     cellIdentifier = "CellTypeA";
    }

    // Gán dữ liệu cho cell
    public void SetData(string data, int dataIndex)
    {
        _dataIndex = dataIndex;
        label.text = data;
    }
}
