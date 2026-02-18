using System;
using System.Collections;
using DG.Tweening;
using Project.Scripts.UI.Manager;
using Project.Scripts.UI.Screen;
using UnityEngine;

namespace Project.Scripts.Effect
{
   public class CoinEffect : MonoBehaviour
   {
      public static event Action OnCompleteGoal;
      public GameObject coinPrefab;            
      public Transform startSpawnPoint;
      private Transform _targetPoint;
      private Transform targetPoint => UIManager.Instance.GetUI<StatusBar>().iconCoin.transform;

      public int pointPerCoin = 10;            
      public float delayBetweenCoins = 0.1f;   

      public float moveDuration = 0.6f;       
      public float parabolaHeight = 100f;
      private int _currentCoin;
      private int _goalScore;
      private int totalCoin;

      public void RewardCoin(int rewardPoint)
      {
         totalCoin = rewardPoint / pointPerCoin;
         _goalScore = 0;
         StartCoroutine(SpawnCoins(totalCoin));
      }

      IEnumerator SpawnCoins(int count)
      {
         for (int i = 0; i < count; i++)
         {
            var startPos = startSpawnPoint.position;
            GameObject coin = Instantiate(coinPrefab, startPos, Quaternion.identity, transform);
            StartCoroutine(ParabolaMove(coin, startPos, targetPoint.position, parabolaHeight, moveDuration));
            yield return new WaitForSeconds(delayBetweenCoins);
         }
      }

      IEnumerator ParabolaMove(GameObject coin, Vector3 start, Vector3 end, float height, float duration)
      {
         float elapsed = 0f;

         while (elapsed < duration)
         {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            Vector3 current = Vector3.Lerp(start, end, t);
            current.x -= height * 7 * t * (1 - t); 

            coin.transform.position = current;

            yield return null;
         }

         Destroy(coin);
         _goalScore++;
         StartCoroutine(AddScoreAnimated(pointPerCoin));
         if (_goalScore >= totalCoin)
         {
            OnCompleteGoal?.Invoke();
         }

      }
      IEnumerator AddScoreAnimated(int addAmount)
      {
         int startScore = _currentCoin;
         int targetScore = _currentCoin + addAmount;
         float duration = 0.001f;
         float elapsed = 0f;

         while (elapsed < duration)
         {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            _currentCoin = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, t));
            //textCurrentCoin.text = textCurrentCoin.ToString();
            yield return null;
         }

         _currentCoin = targetScore;
         targetPoint.transform.DOScale(new Vector2(1.4f, 1.4f), 0.4f).OnComplete((() =>
         {
            targetPoint.transform.DOScale(1, 0.4f);
         }));
         UIManager.Instance.GetUI<StatusBar>().UpdateCoin(pointPerCoin);
        
      }

   }
}
