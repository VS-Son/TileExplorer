using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Manager;
using Project.Scripts.TileTheme;
using Project.Scripts.UI.Manager;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Project.Scripts.Tile
{
    public class TileManager : Singleton<TileManager>
    {
        public int currentLevel;
        private TextAsset _tileJson;
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private List<TileSpriteData> spriteDataList;
        [SerializeField] private ListTileTheme listTileThemeData;
        public GameObject gameplay;
        public int tileIndex;

        private List<LevelData> _tileLevel;
        private readonly Dictionary<TypeId, int> _countTileId = new Dictionary<TypeId, int>();
        private Dictionary<TypeId, Sprite> _spritesLookup;
        private float _sizeToLoad;
        private float _spacingToLoad;


        private List<TypeId> _distributedTiles;

        public readonly Dictionary<int, Tile[,]> LayerTiles = new();

        private readonly Dictionary<int, Transform> _layerParent = new();


        private readonly List<TypeId> _listFruit = new List<TypeId>()
        {
            TypeId.Id0,
            TypeId.Id1,
            TypeId.Id2,
            TypeId.Id3,
            TypeId.Id4,
            TypeId.Id5,
            TypeId.Id6,
            TypeId.Id7,
            TypeId.Id8,
            TypeId.Id9,
        };

        private void Awake()
        {
            LoadJson();
        }

        private void LoadJson()
        {
            _tileJson = Resources.Load<TextAsset>("Json/LevelTile");
        }

        private void LoadThemeTile()
        {
            if (spriteDataList != null)
            {
                TileThemeData confirmedTheme = listTileThemeData.tileThemeData.Find(t => t.isSelected);
                for (int i = 0; i < spriteDataList.Count; i++)
                {
                    spriteDataList[i].sprite = confirmedTheme.spriteTile[i];
                }
            }
           
        }

        void Start()
        {
            OnInit();
        }

        private void OnInit()
        {
            TileJson();
            LoadThemeTile();
            GenerateTileManager();
        }

        private void TileJson()
        {
            LevelTileRoot json = JsonUtility.FromJson<LevelTileRoot>(_tileJson.text);
            _tileLevel = json.LevelTile;

        }

        private void GenerateTileManager()
        {
            _spritesLookup = new Dictionary<TypeId, Sprite>();
            foreach (var data in spriteDataList)
            {
                if (!_spritesLookup.ContainsKey(data.typeId))
                {
                    _spritesLookup.Add(data.typeId, data.sprite);
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

            LevelData levelData = _tileLevel.Find(l => l.level == currentLevel);
            if (levelData != null)
            {
                _sizeToLoad = levelData.size;
                _spacingToLoad = levelData.spacing;
                foreach (var config in layerConfigs)
                {
               
                    var tileConfig = GenerateTile(config.rows, config.cols, config.posY, config.posX, config.tileNameLayer, config.layerSort, config.removeTile);
                    LayerTiles[config.layerSort] = tileConfig;
                    Debug.Log("layer Grid " + LayerTiles.Count);

                }
                SetTilesCovered();
            }
            Debug.Log("Total Tile: " + totalTileCount);
        }

        private List<TileLayerData> LoadLayerConfigs()
        {
            LevelData level = _tileLevel.Find(l =>
            {
                if (l.level == currentLevel) return true;
                return false;
            });

            return level.tileLayer;
        }

        private void CalculateIndexLayerTile(int totalIndex)
        {
            _distributedTiles = GenerateDistributedTiles(totalIndex, _listFruit);
            ShuffleList(_distributedTiles);
            foreach (var tileType in _distributedTiles)
            {
                if (!_countTileId.ContainsKey(tileType)) _countTileId[tileType] = 0;
                _countTileId[tileType]++;
            }

            // foreach (var kv in m_Count)
            // {
            //     Debug.Log($"{kv.Key}: {kv.Value}");
            // }
        }

        private List<TypeId> GenerateDistributedTiles(int totalTilesCreated, List<TypeId> list)
        {
            List<int> itemCounts = GenerateBalancedCountsTile(totalTilesCreated, list.Count);
            ShuffleList(itemCounts);
            List<TypeId> result = new List<TypeId>();
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
            List<Vector2Int> removeIndex)
        {

            Tile[,] tiles = new Tile[cols, rows];
            float offset = (rows - 1) * _spacingToLoad / 2f;
            float offsetY = (cols - 1) * _spacingToLoad / 2f;
            if (!_layerParent.ContainsKey(orderInLayer))
            {
                var layerParent = new GameObject(nameParentLayer);
                _layerParent[orderInLayer] = layerParent.transform;
                layerParent.transform.SetParent(gameplay.transform);

            }

            for (int colY = 0; colY < tiles.GetLength(0); colY++)
            {
                for (int rowX = 0; rowX < tiles.GetLength(1); rowX++)
                {
                    Vector3 spawnPosition = new Vector3((rowX * _spacingToLoad - offset) + x, -colY * _spacingToLoad - offsetY + y, 0);
                    if (removeIndex != null && removeIndex.Contains(new Vector2Int(rowX, colY))) 
                    {
                        continue;
                    }

                    var tile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                    tile.transform.SetParent(_layerParent[orderInLayer]);
                    tile.spriteTile.sortingOrder = orderInLayer;
                    tile.background.sortingOrder = orderInLayer -1 ;

                    tile.row = rowX;
                    tile.col = colY;
                    tile.currentLayer = orderInLayer;
                    tile.isCollected = false;
                    
                    var tileType = GetDistributedTileType();
                    tile.typeId = tileType;
                    tile.spriteTile.sprite = GetSpriteForTileType(tileType);
                    if (tile.spriteTile.sprite == null)
                    {
                        Debug.LogWarning($"Sprite null for tileType: {tileType}");
                    }

                    // if (!HasTileCovered(tile))
                    // {
                    //     tile.spriteTile.color = Color.black;
                    // }
                    tile.transform.localScale = new Vector2(_sizeToLoad, _sizeToLoad);

                    tile.name = $"Tile_{colY}_{rowX}";
                    tiles[colY, rowX] = tile;
                }
            }

            return tiles;
        }
        private void SetTilesCovered()
        {
            foreach (var layerValue in LayerTiles.Values)
            {
                for (int row = 0; row < layerValue.GetLength(0); row++)
                {
                    for (int col = 0; col < layerValue.GetLength(1); col++)
                    {
                        global::Project.Scripts.Tile.Tile tile = layerValue[row, col];
                        if (tile != null)
                        {
                            foreach (var layer in LayerTiles)
                            {
                                var higherLayer = layer.Key;
                                if (higherLayer > tile.currentLayer)
                                {
                                    tile.isSelect = false;
                                    tile.spriteTile.color = tile.background.color = SetAlphaSprite(100);
                                
                                }
                                else
                                {
                                    tile.isSelect = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    
        private TypeId GetDistributedTileType()
        {

            var tileType = _distributedTiles[tileIndex];
            tileIndex++;
            return tileType;
        }

        private Sprite GetSpriteForTileType(TypeId typeId)
        {
            if (_spritesLookup.TryGetValue(typeId, out Sprite sprite))
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

        private TypeId GetFruitTypeFromSprite(Sprite sprite)
        {
            foreach (var kvp in _spritesLookup)
            {
                if (kvp.Value == sprite)
                    return kvp.Key;
            }

            return TypeId.None;
        }
        public void UpdateTileSelect(global::Project.Scripts.Tile.Tile tile)
        {
            int[,] offsets = { { 0, 0 }, { 1, 0 }, { 0, 1 }, { 1, 1 } };
            if (tile.isSelect)
            {
                foreach (var layer in LayerTiles)
                {
                    global::Project.Scripts.Tile.Tile[,] lowerLayer = layer.Value;
                    for (int i = 0; i < offsets.GetLength(0); i++)
                    {
                        int checkCol = tile.col + offsets[i, 0];
                        int checkRow = tile.row + offsets[i, 1];

                        if (checkCol >= 0 && checkCol < lowerLayer.GetLength(0) &&
                            checkRow >= 0 && checkRow < lowerLayer.GetLength(1))
                        {
                            global::Project.Scripts.Tile.Tile lowerTile = lowerLayer[checkCol, checkRow];
                            if (lowerTile != null)
                            {
                                bool covered = HasTileCovered(offsets,lowerTile);
                                lowerTile.isSelect = !covered;
                                lowerTile.spriteTile.color = lowerTile.background.color = (lowerTile.isSelect ? SetAlphaSprite( 255) : SetAlphaSprite(150) );
                             
                            }
                        }
                    }
                }
            }
       
        }

        private bool  HasTileCovered(int[,] offsets, global::Project.Scripts.Tile.Tile lowerTile)
        {

            foreach (var layer in LayerTiles)
            {
                int higherLayer = layer.Key;
                if (higherLayer <= lowerTile.currentLayer) continue;

                global::Project.Scripts.Tile.Tile[,] higherGrid = layer.Value;
                for (int i = 0; i < offsets.GetLength(0); i++)
                {
                    int checkCol = lowerTile.col - offsets[i, 0];
                    int checkRow = lowerTile.row - offsets[i, 1];

                    if (checkCol >= 0 && checkCol < higherGrid.GetLength(0) &&
                        checkRow >= 0 && checkRow < higherGrid.GetLength(1))
                    {
                        global::Project.Scripts.Tile.Tile coveringTile = higherGrid[checkCol, checkRow];
                        if (coveringTile != null && !coveringTile.isCollected)
                        {
                            return true; 
                        }
                    }
                }
            }

            return false;
        }

        private Color SetAlphaSprite(int rgb)
        {
            float value = rgb / 255f;
            var color = new Color(value,value,value, 1f);
            return color;
        }
        public void ShuffleTiles()
        {
            List<global::Project.Scripts.Tile.Tile> remainingTiles = new List<global::Project.Scripts.Tile.Tile>();
            List<TypeId> remainingTileTypes = new List<TypeId>();

            foreach (var layers in LayerTiles.Values)
            {
                for (int row = 0; row < layers.GetLength(0); row++)
                {
                    for (int col = 0; col < layers.GetLength(1); col++)
                    {
                        Tile tile = layers[row, col];
                        if (tile != null && !tile.isCollected)
                        {
                            remainingTiles.Add(tile);
                            TypeId typeId = GetFruitTypeFromSprite(tile.spriteTile.sprite);
                            remainingTileTypes.Add(typeId);
                        }
                    }
                }
            }

            ShuffleList(remainingTileTypes);

            for (int i = 0; i < remainingTiles.Count; i++)
            {
                if (_spritesLookup.TryGetValue(remainingTileTypes[i], out Sprite sprite))
                {
                    remainingTiles[i].spriteTile.sprite = sprite;
                    remainingTiles[i].typeId = remainingTileTypes[i];

                }
            }
        }

        public bool HasNextLevel()
        {
            return _tileLevel.Exists(level => level.level == currentLevel + 1);
        }

        public void NextLevel()
        {

            currentLevel += 1;
            _distributedTiles = null;
            _countTileId.Clear();

            foreach (Transform layer in _layerParent.Values)
            {
                Destroy(layer.gameObject);
            }

            _layerParent.Clear();
            LayerTiles.Clear();
            BoardTileCollector.Instance.ResetGame();
            GenerateTileManager();

        }

        public void ResetTile(int reviveLevel)
        {
            currentLevel = reviveLevel;
            tileIndex = 0;
            _distributedTiles = null;
            _countTileId.Clear();

            foreach (Transform layer in _layerParent.Values)
            {
                Destroy(layer.gameObject);
            }

            _layerParent.Clear();
            LayerTiles.Clear();
            BoardTileCollector.Instance.ResetGame();
            GenerateTileManager();

        }

        public void SetLayer(Tile tile, int layer)
        {
            tile.transform.SetParent(_layerParent[layer]);
        }

        public void CollectSameTiles()
        {
            List<Tile> collected = BoardTileCollector.Instance.GetCollectedTiles();

            if (collected.Count == 0)
            {
                Dictionary<TypeId, List<global::Project.Scripts.Tile.Tile>> availableTiles = new();

                foreach (var layers in LayerTiles.Values)
                {
                    for (int row = 0; row < layers.GetLength(0); row++)
                    {
                        for (int col = 0; col < layers.GetLength(1); col++)
                        {
                            global::Project.Scripts.Tile.Tile tile = layers[row, col];
                            if (tile != null && !tile.isCollected)
                            {
                                if (!availableTiles.ContainsKey(tile.typeId))
                                    availableTiles[tile.typeId] = new List<global::Project.Scripts.Tile.Tile>();

                                availableTiles[tile.typeId].Add(tile);
                            }
                        }
                    }
                }

                List<TypeId> validTileTypes =
                    availableTiles.Where(kvp => kvp.Value.Count >= 3).Select(kvp => kvp.Key).ToList();

                TypeId random = validTileTypes[Random.Range(0, validTileTypes.Count)];
                List<Tile> selectedTiles = availableTiles[random];

                ShuffleList(selectedTiles);
                List<Tile> result = selectedTiles.GetRange(0, 3);

                BoardTileCollector.Instance.MoveSlotTile(result);
                return;
            }

            TypeId typeId = collected[0].typeId;
            int countSlot = collected.Count(t => t.typeId == typeId);
            int missingCount = 3 - countSlot;

            List<global::Project.Scripts.Tile.Tile> remaining = FindTilesOfSame(typeId, missingCount);

            if (remaining.Count == missingCount)
            {
                BoardTileCollector.Instance.MoveSlotTile(remaining);
            }
        }


        private List<Tile> FindTilesOfSame(TypeId typeId, int requiredCount = 3)
        {
            List<global::Project.Scripts.Tile.Tile> result = new List<global::Project.Scripts.Tile.Tile>();

            foreach (var layers in LayerTiles.Values)
            {
                for (int row = 0; row < layers.GetLength(0); row++)
                {
                    for (int col = 0; col < layers.GetLength(1); col++)
                    {
                        global::Project.Scripts.Tile.Tile tile = layers[row, col];
                        if (tile != null && !tile.isCollected && tile.typeId == typeId)
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
            _spritesLookup = new Dictionary<TypeId, Sprite>();
            foreach (var data in spriteDataList)
            {
                if (!_spritesLookup.ContainsKey(data.typeId))
                {
                    _spritesLookup.Add(data.typeId, data.sprite);
                }
            }
            foreach (var layer in LayerTiles.Values)
            {
                for (int row = 0; row < layer.GetLength(0); row++)
                {
                    for (int col = 0; col < layer.GetLength(1); col++)
                    {
                        global::Project.Scripts.Tile.Tile tile = layer[row, col];
                        if (tile != null)
                        {
                            if (_spritesLookup.TryGetValue(tile.typeId, out Sprite newSprite))
                            {
                                tile.spriteTile.sprite = newSprite;
                            }
                        }
                    }
                }
            }

        }



    }
}