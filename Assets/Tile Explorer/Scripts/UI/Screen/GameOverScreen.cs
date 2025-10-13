using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : UICanvas
{
  [SerializeField] private Button startOver;
  [SerializeField] private TMPro.TMP_Text textStartOver;

  private void Start()
  {
    textStartOver.text = "Start Over Level " + 1;
  }

  public void OnStartOver()
  {
    GameManager.ChangeState(GameState.PlayScreen);
    CloseDirectly();
    TileManager.Instance.ResetTile(1);
  }
}
