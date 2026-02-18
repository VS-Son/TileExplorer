using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Project.Scripts.Manager;
using Project.Scripts.UI.Manager;
using UnityEngine;

namespace Project.Scripts.Sound
{
   public static class AudioConstants
   {
      public const string BGM = "bgm";
      public const string HighPitchDefault = "Button_HighPitch_Default";
      public const string Select = "touch";
      public const string Destroy = "destroy";
   }
   public enum AudioType
   {
      Bgm,
      Sfx
   }
   public class AudioManager :Singleton<AudioManager>
   {
      [Header("Audio")]
      public AudioSource bgmSource;
      public AudioSource sfxSource;
      [Header("Volume")]
      [Range(0f, 1f)] public float bgmVolume = 0.5f;
      [Range(0f, 1f)] public float sfxVolume = 1f;
   
      private readonly Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

      private void Awake()
      {
         LoadAllSfx();
      }

      void Start()
      {
         sfxSource.volume = sfxVolume;
      }
      private void LoadAllSfx()
      {
         AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");

         foreach (var clip in clips)
         {
            var nameClip = clip.name.ToLower();
            Debug.LogWarning("AudioManager Loaded: " + nameClip);
            _audioClips[nameClip] = clip;

         }
      }

      public void PlayBgm(string nameBgm, float duration)
      {
         nameBgm = nameBgm.ToLower();
         if (_audioClips.TryGetValue(nameBgm, out  var clip))
         {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.volume = 0;
            bgmSource.Play();
            bgmSource.volume = bgmVolume;
            bgmSource.DOFade(bgmVolume, duration);
         }
         else
         {
            Debug.LogError("Bgm not found: "+ nameBgm);
         }
     
      }

      public void StopBgm()
      {
         bgmSource.Stop();
      }
      public void PlaySfx(string nameSfx)
      {
         nameSfx = nameSfx.ToLower();

         if (_audioClips.TryGetValue(nameSfx, out var clip))
         {
            sfxSource.PlayOneShot(clip,sfxVolume);
         }
      }
      public void SetSfxVolume(float volume)
      {
         sfxVolume = volume;
      }

      public void ToggleMute(bool isMute, AudioType audioType)
      {
         if (audioType == AudioType.Bgm)
         {
            bgmSource.mute = isMute;

         }
         if(audioType == AudioType.Sfx)
         {
            sfxSource.mute = isMute;

         }
      }

   }
}