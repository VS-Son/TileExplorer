using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[Serializable]
public class TileThemeData
{
    public Sprite[] spritesTile;
}
[CreateAssetMenu(fileName = "ThemeData", menuName = "ListThemeData")]
public class ListTileTheme: ScriptableObject
{
    public List<TileThemeData> tileThemeData;
}
public class TileCollection : EnhancedScrollerCellView
{
    public List<Image> imagesTile;
    private TileThemeData _data;

    public void SetData(TileThemeData data)
    {
        _data = data;
        foreach (var index in data.spritesTile)
        {
        }
    }
}
