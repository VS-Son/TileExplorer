using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using Project.Scripts.Tile;
using Tile_Explorer.Scripts.Tile;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public SpriteRenderer spriteTile;
    public SpriteRenderer background;
    public TypeTileId typeTileId;

    public int col;
    public int row;
    public int currentLayer;
    public TileManager tileManager;
    public bool isCollected;
    public bool isSelect;
    public Collider2D collider2d;
    
    public Vector2 originalPosition;
    public int originalLayer;

    private bool _isShaking;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (!isSelect)
        {
            if (_isShaking) return;
            _isShaking = true;
            transform.DOShakePosition(
                duration: 0.3f,
                strength: new Vector3(0.1f, 0f, 0f),
                vibrato: 10,
                randomness: 0f,
                snapping: false,
                fadeOut: true
            ).OnComplete(() => { _isShaking = false; });
            return;
        }

        AudioManager.Instance.PlaySfx("touch");
        isCollected = true;
        collider2d.enabled = false;
        TileManager.Instance.UpdateTileSelect(this);
        BoardTileCollector.Instance.AddTileObject(this);
    }

    
}
