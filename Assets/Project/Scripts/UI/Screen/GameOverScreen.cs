using Project.Scripts.Game;
using Project.Scripts.Tile;
using Project.Scripts.UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Screen
{
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
      GameState.ChangeState(StateUI.PlayScreen);
      CloseDirectly();
      TileManager.Instance.ResetTile(1);
    }
  }
}
