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

public enum FruitType
{
    Banana,
    Apple,
    Orange,
    Watermelon,
    Grapes,
    Cherry,
    Strawberry,
    Lemon,
    Pineapple,
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
    public FruitType fruitType;
    public Sprite sprite;
}
