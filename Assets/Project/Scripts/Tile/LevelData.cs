using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

public enum TypeTileId
{
    ID1,
    ID2,
    ID3,
    ID4,
    ID5,
    ID6,
    ID7,
    ID8,
    ID9,
    ID10,
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
    public TypeTileId typeTileId;
    public Sprite sprite;
}
