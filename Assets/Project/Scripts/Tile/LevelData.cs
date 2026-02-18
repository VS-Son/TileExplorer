using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Tile
{
    [System.Serializable]
    public class LevelTileRoot
    {
        public string name;
        public List<LevelData> LevelTile;
    }
    [Serializable]
    public class LevelData
    {
        public int level;
        public float size;
        public float spacing;

        public List<TileLayerData> tileLayer;
    }
    [Serializable]
    public class TileLayerData
    {
        public int layerSort;
        public float posY;
        public float posX;
        public int rows;
        public int cols;
        public string tileNameLayer;
        public List<Vector2Int> removeTile;



    }
    [Serializable]
    public class RemoveTileIndex
    {
        public int posY;
        public int posX;
    }

    public enum TypeId
    {
        Id0,
        Id1,
        Id2,
        Id3,
        Id4,
        Id5,
        Id6,
        Id7,
        Id8,
        Id9,
        None
    }

    public enum NotificationType
    {
        Level,
        TileMissing
    }

    [Serializable]
    public class TileSpriteData
    {
        public TypeId typeId;
        public Sprite sprite;
    }
}