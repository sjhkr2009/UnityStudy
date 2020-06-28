using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoundManager
{
    AudioSource[] audio = new AudioSource[(int)Define.SoundType.Count];
    
    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
            root = new GameObject("@Sound");

        Object.DontDestroyOnLoad(root);

        for (int i = 0; i < audio.Length; i++)
        {
            GameObject _audio = new GameObject($"Audio_{(Define.SoundType)i}");
            audio[i] = _audio.GetOrAddComponent<AudioSource>();
            _audio.transform.parent = root.transform;
        }
        audio[(int)Define.SoundType.BGM].loop = true;
        audio[(int)Define.SoundType.FX].loop = false;
    }

    public void Play(Define.SoundType type, string path, float volume = 1f)
    {
        AudioClip source = null;

        switch (type)
        {
            case Define.SoundType.BGM:
                source = GameManager.Resource.Load<AudioClip>($"SoundSources/BGM/{path}");
                audio[(int)type].clip = source;
                audio[(int)type].volume = volume;
                audio[(int)type].Play();
                break;
            case Define.SoundType.FX:
                source = GameManager.Resource.Load<AudioClip>($"SoundSources/FX/{path}");
                audio[(int)type].PlayOneShot(source, volume);
                break;
        }
    }
}
