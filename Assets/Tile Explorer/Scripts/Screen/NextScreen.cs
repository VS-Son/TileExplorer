using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;

public class NextScreen : MonoBehaviour
{
   
   [SerializeField] private Button buttonNextLevel;
   [SerializeField] private TMP_Text textCompleteLevel;
   [SerializeField] private TMP_Text textNextLevel;
   [SerializeField] private PlayScreen gamePlay;
   [SerializeField] private GameObject lockedUndo, lockedMagicWand, lockedShuffle;
   [SerializeField] private Slider slider;
   
   private int nextLevel;

   private void Start()
   {
      buttonNextLevel.onClick.AddListener((() => OnNextLevel()));
   }
   private void OnEnable()
   {
      BoardTileCollector.completeLevel += CompleteLevel;
   }
   private void OnDisable()
   {
      BoardTileCollector.completeLevel -= CompleteLevel;
   }
   
   private void CompleteLevel(int level)
   {
      nextLevel = level;
      textCompleteLevel.text = "Level " + nextLevel;
      textNextLevel.text = "Level " + (nextLevel + 1);
   }

   private void OnNextLevel()
   {
      gamePlay.ShowStatusBar(nextLevel + 1);
      if (nextLevel + 1 > 1)
      {
          lockedUndo.SetActive(false);
      }
      if (nextLevel + 1 > 2)
      {
          lockedShuffle.SetActive(false);  
          var color = gamePlay.shuffle.image.color;
          color.a = 1;
          gamePlay.shuffle.image.color = color;
          var colorIcon = gamePlay.iconShuffle.color;
          colorIcon.a = 1;
          gamePlay.iconShuffle.color = colorIcon;
      }
      if (nextLevel + 1 > 3)
      {
          lockedMagicWand.SetActive(false);
          var color = gamePlay.magicWand.image.color;
          color.a = 1;
          gamePlay.magicWand.image.color = color;
          var colorIcon = gamePlay.iconMagicWand.color;
          colorIcon.a = 1;
          gamePlay.iconMagicWand.color = colorIcon;
      }
      this.gameObject.SetActive(false);
      TileManager.Instance.NextLevel();
   }

   public void ProgressionRewards()
   {
      
      var progression = slider.maxValue / 4;
      if (slider.value.Equals(1))
      {
         slider.value = 0;
      }
      slider.value = Mathf.Min(slider.value + progression, slider.maxValue);
      Debug.LogError(progression);
      
   }  
}
