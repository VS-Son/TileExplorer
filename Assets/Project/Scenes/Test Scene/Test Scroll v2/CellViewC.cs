using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CellViewC : EnhancedScrollerCellView,ICellDataHandler
{
    public TMP_Text label;
    //public string cellIdentifier = "CellTypeB"; // Có thể set tự động

    private int _dataIndex;
    private void Awake()
    {
        // Tự động set identifier nếu cần
       // if (string.IsNullOrEmpty(cellIdentifier))
            //cellIdentifier = "CellTypeB";
    }

    public void SetData(string data, int dataIndex)
    {
        _dataIndex = dataIndex;
        label.text = data;
    }
}
