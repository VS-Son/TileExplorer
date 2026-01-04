using System;
using System.Collections;
using System.Collections.Generic;
using Tile_Explorer.Scripts.Tile;
using UnityEngine;
using DG.Tweening;
using Project.Scripts.UI.Screen;

namespace Project.Scripts.Tile
{
    public class BoardTileCollector : Singleton<BoardTileCollector>
    {
        public static event Action<int> completeLevel;
        [SerializeField] private float duration;
        [SerializeField] private List<Transform> slots;
        public readonly List<global::Tile> collectedTiles = new List<global::Tile>();
        private List<global::Tile> originalTile = new List<global::Tile>();
        private int _tileCount = 0;
        private bool isProcessing = false ;
        private bool isSame = false;
        private bool isMoveSlot;
        private Vector2 originalScale;
        private int count = 0;
        private int slotIndex = 0;
        private Queue<global::Tile> waitSlot = new Queue<global::Tile>();
        private Dictionary<TypeTileId, List<global::Tile>> availableFruits = new Dictionary<TypeTileId, List<global::Tile>>();
        private int countProcessing = 0;
        

        public void AddTileObject(global::Tile tile)
        {
            originalScale = tile.transform.lossyScale;
            tile.originalPosition = tile.transform.position;
            tile.originalLayer = tile.currentLayer;
            
            collectedTiles.Add(tile);
            originalTile.Add(tile);

            tile.spriteTile.sortingOrder = 10;
            tile.background.sortingOrder = 9;

            Debug.Log("Count " + collectedTiles.Count);
            slotIndex = collectedTiles.Count;
            slotIndex = _tileCount;
      
            _tileCount++;
            tile.transform.parent = slots[slotIndex];
            if (collectedTiles.Count > 0 && TileManager.Instance.currentLevel > 1)
            {
                UIManager.Instance.GetUI<PlayScreen>().BadgeUndoActive();
            }
        
            SetSameFruits(tile);
        
        
            tile.transform.DOMove(slots[slotIndex].position, duration).OnComplete((() =>
            {
                ReArrangeTileObjects();
        
                if (_tileCount >= 7)
                {
                    if (isProcessing) return;
                    Debug.Log("Revive");
                    UIManager.Instance.GetUI<ReviveScreen>().ShowCountDownTime(5f);
                }
            }));
            tile.transform.DOScale(tile.transform.localScale + new Vector3(0.2f, 0.2f), 0.4f).SetLink(tile.gameObject)
                .OnComplete((() => { tile.transform.DOScale(1, 0.2f); }));

        }

        public void MoveSlotTile(List<global::Tile> tileList)
        {
        
            slotIndex = collectedTiles.Count;
            foreach (var tile in tileList)
            {
                slotIndex = _tileCount;
                _tileCount ++;
                originalScale = tile.transform.localScale;
                tile.originalPosition = tile.transform.position;
                tile.originalLayer = tile.currentLayer;
                tile.transform.parent = slots[slotIndex];
                Debug.Log(_tileCount);
                collectedTiles.Add(tile);
                originalTile.Add(tile);
                SetSameFruits(tile);
            }
        }

        private void SetSameFruits(global::Tile tile)
        {
            var sameFruit = new List<global::Tile>();
            foreach (var tileObject in collectedTiles)
            {
                if (tileObject.typeTileId == tile.typeTileId)
                {
                    sameFruit.Add(tileObject);
                
                }
            
            }

            if (sameFruit.Count == 1)
            {
                DOVirtual.DelayedCall(0.1f, ReArrangeTileObjects);
                tile.transform.DOMove(slots[slotIndex].position, duration).OnComplete((() =>
                {
                
                    OnRevive(_tileCount);

                }));
                tile.transform.DOScale(tile.transform.localScale + new Vector3(0.2f, 0.2f), 0.4f).SetLink(tile.gameObject)
                    .OnComplete((() =>
                    {
                        tile.transform.DOScale(1, 0.2f);
                    }));

            }

            if (sameFruit.Count == 2)
            {
                Debug.LogWarning("matching tiles");
                global::Tile firstTile = sameFruit[0];
                int targetIndex = collectedTiles.IndexOf(firstTile);

                if (targetIndex != -1)
                {
                    Debug.LogWarning("Tile indices of matching tiles " + (targetIndex + 1));
                    int targetTile = targetIndex + 1;
                    slotIndex = targetTile;
                    collectedTiles.Remove(tile);
                    collectedTiles.Insert(targetIndex + 1, tile);
                    DOVirtual.DelayedCall(0.1f, ReArrangeTileObjects);
                    tile.transform.DOMove(slots[slotIndex].position, duration).OnComplete((() => { 
                        OnRevive(_tileCount);
                    
                    }));
                    tile.transform.DOScale(tile.transform.localScale + new Vector3(0.2f, 0.2f), 0.4f).SetLink(tile.gameObject)
                        .OnComplete((() => { tile.transform.DOScale(1, 0.2f); }));
                }
            }

            if (sameFruit.Count == 3)
            {
                isProcessing = true;
                countProcessing++;
                Debug.LogWarning("matching tiles");
                global::Tile firstTile = sameFruit[1];
                int targetIndex = collectedTiles.IndexOf(firstTile);

                if (targetIndex != -1)
                {
                    Debug.LogWarning("Tile indices of matching tiles " + (targetIndex + 1));
                    int targetTile = targetIndex + 1;
                    slotIndex = targetTile;
                    collectedTiles.Remove(tile);
                    collectedTiles.Insert(targetIndex + 1, tile);
                    DOVirtual.DelayedCall(0.1f, ReArrangeTileObjects);
                    tile.transform.DOMove(slots[slotIndex].position, duration).OnComplete((() =>
                    { 
                        DOVirtual.DelayedCall(0.1f, () =>
                        {
                            StartCoroutine(ScaleMatchingTiles(sameFruit));
                        });                    
                    }));
                    tile.transform.DOScale(tile.transform.localScale + new Vector3(0.2f, 0.2f), 0.4f).SetLink(tile.gameObject)
                        .OnComplete((() => { tile.transform.DOScale(1, 0.3f); }));

                    //ReArrangeTileObjects();

                }

            }
        }

        private void OnRevive(int tileCount)
        {
            if (tileCount >= 7)
            {
                if (isProcessing) return;
                Debug.Log("Revive");
                GameManager.ChangeState(GameState.ReviveScreen );
                UIManager.Instance.GetUI<ReviveScreen>().ShowCountDownTime(5f);
            }
        }

        private void ReArrangeTileObjects()
        {
            for (int i = 0; i < collectedTiles.Count; i++)
            {
                var tile = collectedTiles[i];
                tile.transform.parent = slots[i];
                tile.transform.DOMove(slots[i].position, 0.4f).SetLink(tile.gameObject);
            }
        }





        IEnumerator ScaleMatchingTiles(List<global::Tile> sameFruitTiles)
        {
            foreach (var tile in sameFruitTiles)
            {
                yield return tile.transform.DOScale(tile.transform.localScale + new Vector3(0.2f, 0.2f), 0.05f)
                    .SetEase(Ease.OutBack).OnComplete((() => { tile.transform.DOScale(0.001f, 0.1f); }))
                    .WaitForCompletion();
                yield return new WaitForSeconds(0.01f);

            }


            count++;
            AudioManager.Instance.PlaySfx("destroy");
            foreach (var tile in sameFruitTiles)
            {
                collectedTiles.Remove(tile);
                originalTile.Remove(tile);
                _tileCount--;
                Debug.Log(_tileCount);
                Destroy(tile.gameObject);
            }

       

            ReArrangeTileObjects();
            isProcessing = false;
            DOVirtual.DelayedCall(0.4f, () =>
            {
                countProcessing--;
                if (CheckTilesSelected() && countProcessing == 0 && !isProcessing)
                {
                    if (TileManager.Instance.HasNextLevel())
                    {
                        Debug.Log("win");
                        GameManager.ChangeState(GameState.NextLevel);
                        UIManager.Instance.GetUI<StatusBar>().SetActiveStatus(false);
                        UIManager.Instance.GetUI<NextScreen>().ProgressionRewards();
                        TileManager.Instance.tileIndex = 0;
                        completeLevel?.Invoke(TileManager.Instance.currentLevel);
                    }
                    else
                    {
                        GameManager.ChangeState(GameState.GameOver);
                    }

                }
            });

        }


        private bool CheckTilesSelected()
        {
            var layerGrids = TileManager.Instance.LayerTiles;
            int countLayer = 0;
            foreach (var grid in layerGrids.Values)
            {
                for (int row = 0; row < grid.GetLength(0); row++)
                {
                    for (int col = 0; col < grid.GetLength(1); col++)
                    {
                        global::Tile tile = grid[row, col];
                   
                        if (tile != null && !tile.isCollected)
                        {
                       
                            return false;
                        }
                    }
                }
            }
            return true;
        
        }
    
        public void ResetGame()
        {
            foreach (var tileObject in collectedTiles)
            {
                Destroy(tileObject.gameObject);
            }

            _tileCount = 0;
            collectedTiles.Clear();
            originalTile.Clear();
        }

        public void UndoTiles(int numTiles)
        {
            int countNumberOfTimeUndo = 0;
            int count = Mathf.Min(numTiles, originalTile.Count);
            countNumberOfTimeUndo++;
            for (int i = 0; i < count; i++)
            {
                int index = originalTile.Count - 1 - i;
                global::Tile tile = originalTile[index];

          
                tile.transform.DOMove(tile.originalPosition, 0.4f).SetEase(Ease.InOutQuad).OnComplete((() =>
                {
                  
                }));
                tile.transform.DOScale(originalScale, 0.6f).OnComplete((() =>
                {
                    if (numTiles > 1)
                    {
                        TileManager.Instance.ShuffleTiles();
                        Debug.Log("Shuffle");
                    }
                    Debug.Log("Undo" + countNumberOfTimeUndo);
                }));
                tile.currentLayer = tile.originalLayer;
                tile.spriteTile.sortingOrder = tile.originalLayer;
                tile.background.sortingOrder = tile.originalLayer - 1;
                tile.isCollected = false;
                tile.collider2d.enabled = true;
                tile.transform.SetParent(null);
                tile.collider2d.enabled = true;
                TileManager.Instance.SetLayer(tile, tile.originalLayer);
                TileManager.Instance.UpdateTileSelect(tile);
                collectedTiles.Remove(tile);

            }

            originalTile.RemoveRange(originalTile.Count - count, count);
            _tileCount -= count;
            ReArrangeTileObjects();
        }


        public List<global::Tile> GetCollectedTiles()
        {
            return new List<global::Tile>(collectedTiles);
        }
    }
}