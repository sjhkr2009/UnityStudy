using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
	Transform _root;
	AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.Count];
	Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

	public void Init()
	{
		//GameObject _root = GameObject.Find(Define.DefaultName.SoundRoot);
		if (_root == null)
		{
			_root = new GameObject(Define.DefaultName.SoundRoot).transform;
			Object.DontDestroyOnLoad(_root);
		}

		string[] soundTypes = System.Enum.GetNames(typeof(Define.Sound));
		for (int i = 0; i < (int)Define.Sound.Count; i++)
		{
			GameObject audio = new GameObject(soundTypes[i]);
			audio.transform.parent = _root;
			_audioSources[i] = audio.AddComponent<AudioSource>();
			_audioSources[i].playOnAwake = false;
		}

		_audioSources[(int)Define.Sound.Bgm].loop = true;
	}

	public void Clear()
	{
		foreach (AudioSource audioSource in _audioSources)
		{
			audioSource.clip = null;
			audioSource.Stop();
		}
		
		_audioClips.Clear();
	}

	public void Play(AudioClip clip, Define.Sound type = Define.Sound.Effect, float volume = 1.0f, float pitch = 1.0f)
	{
		AudioSource audio = _audioSources[(int)type];
		audio.pitch = pitch;
		audio.volume = volume;


		switch (type)
		{
			case Define.Sound.Bgm:
				if (audio.isPlaying)
					audio.Stop();
				audio.clip = clip;
				audio.Play();
				break;
			case Define.Sound.Effect:
				audio.PlayOneShot(clip, volume);
				break;
			default:
				break;
		}
	}

	public void Play(string path, Define.Sound type = Define.Sound.Effect, float volume = 1.0f, float pitch = 1.0f)
	{
		AudioClip clip = GetOrAddAudioClip(path, type);
		if (clip != null)
			Play(clip, type, volume, pitch);
	}

	AudioClip GetOrAddAudioClip(string path, Define.Sound type)
	{
		if(!path.Contains(Define.ResourcesPath.AudioClip))
			path = Define.ResourcesPath.ToAudio(path);

		AudioClip audioClip = null;
		if (type == Define.Sound.Bgm)
		{
			audioClip = GameManager.Resource.Load<AudioClip>(path);
		}
		else if (!_audioClips.TryGetValue(path, out audioClip))
		{
			audioClip = GameManager.Resource.Load<AudioClip>(path);
			_audioClips.Add(path, audioClip);
		}

		if (audioClip == null)
			Debug.Log($"오디오 클립을 찾을 수 없습니다: {path}");

		return audioClip;
	}
}
