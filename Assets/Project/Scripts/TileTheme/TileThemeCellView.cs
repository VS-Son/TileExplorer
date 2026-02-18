using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Project.Scripts.TileTheme
{
    public class TileThemeCellView : EnhancedScrollerCellView
    {
        public Action<TileThemeCellView> onSelected; 
        public int id;
        public List<Image> imagesTile;
        public Button select;
        public Image outline;
        public TileThemeData data;
        public bool isSelected;


        private void Start()
        {
            select.onClick.AddListener(OnSelect);
        }

        private void OnSelect()
        {
            Debug.Log("select tile theme");
            Debug.Log(data.id);
            onSelected?.Invoke(this);
        }

   

        public void SetData(TileThemeData data, int  tempSelectedId)
        {
            this.data = data;
            for (int i = 0; i < imagesTile.Count; i++)
            {
                imagesTile[i].sprite = data.spriteTile[i];
            }

            id = data.id;
            bool outlineOn = (tempSelectedId == -1) ? data.isSelected : (data.id == tempSelectedId);
            outline.enabled = outlineOn;    }
    
        public void SetSelect(bool selected)
        {
            outline.enabled = selected;
        }

        public TileThemeData GetData()
        {
            return data;
        }
    }
}