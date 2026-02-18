using System.Collections.Generic;
using Project.Scripts.Manager;
using Project.Scripts.TileTheme;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Edit_Tileset
{
   public class EditTileset : Singleton<EditTileset>
   {
      public List<Image> imageTiles = new List<Image>();
      [SerializeField] private Button editsTile;
      [SerializeField] private TileThemeScroller tileThemeScroller;
      public Image panelSetting;

     

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
}
