using Project.Scripts.Game;
using Project.Scripts.Sound;
using Project.Scripts.UI.Manager;
using UnityEngine;
using AudioType = Project.Scripts.Sound.AudioType;
using Button = UnityEngine.UI.Button;

namespace Project.Scripts.UI.Screen
{
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
         AudioManager.Instance.PlaySfx(AudioConstants.HighPitchDefault);
         BackKey();
      }

      public override void BackKey()
      {
         CloseDirectly();
      }

      private void SoundOn()
      {
         AudioManager.Instance.PlaySfx(AudioConstants.HighPitchDefault);
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
         AudioManager.Instance.PlaySfx(AudioConstants.HighPitchDefault);
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
}
