using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;


public class AudioManager : MonoBehaviour
{
   public static AudioManager Instance;
   [Header("Audio")]
   public AudioSource bgmSource;
   public AudioSource sfxSource;
   [Header("Volume")]
   [Range(0f, 1f)] public float bgmVolume = 0.5f;
   [Range(0f, 1f)] public float sfxVolume = 1f;
   
   private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

   private void Awake()
   {
      if (Instance != null && Instance != this)
      {
         Destroy(this);
      }
      else
      {
         Instance = this;
         LoadAllSfx();
      }
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
         var name = clip.name.ToLower();
         Debug.LogWarning("AudioManager Loaded: " + name);
         audioClips[name] = clip;

      }
   }

   public void PlayBgm(string nameBgm, float duration)
   {
      nameBgm = nameBgm.ToLower();
      if (audioClips.TryGetValue(nameBgm, out  var clip))
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
   public void PlaySfx(string name)
   {
      name = name.ToLower();

      if (audioClips.TryGetValue(name, out var clip))
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
public enum AudioType
{
   Bgm,
   Sfx
}
