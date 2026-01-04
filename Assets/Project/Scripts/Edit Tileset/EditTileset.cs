using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditTileset : MonoBehaviour
{
   public static EditTileset Instance;
   public List<Image> imageTiles = new List<Image>();
   [SerializeField] private Button editsTile;
   [SerializeField] private TileThemeScroller tileThemeScroller;
   public Image panelSetting;

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

   private void Start()
   {
      editsTile.onClick.AddListener(OnEditsTile);
      TileThemeData confirmedTheme = tileThemeScroller.listTileTheme.tileThemeData.Find(t => t.isSelected);
      for (int i = 0; i < 3; i++)
      {
         imageTiles[i].sprite = confirmedTheme.spriteTile[i];
      }
   }
   private void OnEditsTile()
   {
      tileThemeScroller.gameObject.SetActive(true);
      SetAlphaPanel(1f);
      tileThemeScroller.SetOutlineSelect();
   }
   public void SetEditTiles(List<Sprite> sprites)
   {
      for (int i = 0; i < 3; i++)
      {
         imageTiles[i].sprite = sprites[i];
      }
   }
   public void SetAlphaPanel(float value)
   {
      var alpha = panelSetting.color;
      alpha.a = Mathf.Clamp01(value);
      panelSetting.color = alpha;
   }
}
