using Project.Scripts.Game;
using Project.Scripts.Tile;
using Project.Scripts.UI.Manager;
using UnityEngine;

namespace Project.Scripts.UI.Screen
{
    public class ReviveScreen : UICanvas
    {
        [SerializeField] private CountdownTime countdownTime;
    
        public void ShowCountDownTime(float duration)
        {
            StartCoroutine(countdownTime.UpdateTimer(duration));
        }
        public void OnRevive()
        {
            GameState.ChangeState(StateUI.PlayScreen);
            CloseDirectly();
            BoardTileCollector.Instance.UndoTiles(5);

        }
    }
}
