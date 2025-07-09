using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class SettingScreen : MonoBehaviour
{
  // [SerializeField] private GamePlaying gamePlaying;
   [SerializeField] private Button close;
   [SerializeField] private Button musicToggle;
   [SerializeField] private GameObject musicMute;
   [SerializeField] private Button soundToggle;
   [SerializeField] private GameObject soundMute;


   private void Start()
   {
      close.onClick.AddListener(OnClose);
      musicToggle.onClick.AddListener(MusicOn);
      soundToggle.onClick.AddListener(SoundOn);
   }

   private void OnClose()
   {
      AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
      gameObject.SetActive(false);
   }

   private void SoundOn()
   {
      AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
      if (!soundMute.gameObject.activeSelf)
      {
         soundMute.gameObject.SetActive(true);
         AudioManager.Instance.ToggleMute(false, AudioType.Sfx);
      }
      else
      {
         soundMute.gameObject.SetActive(false);
         AudioManager.Instance.ToggleMute(true, AudioType.Sfx); 
      }
      
   }
  
   private void MusicOn()
   {
      AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
      if (!musicMute.gameObject.activeSelf)
      {
         musicMute.gameObject.SetActive(true);
         AudioManager.Instance.ToggleMute(false, AudioType.Bgm);
      }
      else
      {
         musicMute.gameObject.SetActive(false);
         AudioManager.Instance.ToggleMute(true, AudioType.Bgm); 
      }
   }
}
