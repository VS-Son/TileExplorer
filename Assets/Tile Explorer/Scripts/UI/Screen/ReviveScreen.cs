using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveScreen : UICanvas
{

  

    [SerializeField] private CountdownTime countdownTime;
    
    public void ShowCountDownTime(float duration)
    {
        StartCoroutine(countdownTime.UpdateTimer(duration));
    }
    public void OnRevive()
    {
        GameManager.ChangeState(GameState.PlayScreen);
        CloseDirectly();
        BoardTileCollector.Instance.UndoTiles(5);

    }
}
