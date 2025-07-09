using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
public class Tile : MonoBehaviour, IPointerClickHandler
{
    public SpriteRenderer spriteFruit;
    public SpriteRenderer background;
    public FruitType fruitType;

    public int col;
    public int row;
    public int currentLayer;
    public TileManager tileManager;
    public bool isSelected;
    public Collider2D collider2d;
    
    public Vector2 originalPosition;
    public int originalLayer;

    private bool _isShaking;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (TileManager.Instance.IsTileCovered(this))
        {
            if(_isShaking)return;      
            _isShaking = true;
            transform.DOShakePosition(
                duration: 0.3f,
                strength: new Vector3(0.3f, 0f, 0f),
                vibrato: 10,
                randomness: 0f, 
                snapping: false,
                fadeOut: true
            ).OnComplete((() =>
            {
                _isShaking = false;
            }));
            return;
        }
        AudioManager.Instance.PlaySfx("touch");
        isSelected = true;
        collider2d.enabled = false;
        //Debug.Log($"Clicked tile Layer {currentLayer}");
        BoardTileCollector.Instance.AddTileObject(this);
    }
    
}
