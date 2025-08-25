using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
   public Button back;

   private void Start()
   {
      back.onClick.AddListener(OnBack);
   }

   private void OnBack()
   {
      gameObject.SetActive(false);
   }
}
