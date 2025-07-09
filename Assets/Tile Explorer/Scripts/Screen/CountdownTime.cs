using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTime : MonoBehaviour
{
    [SerializeField] private Image fillTimer;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private RectTransform handle;

    [SerializeField] private float handleRadius = 100f;

    public IEnumerator UpdateTimer(float duration, GameObject screen)
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

        screen.SetActive(false);
        TileManager.Instance.ResetTile(TileManager.Instance.currentLevel);
    }
}