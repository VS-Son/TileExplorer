using System.Collections;
using Project.Scripts.Game;
using Project.Scripts.Tile;
using Project.Scripts.UI.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Screen
{
    public class CountdownTime : MonoBehaviour
    {
        [SerializeField] private Image fillTimer;
        [SerializeField] private TMP_Text timer;
        [SerializeField] private RectTransform handle;
        [SerializeField] private float handleRadius = 100f;
    
        public IEnumerator UpdateTimer(float duration)
        {
            float remainingDuration = duration;

            while (remainingDuration > 0)
            {
                remainingDuration -= Time.deltaTime;

                float clampedTime = Mathf.Max(0, remainingDuration);

                timer.text = clampedTime.ToString("0");

                float fill = clampedTime / duration;
                fillTimer.fillAmount = 1f - (clampedTime / duration);

                float t = 1 - fill; 
                float angle = 90f - t * 360f;
                float x = Mathf.Cos(angle * Mathf.Deg2Rad) * handleRadius;
                float y = Mathf.Sin(angle * Mathf.Deg2Rad) * handleRadius;
                handle.anchoredPosition = new Vector2(x, y);

                yield return null;
            }
        
            GameState.ChangeState(StateUI.PlayScreen);
            UIManager.Instance.GetUI<ReviveScreen>().CloseDirectly();
            TileManager.Instance.ResetTile(TileManager.Instance.currentLevel);
        }
    }
}