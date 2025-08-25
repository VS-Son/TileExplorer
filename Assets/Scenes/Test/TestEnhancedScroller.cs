using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;

public class TestEnhancedScroller : MonoBehaviour, IEnhancedScrollerDelegate
{
    [System.Serializable]
    public class CellPrefabConfig
    {
        public CellType type;
        public EnhancedScrollerCellView prefab;
    }

    public EnhancedScroller scroller;
    public List<CellPrefabConfig> cellConfigs = new List<CellPrefabConfig>();

    private List<ItemData> _data;
    private Dictionary<CellType, EnhancedScrollerCellView> _prefabDict = new Dictionary<CellType, EnhancedScrollerCellView>();

    private void Start()
    {
        var types = System.Enum.GetValues(typeof(CellType));
        int totalTypes = types.Length;
        // Khởi tạo dictionary
        foreach (var config in cellConfigs)
        {
            if (config.prefab != null && !_prefabDict.ContainsKey(config.type))
            {
                _prefabDict[config.type] = config.prefab;
            }
        }

        _data = new List<ItemData>();
        for (int i = 0; i < 100; i++)
        {
            var type = (CellType)types.GetValue(i % totalTypes);

            _data.Add(new ItemData
            {
                text = "Item " + i,
                 type = type
            });
        }

        scroller.Delegate = this;
        scroller.ReloadData();
    }

    public int GetNumberOfCells(EnhancedScroller scroller) => _data.Count;

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return _data[dataIndex].type == CellType.B ? 120f : 100f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var item = _data[dataIndex];
        
        if (!_prefabDict.TryGetValue(item.type, out var prefab))
        {
            Debug.LogError($"No prefab found for type: {item.type}");
            return CreateFallbackCell();
        }

        // ENHANCEDSCROLLER TỰ ĐỘNG SỬ DỤNG CELL IDENTIFIER
        // Identifier được lấy từ prefab đã setup trong Inspector
        var cellView = scroller.GetCellView(prefab);
        
        if (cellView is ICellDataHandler dataHandler)
        {
            dataHandler.SetData(item.text, dataIndex);
        }

        return cellView;
    }

    private EnhancedScrollerCellView CreateFallbackCell()
    {
        var go = new GameObject("FallbackCell");
        return go.AddComponent<EnhancedScrollerCellView>();
    }
}
public interface ICellDataHandler
{
    void SetData(string text, int index);
}

public class ItemData
{
    public string text;
    public CellType type;
}

public enum CellType
{
    A,
    B,
    C,
}