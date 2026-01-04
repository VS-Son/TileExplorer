using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using Tile_Explorer.Scripts.Tile;
using UnityEngine;
using UnityEngine.UI;

public class TileThemeScroller : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroller;
    [SerializeField] private EnhancedScrollerCellView cellViewPrefab;
    public ListTileTheme listTileTheme;
    [SerializeField] private Button close;
    [SerializeField] private Button confirm;
    private TileThemeCellView _currentSelect;
    private TileThemeCellView _confirmedSelect;
    private int _tempSelectedId = -1;       // id đã confirm trước đó

    private void Start()
    {
        close.onClick.AddListener(OnClose);
        confirm.onClick.AddListener(OnConfirm);
        scroller.Delegate = this;
        scroller.ReloadData();
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return listTileTheme.tileThemeData.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 300f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var tileData = listTileTheme.tileThemeData[dataIndex];
        var tileView = scroller.GetCellView(cellViewPrefab);
        if (tileView is TileThemeCellView tile)
        {
            tile.SetData(tileData,  _tempSelectedId);
            tile.onSelected = OnTileThemeSelect;
        }

        return tileView;
    }

    private void OnTileThemeSelect(TileThemeCellView tile)
    {
        _tempSelectedId = tile.id;
        scroller.ReloadData();
    }

    private void OnClose()
    {
        _tempSelectedId = -1;
        scroller.ReloadData();

        gameObject.SetActive(false);
        EditTileset.Instance.SetAlphaPanel(0.6f);
    }

    private void OnConfirm()
    {
        if (_tempSelectedId != -1)
        {
            foreach (var t in listTileTheme.tileThemeData)
                t.isSelected = (t.id == _tempSelectedId);

            // gọi TileManager đổi theme
            TileThemeData confirmedTheme = listTileTheme.tileThemeData.Find(t => t.isSelected);
            TileManager.Instance.ApplyTheme(confirmedTheme);
            EditTileset.Instance.SetEditTiles(confirmedTheme.spriteTile);

            _tempSelectedId = -1;
            scroller.ReloadData();
        }
        
        gameObject.SetActive(false);
        EditTileset.Instance.SetAlphaPanel(0.6f);
        
    }
    
    public void SetOutlineSelect()
    {
        _tempSelectedId = -1;
        scroller.ReloadData();
        
    }
}
