using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;
using UnityEngine.Tilemaps;


public class BoardTileCollector : MonoBehaviour
{
    public static BoardTileCollector Instance;
    public static event Action<int> completeLevel;
    [SerializeField] private float duration;
    [SerializeField] private List<Transform> slots;
    [Header("Screen")]
    [SerializeField] private NextScreen nextScreen;
    [SerializeField] private ReviveScreen reviveScreen;
    [SerializeField] private GameOverScreen gameOver;
    [SerializeField] private PlayScreen playScreen;
    
    public readonly List<Tile> collectedTiles = new List<Tile>();
    private List<Tile> originalTile = new List<Tile>();
    private int _tileCount = 0;
    private bool isProcessing = false ;
    private bool isSame = false;
    private bool isMoveSlot;
    private Vector2 originalScale;
    private int count = 0;
    private int slotIndex = 0;
    private Queue<Tile> waitSlot = new Queue<Tile>();
    private Dictionary<FruitType, List<Tile>> availableFruits = new Dictionary<FruitType, List<Tile>>();
    private int countProcessing = 0;


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
    }

    public void AddTileObject(Tile tile)
    {
        originalScale = tile.transform.lossyScale;
        tile.originalPosition = tile.transform.position;
        tile.originalLayer = tile.currentLayer;

        collectedTiles.Add(tile);
        originalTile.Add(tile);

        tile.spriteFruit.sortingOrder = 10;
        tile.background.sortingOrder = 9;

        Debug.Log("Count " + collectedTiles.Count);
        slotIndex = collectedTiles.Count;
        slotIndex = _tileCount;
      
        _tileCount++;
        tile.transform.parent = slots[slotIndex];
        if (collectedTiles.Count > 0 && TileManager.Instance.currentLevel > 1)
        {
            var color = playScreen.undo.image.color;
            color.a = 1;
            playScreen.undo.image.color = color;
            var colorIcon = playScreen.iconUndo.color;
            colorIcon.a = 1;
            playScreen.iconUndo.color = colorIcon;
        }
        SetSameFruits(tile);
        
        
        tile.transform.DOMove(slots[slotIndex].position, duration).OnComplete((() =>
        {
            ReArrangeTileObjects();
        
            if (_tileCount >= 7)
            {
                if (isProcessing) return;
                Debug.Log("Revive");
                reviveScreen.ShowCountDownTime(5f);
            }
        }));
        tile.transform.DOScale(tile.transform.localScale + new Vector3(0.2f, 0.2f), 0.4f).SetLink(tile.gameObject)
            .OnComplete((() => { tile.transform.DOScale(1, 0.2f); }));

    }

    public void MoveSlotTile(List<Tile> tileList)
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

    private void SetSameFruits(Tile tile)
    {
        var sameFruit = new List<Tile>();
        foreach (var tileObject in collectedTiles)
        {
            if (tileObject.fruitType == tile.fruitType)
            {
                sameFruit.Add(tileObject);
                
            }
            
        }

        if (sameFruit.Count == 1)
        {
            DOVirtual.DelayedCall(0.1f, () => {
                ReArrangeTileObjects();
            });
            tile.transform.DOMove(slots[slotIndex].position, duration).OnComplete((() =>
            {
                if (_tileCount >= 7)
                {
                    if (isProcessing) return;
                    Debug.Log("Revive");
                    reviveScreen.ShowCountDownTime(5f);
                }
            }));
            tile.transform.DOScale(tile.transform.localScale + new Vector3(0.2f, 0.2f), 0.4f).SetLink(tile.gameObject)
                .OnComplete((() =>
                {
                    tile.transform.DOScale(1, 0.2f);
                }));
         //  ReArrangeTileObjects();

        }

        if (sameFruit.Count == 2)
        {
            Debug.LogWarning("matching tiles");
            Tile firstTile = sameFruit[0];
            int targetIndex = collectedTiles.IndexOf(firstTile);

            if (targetIndex != -1)
            {
                Debug.LogWarning("Tile indices of matching tiles " + (targetIndex + 1));
                int targetTile = targetIndex + 1;
                slotIndex = targetTile;
                collectedTiles.Remove(tile);
                collectedTiles.Insert(targetIndex + 1, tile);
                DOVirtual.DelayedCall(0.1f, () => {
                    ReArrangeTileObjects();
                });
                tile.transform.DOMove(slots[slotIndex].position, duration).OnComplete((() => { 
                    if (_tileCount >= 7)
                    {
                        if (isProcessing) return;
                        Debug.Log("Revive");
                        DOVirtual.DelayedCall(0.3f, ()=>
                        {
                            reviveScreen.ShowCountDownTime(5f);

                        });
                    }}));
                tile.transform.DOScale(tile.transform.localScale + new Vector3(0.2f, 0.2f), 0.4f).SetLink(tile.gameObject)
                    .OnComplete((() => { tile.transform.DOScale(1, 0.2f); }));
            }
        }

        if (sameFruit.Count == 3)
        {
            isProcessing = true;
            countProcessing++;
            Debug.LogWarning("matching tiles");
            Tile firstTile = sameFruit[1];
            int targetIndex = collectedTiles.IndexOf(firstTile);

            if (targetIndex != -1)
            {
                Debug.LogWarning("Tile indices of matching tiles " + (targetIndex + 1));
                int targetTile = targetIndex + 1;
                slotIndex = targetTile;
                collectedTiles.Remove(tile);
                collectedTiles.Insert(targetIndex + 1, tile);
                DOVirtual.DelayedCall(0.1f, () => {
                    ReArrangeTileObjects();
                });
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

    private void ReArrangeTileObjects()
    {
        for (int i = 0; i < collectedTiles.Count; i++)
        {
            var tile = collectedTiles[i];
            tile.transform.parent = slots[i];
            tile.transform.DOMove(slots[i].position, 0.4f).SetLink(tile.gameObject);
        }
    }



    

    IEnumerator ScaleMatchingTiles(List<Tile> sameFruitTiles)
    {
        foreach (var tile in sameFruitTiles)
        {
            yield return tile.transform.DOScale(tile.transform.localScale + new Vector3(0.2f, 0.2f), 0.05f)
                .SetEase(Ease.OutBack).OnComplete((() => { 
                    tile.transform.DOScale(0.001f, 0.1f);      
                }))
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

        countProcessing--;
        isProcessing = false;
        if (collectedTiles.Count == 0 && TileManager.Instance.currentLevel > 1)
        {
            var color = playScreen.undo.image.color;
            color.a = 0.3f;
            playScreen.undo.image.color = color;
            var colorIcon = playScreen.iconUndo.color;
            colorIcon.a = 0.3f;
            playScreen.iconUndo.color = colorIcon;
        }
        ReArrangeTileObjects();
        DOVirtual.DelayedCall(0.4f, () =>
        {
            if (CheckTilesSelected()&& countProcessing == 0)
            {
                if (TileManager.Instance.HasNextLevel())
                {
                    Debug.Log("win");
                    nextScreen.gameObject.SetActive(true);
                    TileManager.Instance.tileIndex = 0;
                    nextScreen.ProgressionRewards();
                    completeLevel?.Invoke(TileManager.Instance.currentLevel);
                }
                else
                {
                    gameOver.gameObject.SetActive(true);
                    nextScreen.gameObject.SetActive(false);
                }

            }
        });

    }
    
    private bool CheckTilesSelected()
    {
        var layerGrids = TileManager.Instance.m_LayerTiles;
        int countLayer = 0;
        foreach (var grid in layerGrids.Values)
        {
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    Tile tile = grid[row, col];
                   
                    if (tile != null && !tile.isSelected)
                    {
                       
                        return false;
                    }
                }
            }
        }
        return true;
        
    }
    public bool IsLastMatchingTripleRemoved(FruitType fruitType)
    {
        int count = 0;
        var layerGrids = TileManager.Instance.m_LayerTiles;

        foreach (var grid in layerGrids.Values)
        {
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    Tile tile = grid[row, col];
                    if (tile != null && !tile.isSelected && tile.fruitType == fruitType)
                    {
                        count++;
                    }
                }
            }
        }

        // Nếu còn dưới 3 tile giống nhau => đã gom hết nhóm cuối
        return count < 3;
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

    public void RevertTiles(int numTiles)
    {
        int count = Mathf.Min(numTiles, originalTile.Count);

        for (int i = 0; i < count; i++)
        {
            int index = originalTile.Count - 1 - i;
            Tile tile = originalTile[index];

          
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
                Debug.Log("Undo");
            }));
            // if (originalTile.Count == 0)
            // {
            //     var color = gamePlaying.undo.image.color;
            //     color.a = 0.3f;
            //     gamePlaying.undo.image.color = color;
            //     var colorIcon = gamePlaying.iconUndo.color;
            //     colorIcon.a = 0.3f;
            //     gamePlaying.iconUndo.color = colorIcon;
            // }
            tile.currentLayer = tile.originalLayer;
            tile.spriteFruit.sortingOrder = tile.originalLayer;
            tile.background.sortingOrder = tile.originalLayer - 1;
            tile.isSelected = false;
            tile.transform.SetParent(null);
            tile.collider2d.enabled = true;
            TileManager.Instance.SetLayer(tile, tile.originalLayer);
            collectedTiles.Remove(tile);

        }

        originalTile.RemoveRange(originalTile.Count - count, count);
        _tileCount -= count;
       ReArrangeTileObjects();
    }


    public List<Tile> GetCollectedTiles()
    {
        return new List<Tile>(collectedTiles);
    }
}