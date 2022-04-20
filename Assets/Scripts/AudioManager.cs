﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource Background, Dialogue, SoundEffect;
    private void Awake()
    {
        Instance = this;
        Background.loop = true;
    }

    public void Update()
    {
        //var info = Database.Instance.GlobalInfo;
        //if (info != null)
        //{
        //    Background.volume = info.BackgroundVoice * info.Voice / 10000;
        //    Dialogue.volume = info.DialogueVoice * info.Voice / 10000;
        //    SoundEffect.volume = info.SoundEffectVoice * info.Voice / 10000;
        //}
    }

    public void PlaySoundEffectAudio(AudioClip clip)
    {
        SoundEffect.PlayOneShot(clip);
    }

    public void PlayBackgroundAudio(AudioClip clip)
    {
        Background.clip = clip;
        Background.Play();
    }

    public void PlayDialougue(AudioClip clip)
    {
        Dialogue.clip = clip;
        Dialogue.Play();
    }

    public void PlaySoundEffectAudio(string clipName)
    {
        PlaySoundEffectAudio(ResHelper.GetAsset<AudioClip>(PathHelper.AudioPath + clipName));
    }

    public void PlayBackgroundAudio(string clipName)
    {
        PlayBackgroundAudio(ResHelper.GetAsset<AudioClip>(PathHelper.AudioPath + clipName));

    }

    public void PlayDialougue(string clipName)
    {
        PlayDialougue(ResHelper.GetAsset<AudioClip>(PathHelper.AudioPath + clipName));

    }
}