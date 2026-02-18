using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Project.Scripts.Game;
using Project.Scripts.Manager;
using Project.Scripts.Sound;
using Project.Scripts.UI.Manager;
using Project.Scripts.UI.Screen;

namespace Project.Scripts.Tile
{
    public class BoardTileCollector : Singleton<BoardTileCollector>
    {
        public static event Action<int> CompleteLevel;
        
        [SerializeField] private float duration;
        [SerializeField] private List<Transform> slots;
        
        public readonly List<global::Project.Scripts.Tile.Tile> CollectedTiles = new List<global::Project.Scripts.Tile.Tile>();
        private readonly List<global::Project.Scripts.Tile.Tile> _originalTile = new List<global::Project.Scripts.Tile.Tile>();
        private int _tileCount = 0;
        private bool _isProcessing = false ;
        private bool _isMoveSlot;
        private Vector2 _originalScale;
        private int _slotIndex = 0;
        private int _countProcessing = 0;

        
        public void AddTileObject(global::Project.Scripts.Tile.Tile tile)
        {
            _originalScale = tile.transform.lossyScale;
            tile.originalPosition = tile.transform.position;
            tile.originalLayer = tile.currentLayer;
            
            CollectedTiles.Add(tile);
            _originalTile.Add(tile);

            tile.spriteTile.sortingOrder = 10;
            tile.background.sortingOrder = 9;

            Debug.Log("Count " + CollectedTiles.Count);
            _slotIndex = CollectedTiles.Count;
            _slotIndex = _tileCount;
      
            _tileCount++;
            tile.transform.parent = slots[_slotIndex];
            if (CollectedTiles.Count > 0 && TileManager.Instance.currentLevel > 1)
            {
                UIManager.Instance.GetUI<PlayScreen>().BadgeUndoActive();
            }
        
            SetSameFruits(tile);
        
        
            tile.transform.DOMove(slots[_slotIndex].position, duration).OnComplete((() =>
            {
                ReArrangeTileObjects();
        
                if (_tileCount >= 7)
                {
                    if (_isProcessing) return;
                    Debug.Log("Revive");
                    UIManager.Instance.GetUI<ReviveScreen>().ShowCountDownTime(5f);
                }
            }));
            tile.transform.DOScale(tile.transform.localScale + new Vector3(0.2f, 0.2f), 0.4f).SetLink(tile.gameObject)
                .OnComplete((() => { tile.transform.DOScale(1, 0.2f); }));

        }

        public void MoveSlotTile(List<global::Project.Scripts.Tile.Tile> tileList)
        {
        
            _slotIndex = CollectedTiles.Count;
            foreach (var tile in tileList)
            {
                _slotIndex = _tileCount;
                _tileCount ++;
                _originalScale = tile.transform.localScale;
                tile.originalPosition = tile.transform.position;
                tile.originalLayer = tile.currentLayer;
                tile.transform.parent = slots[_slotIndex];
                Debug.Log(_tileCount);
                CollectedTiles.Add(tile);
                _originalTile.Add(tile);
                SetSameFruits(tile);
            }
        }

        private void SetSameFruits(global::Project.Scripts.Tile.Tile tile)
        {
            var sameFruit = new List<global::Project.Scripts.Tile.Tile>();
            foreach (var tileObject in CollectedTiles)
            {
                if (tileObject.typeId == tile.typeId)
                {
                    sameFruit.Add(tileObject);
                
                }
            
            }

            if (sameFruit.Count == 1)
            {
                DOVirtual.DelayedCall(0.1f, ReArrangeTileObjects);
                tile.transform.DOMove(slots[_slotIndex].position, duration).OnComplete((() =>
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
                global::Project.Scripts.Tile.Tile firstTile = sameFruit[0];
                int targetIndex = CollectedTiles.IndexOf(firstTile);

                if (targetIndex != -1)
                {
                    Debug.LogWarning("Tile indices of matching tiles " + (targetIndex + 1));
                    int targetTile = targetIndex + 1;
                    _slotIndex = targetTile;
                    CollectedTiles.Remove(tile);
                    CollectedTiles.Insert(targetIndex + 1, tile);
                    DOVirtual.DelayedCall(0.1f, ReArrangeTileObjects);
                    tile.transform.DOMove(slots[_slotIndex].position, duration).OnComplete((() => { 
                        OnRevive(_tileCount);
                    
                    }));
                    tile.transform.DOScale(tile.transform.localScale + new Vector3(0.2f, 0.2f), 0.4f).SetLink(tile.gameObject)
                        .OnComplete((() => { tile.transform.DOScale(1, 0.2f); }));
                }
            }

            if (sameFruit.Count == 3)
            {
                _isProcessing = true;
                _countProcessing++;
                Debug.LogWarning("matching tiles");
                global::Project.Scripts.Tile.Tile firstTile = sameFruit[1];
                int targetIndex = CollectedTiles.IndexOf(firstTile);

                if (targetIndex != -1)
                {
                    Debug.LogWarning("Tile indices of matching tiles " + (targetIndex + 1));
                    int targetTile = targetIndex + 1;
                    _slotIndex = targetTile;
                    CollectedTiles.Remove(tile);
                    CollectedTiles.Insert(targetIndex + 1, tile);
                    DOVirtual.DelayedCall(0.1f, ReArrangeTileObjects);
                    tile.transform.DOMove(slots[_slotIndex].position, duration).OnComplete((() =>
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
                if (_isProcessing) return;
                Debug.Log("Revive");
                GameState.ChangeState(StateUI.ReviveScreen );
                UIManager.Instance.GetUI<ReviveScreen>().ShowCountDownTime(5f);
            }
        }

        private void ReArrangeTileObjects()
        {
            for (int i = 0; i < CollectedTiles.Count; i++)
            {
                var tile = CollectedTiles[i];
                tile.transform.parent = slots[i];
                tile.transform.DOMove(slots[i].position, 0.4f).SetLink(tile.gameObject);
            }
        }





        IEnumerator ScaleMatchingTiles(List<global::Project.Scripts.Tile.Tile> sameFruitTiles)
        {
            foreach (var tile in sameFruitTiles)
            {
                yield return tile.transform.DOScale(tile.transform.localScale + new Vector3(0.2f, 0.2f), 0.05f)
                    .SetEase(Ease.OutBack).OnComplete((() => { tile.transform.DOScale(0.001f, 0.1f); }))
                    .WaitForCompletion();
                yield return new WaitForSeconds(0.01f);

            }


            AudioManager.Instance.PlaySfx(AudioConstants.Destroy);
            foreach (var tile in sameFruitTiles)
            {
                CollectedTiles.Remove(tile);
                _originalTile.Remove(tile);
                _tileCount--;
                Debug.Log(_tileCount);
                Destroy(tile.gameObject);
            }

       

            ReArrangeTileObjects();
            _isProcessing = false;
            DOVirtual.DelayedCall(0.4f, () =>
            {
                _countProcessing--;
                if (CheckTilesSelected() && _countProcessing == 0 && !_isProcessing)
                {
                    if (TileManager.Instance.HasNextLevel())
                    {
                        Debug.Log("win");
                        GameState.ChangeState(StateUI.NextLevel);
                        UIManager.Instance.GetUI<StatusBar>().SetActiveStatus(false);
                        UIManager.Instance.GetUI<NextScreen>().ProgressionRewards();
                        TileManager.Instance.tileIndex = 0;
                        CompleteLevel?.Invoke(TileManager.Instance.currentLevel);
                    }
                    else
                    {
                        GameState.ChangeState(StateUI.GameOver);
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
                        global::Project.Scripts.Tile.Tile tile = grid[row, col];
                   
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
            foreach (var tileObject in CollectedTiles)
            {
                Destroy(tileObject.gameObject);
            }

            _tileCount = 0;
            CollectedTiles.Clear();
            _originalTile.Clear();
        }

        public void UndoTiles(int numTiles)
        {
            int countNumberOfTimeUndo = 0;
            int count = Mathf.Min(numTiles, _originalTile.Count);
            countNumberOfTimeUndo++;
            for (int i = 0; i < count; i++)
            {
                int index = _originalTile.Count - 1 - i;
                global::Project.Scripts.Tile.Tile tile = _originalTile[index];

          
                tile.transform.DOMove(tile.originalPosition, 0.4f).SetEase(Ease.InOutQuad).OnComplete((() =>
                {
                  
                }));
                tile.transform.DOScale(_originalScale, 0.6f).OnComplete((() =>
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
                CollectedTiles.Remove(tile);

            }

            _originalTile.RemoveRange(_originalTile.Count - count, count);
            _tileCount -= count;
            ReArrangeTileObjects();
        }


        public List<global::Project.Scripts.Tile.Tile> GetCollectedTiles()
        {
            return new List<global::Project.Scripts.Tile.Tile>(CollectedTiles);
        }
    }
}