using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

[System.Serializable]
public class CellPrefabConfig
{
    public ShopTypeItem type;
    public EnhancedScrollerCellView prefab;
}
[CreateAssetMenu(fileName = "cellConfigs", menuName = "ListCellPrefabConfig")]
public class ListItemPrefabConfig : ScriptableObject
{
    public List<CellPrefabConfig> cellPrefab = new List<CellPrefabConfig>();
}