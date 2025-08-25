
using System;
using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;

public class TestItemView: EnhancedScrollerCellView
{
    [SerializeField] private TextMeshProUGUI title;
    public Action<TestItemView> onClick;
    public TestItemData itemData;
    public void SetData(TestItemData data)
    {
        if (data is TestItemData itemData)
        {
            this.itemData = itemData;
            title.SetText(itemData.title);
        }
    }

    public void OnClick()
    {
        onClick?.Invoke(this);
    }
}
