using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.TileTheme
{
    [Serializable]
    public class TileThemeData
    {
        public int id;
        public TypeTileTheme typeTileTheme;
        public List<Sprite> spriteTile;
        public bool isSelected;
    }
    [CreateAssetMenu(fileName = "ThemeData", menuName = "ListThemeData")]
    public class ListTileTheme: ScriptableObject
    {
        public List<TileThemeData> tileThemeData;
    
    }
}