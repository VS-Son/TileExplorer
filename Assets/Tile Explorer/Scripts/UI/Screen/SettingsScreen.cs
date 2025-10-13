using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class SettingsScreen :UICanvas
{
   [SerializeField] private Button buttonToggleMusic;
   [SerializeField] private GameObject musicOn;
   [SerializeField] private Button buttonSoundToggle;
   [SerializeField] private GameObject soundOn;

   private GameState _gameState;
   
    private void Start()
   {
      buttonToggleMusic.onClick.AddListener(MusicOn);
      buttonSoundToggle.onClick.AddListener(SoundOn);
   }

    
    public void OnClose()
   {
      AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
      BackKey();
   }

    public override void BackKey()
    {
       CloseDirectly();
    }

    private void SoundOn()
   {
      AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
      if (!soundOn.gameObject.activeSelf)
      {
         soundOn.gameObject.SetActive(true);
         AudioManager.Instance.ToggleMute(false, AudioType.Sfx);
      }
      else
      {
         soundOn.gameObject.SetActive(false);
         AudioManager.Instance.ToggleMute(true, AudioType.Sfx); 
      }
      
   }
  
   private void MusicOn()
   {
      AudioManager.Instance.PlaySfx("Button_HighPitch_Default");
      if (!musicOn.gameObject.activeSelf)
      {
         musicOn.gameObject.SetActive(true);
         AudioManager.Instance.ToggleMute(false, AudioType.Bgm);
      }
      else
      {
         musicOn.gameObject.SetActive(false);
         AudioManager.Instance.ToggleMute(true, AudioType.Bgm); 
      }
   }

 
}
