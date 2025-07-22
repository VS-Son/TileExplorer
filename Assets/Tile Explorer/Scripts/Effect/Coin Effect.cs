using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEditor.Timeline.Actions;

public class CoinEffect : MonoBehaviour
{
   public static event Action onCompleteGoal;
   public GameObject coinPrefab;            
   public Transform startSpawnPoint;       
   public Transform targetPoint;             
   public int pointPerCoin = 10;            
   public float delayBetweenCoins = 0.1f;   

   private int _currentScore = 0;
   public float moveDuration = 0.6f;       
   public float parabolaHeight = 100f;
   public TMP_Text currentCoin;
   private int currentScore = 0;
   private int goalScore;

   public void RewardPlayer(int rewardPoint)
   {
      int coinCount = rewardPoint / pointPerCoin;
      goalScore = rewardPoint;
      Debug.LogError(coinCount);
      StartCoroutine(SpawnCoins(coinCount));
   }

   IEnumerator SpawnCoins(int count)
   {
      for (int i = 0; i < count; i++)
      {
         GameObject coin = Instantiate(coinPrefab, startSpawnPoint.position, Quaternion.identity, transform);
         StartCoroutine(ParabolaMove(coin, startSpawnPoint.position, targetPoint.position, parabolaHeight, moveDuration));
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
      
      StartCoroutine(AddScoreAnimated(pointPerCoin));  // gọi hiệu ứng cộng điểm

   }
   IEnumerator AddScoreAnimated(int addAmount)
   {
      int startScore = currentScore;
      int targetScore = currentScore + addAmount;
      float duration = 0.001f;
      float elapsed = 0f;

      while (elapsed < duration)
      {
         elapsed += Time.deltaTime;
         float t = elapsed / duration;
         currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, t));
        // currentCoin.text = currentScore.ToString();
         yield return null;
      }

      currentScore = targetScore;
      currentCoin.text = currentScore.ToString();
      if (currentScore >= goalScore)
      {
         onCompleteGoal?.Invoke();
      }
   }

}
