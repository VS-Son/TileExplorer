using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using System.Linq;


public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    public int currentLevel;
    public TextAsset tileJson;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private List<TileSpriteData> spriteDataList;
    [SerializeField] private ListTileTheme listTileTheme;
    public Transform gamePlayTransform;

    private List<LevelData> m_TileLevel;
    private readonly Dictionary<TypeTileId, int> m_Count = new Dictionary<TypeTileId, int>();
    private Dictionary<TypeTileId, Sprite> m_SpriteLookup;
    public int tileIndex { get; set; }
    private float sizeToLoad { get; set; }
    private float spacingToLoad { get; set; }


    private List<TypeTileId> m_DistributedTiles;

    public readonly Dictionary<int, Tile[,]> m_LayerTiles = new();

    private readonly Dictionary<int, Transform> m_LayerParent = new();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        TileThemeData confirmedTheme = listTileTheme.tileThemeData.Find(t => t.isSelected);
        for (int i = 0; i < spriteDataList.Count; i++)
        {
            spriteDataList[i].sprite = confirmedTheme.spriteTile[i];
        }
    }

    private readonly List<TypeTileId> m_ListFruit = new List<TypeTileId>()
    {
        TypeTileId.ID1,
        TypeTileId.ID2,
        TypeTileId.ID3,
        TypeTileId.ID4,
        TypeTileId.ID5,
        TypeTileId.ID6,
        TypeTileId.ID7,
        TypeTileId.ID8,
        TypeTileId.ID9,
        TypeTileId.ID10,
    };

    void Start()
    {
        TileJson();
        GenerateTileManager();
        CheckBoosterUnlock();
    }

    private void CheckBoosterUnlock()
    {

    }

    private void TileJson()
    {
        LevelTileRoot json = JsonUtility.FromJson<LevelTileRoot>(tileJson.text);
        m_TileLevel = json.LevelTile;

    }

    private void GenerateTileManager()
    {
        m_SpriteLookup = new Dictionary<TypeTileId, Sprite>();
        foreach (var data in spriteDataList)
        {
            if (!m_SpriteLookup.ContainsKey(data.typeTileId))
            {
                m_SpriteLookup.Add(data.typeTileId, data.sprite);
            }
        }

        List<TileLayerData> layerConfigs = LoadLayerConfigs();

        int totalTileCount = 0;
        foreach (var config in layerConfigs)
        {
            int removeCount = config.removeTile != null ? config.removeTile.Count : 0;
            totalTileCount += (config.rows * config.cols) - removeCount;
        }

        CalculateIndexLayerTile(totalTileCount);

        LevelData levelData = m_TileLevel.Find(l => l.level == currentLevel);
        if (levelData != null)
        {
            sizeToLoad = levelData.size;
            spacingToLoad = levelData.spacing;
            foreach (var config in layerConfigs)
            {
                int maxLayer = 0;
                if (config.layerSort == layerConfigs.Count)
                {
                    maxLayer = config.layerSort;
                }

                Debug.Log("higher " + maxLayer);
                var tileConfig = GenerateTile(config.rows, config.cols, config.posY, config.posX, config.tileNameLayer,
                    config.layerSort, config.removeTile
                    , maxLayer);
                m_LayerTiles[config.layerSort] = tileConfig;
                Debug.Log("layer Grid " + m_LayerTiles.Count);

            }
        }


        Debug.Log("Total Tile: " + totalTileCount);
    }

    private List<TileLayerData> LoadLayerConfigs()
    {
        LevelData level = m_TileLevel.Find(l =>
        {
            if (l.level == currentLevel) return true;
            return false;
        });

        return level.tileLayer;
    }

    private void CalculateIndexLayerTile(int totalIndex)
    {
        m_DistributedTiles = GenerateDistributedTiles(totalIndex, m_ListFruit);
        ShuffleList(m_DistributedTiles);
        foreach (var fruitType in m_DistributedTiles)
        {
            if (!m_Count.ContainsKey(fruitType)) m_Count[fruitType] = 0;
            m_Count[fruitType]++;
        }

        // foreach (var kv in m_Count)
        // {
        //     Debug.Log($"{kv.Key}: {kv.Value}");
        // }
    }

    private List<TypeTileId> GenerateDistributedTiles(int totalTilesCreated, List<TypeTileId> list)
    {
        List<int> itemCounts = GenerateBalancedCountsTile(totalTilesCreated, list.Count);
        ShuffleList(itemCounts);
        List<TypeTileId> result = new List<TypeTileId>();
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < itemCounts[i]; j++)
            {
                result.Add(list[i]);
            }
        }

        return result;
    }

    private List<int> GenerateBalancedCountsTile(int totalTilesCreated, int listCount)
    {
        List<int> countTiles = new List<int>();
        int baseCount = (totalTilesCreated / listCount) / 3 * 3;
        for (int i = 0; i < listCount; i++)
        {
            countTiles.Add(baseCount);
        }

        int used = baseCount * listCount;
        int remaining = totalTilesCreated - used;
        int extra = remaining / 3;

        List<int> indices = new List<int>();
        for (int i = 0; i < listCount; i++) indices.Add(i);
        for (int i = 0; i < extra; i++)
        {
            countTiles[indices[i]] += 3;
        }

        return countTiles;
    }


    private Tile[,] GenerateTile(int rows, int cols, float y, float x, string nameParentLayer, int orderInLayer,
        List<Vector2Int> removeIndex, int maxLayer)
    {

        Tile[,] tiles = new Tile[cols, rows];
        float offset = (rows - 1) * spacingToLoad / 2f;
        float offsetY = (cols - 1) * spacingToLoad / 2f;

        if (!m_LayerParent.ContainsKey(orderInLayer))
        {
            GameObject layerParent = new GameObject(nameParentLayer);
            m_LayerParent[orderInLayer] = layerParent.transform;
            layerParent.transform.SetParent(gamePlayTransform);

        }

        for (int colY = 0; colY < tiles.GetLength(0); colY++)
        {
            for (int rowX = 0; rowX < tiles.GetLength(1); rowX++)
            {
                Vector3 spawnPosition = new Vector3((rowX * spacingToLoad - offset) + x,
                    -colY * spacingToLoad - offsetY + y, 0);
                if (removeIndex != null && removeIndex.Contains(new Vector2Int(rowX, colY))) 
                {
                    continue;
                }

                var tile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                tile.transform.SetParent(m_LayerParent[orderInLayer]);

                tile.spriteFruit.sortingOrder = orderInLayer;
                tile.background.sortingOrder = orderInLayer -1 ;

                tile.row = rowX;
                tile.col = colY;
                tile.currentLayer = orderInLayer;
                tile.tileManager = this;
                tile.isSelected = false;

                var fruit = GetDistributedFruitType();
                tile.typeTileId = fruit;
                tile.spriteFruit.sprite = GetSpriteForFruitType(fruit);
                if (tile.spriteFruit.sprite == null)
                {
                    Debug.LogWarning($"Sprite null for fruitType: {fruit}");
                }

                tile.transform.localScale = new Vector2(sizeToLoad, sizeToLoad);

                tile.name = $"Tile_{colY}_{rowX}";
                tiles[colY, rowX] = tile;
            }
        }

        return tiles;
    }

    private TypeTileId GetDistributedFruitType()
    {

        var fruit = m_DistributedTiles[tileIndex];
        tileIndex++;
        return fruit;
    }

    private Sprite GetSpriteForFruitType(TypeTileId typeTileId)
    {
        if (m_SpriteLookup.TryGetValue(typeTileId, out Sprite sprite))
        {
            return sprite;
        }
        else
        {
            return null;
        }
    }


    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            (list[i], list[randIndex]) = (list[randIndex], list[i]);
        }
    }

    private TypeTileId GetFruitTypeFromSprite(Sprite sprite)
    {
        foreach (var kvp in m_SpriteLookup)
        {
            if (kvp.Value == sprite)
                return kvp.Key;
        }

        return TypeTileId.None;
    }

    public bool IsTileCovered(Tile tile)
    {
        int[,] offsets = { { 0, 0 }, { 1, 0 }, { 0, 1 }, { 1, 1 } };

        foreach (var layer in m_LayerTiles)
        {
            int higherLayer = layer.Key;
            if (higherLayer <= tile.currentLayer) continue;

            Tile[,] higherGrid = layer.Value;
            for (int i = 0; i < offsets.GetLength(0); i++)
            {
                int checkCol = tile.col - offsets[i, 0];
                int checkRow = tile.row - offsets[i, 1];

                if (checkCol >= 0 && checkCol < higherGrid.GetLength(0) &&
                    checkRow >= 0 && checkRow < higherGrid.GetLength(1))
                {
                    Tile coveringTile = higherGrid[checkCol, checkRow];
                    if (coveringTile != null && !coveringTile.isSelected)
                    {
                        return true;
                    }

                }
            }
        }

        return false;
    }

    public void ShuffleTiles()
    {
        List<Tile> remainingTiles = new List<Tile>();
        List<TypeTileId> remainingFruitTypes = new List<TypeTileId>();

        foreach (var layers in m_LayerTiles.Values)
        {
            for (int row = 0; row < layers.GetLength(0); row++)
            {
                for (int col = 0; col < layers.GetLength(1); col++)
                {
                    Tile tile = layers[row, col];
                    if (tile != null && !tile.isSelected)
                    {
                        remainingTiles.Add(tile);
                        TypeTileId typeTileId = GetFruitTypeFromSprite(tile.spriteFruit.sprite);
                        remainingFruitTypes.Add(typeTileId);
                    }
                }
            }
        }

        ShuffleList(remainingFruitTypes);

        for (int i = 0; i < remainingTiles.Count; i++)
        {
            if (m_SpriteLookup.TryGetValue(remainingFruitTypes[i], out Sprite sprite))
            {
                remainingTiles[i].spriteFruit.sprite = sprite;
                remainingTiles[i].typeTileId = remainingFruitTypes[i];

            }
        }
    }

    public bool HasNextLevel()
    {
        return m_TileLevel.Exists(level => level.level == currentLevel + 1);
    }

    public void NextLevel()
    {

        currentLevel += 1;
        m_DistributedTiles = null;
        m_Count.Clear();

        foreach (Transform layer in m_LayerParent.Values)
        {
            Destroy(layer.gameObject);
        }

        m_LayerParent.Clear();
        m_LayerTiles.Clear();
        BoardTileCollector.Instance.ResetGame();
        GenerateTileManager();

    }

    public void ResetTile(int reviveLevel)
    {
        currentLevel = reviveLevel;
        tileIndex = 0;
        m_DistributedTiles = null;
        m_Count.Clear();

        foreach (Transform layer in m_LayerParent.Values)
        {
            Destroy(layer.gameObject);
        }

        m_LayerParent.Clear();
        m_LayerTiles.Clear();
        BoardTileCollector.Instance.ResetGame();
        GenerateTileManager();

    }

    public void SetLayer(Tile tile, int layer)
    {
        tile.transform.SetParent(m_LayerParent[layer]);
    }

    public void CollectSameFruitTiles()
    {
        List<Tile> collected = BoardTileCollector.Instance.GetCollectedTiles();

        if (collected.Count == 0)
        {
            Dictionary<TypeTileId, List<Tile>> availableTiles = new();

            foreach (var layers in m_LayerTiles.Values)
            {
                for (int row = 0; row < layers.GetLength(0); row++)
                {
                    for (int col = 0; col < layers.GetLength(1); col++)
                    {
                        Tile tile = layers[row, col];
                        if (tile != null && !tile.isSelected)
                        {
                            if (!availableTiles.ContainsKey(tile.typeTileId))
                                availableTiles[tile.typeTileId] = new List<Tile>();

                            availableTiles[tile.typeTileId].Add(tile);
                        }
                    }
                }
            }

            List<TypeTileId> validFruitTypes =
                availableTiles.Where(kvp => kvp.Value.Count >= 3).Select(kvp => kvp.Key).ToList();

            TypeTileId random = validFruitTypes[Random.Range(0, validFruitTypes.Count)];
            List<Tile> selectedTiles = availableTiles[random];

            ShuffleList(selectedTiles);
            List<Tile> result = selectedTiles.GetRange(0, 3);

            BoardTileCollector.Instance.MoveSlotTile(result);
            return;
        }

        TypeTileId typeTileId = collected[0].typeTileId;
        int countSlot = collected.Count(t => t.typeTileId == typeTileId);
        int missingCount = 3 - countSlot;

        List<Tile> remaining = FindTilesOfSameFruit(typeTileId, missingCount);

        if (remaining.Count == missingCount)
        {
            BoardTileCollector.Instance.MoveSlotTile(remaining);
        }
    }


    private List<Tile> FindTilesOfSameFruit(TypeTileId typeTileId, int requiredCount = 3)
    {
        List<Tile> result = new List<Tile>();

        foreach (var layers in m_LayerTiles.Values)
        {
            for (int row = 0; row < layers.GetLength(0); row++)
            {
                for (int col = 0; col < layers.GetLength(1); col++)
                {
                    Tile tile = layers[row, col];
                    if (tile != null && !tile.isSelected && tile.typeTileId == typeTileId)
                    {
                        result.Add(tile);

                        if (result.Count >= requiredCount)
                            return result;
                    }
                }
            }
        }

        return result;
    }
    public void ApplyTheme(TileThemeData theme)
    {
        if (theme == null || theme.spriteTile == null || theme.spriteTile.Count == 0)
        {
            Debug.LogWarning("Theme invalid!");
            return;
        }
        
        for (int i = 0; i < spriteDataList.Count; i++)
        {
            spriteDataList[i].sprite = theme.spriteTile[i];
        }
        m_SpriteLookup = new Dictionary<TypeTileId, Sprite>();
        foreach (var data in spriteDataList)
        {
            if (!m_SpriteLookup.ContainsKey(data.typeTileId))
            {
                m_SpriteLookup.Add(data.typeTileId, data.sprite);
            }
        }
        foreach (var layer in m_LayerTiles.Values)
        {
            for (int row = 0; row < layer.GetLength(0); row++)
            {
                for (int col = 0; col < layer.GetLength(1); col++)
                {
                    Tile tile = layer[row, col];
                    if (tile != null)
                    {
                        if (m_SpriteLookup.TryGetValue(tile.typeTileId, out Sprite newSprite))
                        {
                            tile.spriteFruit.sprite = newSprite;
                        }
                    }
                }
            }
        }

    }



}