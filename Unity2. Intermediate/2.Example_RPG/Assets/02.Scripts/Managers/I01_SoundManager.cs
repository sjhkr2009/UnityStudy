using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I01_SoundManager
{
    AudioSource[] audioSources = new AudioSource[(int)E02_Define.Sound.Count];
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject sound = GameObject.Find("@Sound");
        if (sound == null) sound = new GameObject("@Sound");
        
        Object.DontDestroyOnLoad(sound);

        string[] soundTypes = System.Enum.GetNames(typeof(E02_Define.Sound));
        for (int i = 0; i < soundTypes.Length - 1; i++)
        {
            GameObject audio = new GameObject(soundTypes[i]);
            audio.transform.parent = sound.transform;
            audioSources[i] = audio.AddComponent<AudioSource>();
            audioSources[i].playOnAwake = false;
        }

        audioSources[(int)E02_Define.Sound.BGM].loop = true;
        audioSources[(int)E02_Define.Sound.AtPoint].spatialBlend = 1f;
        audioSources[(int)E02_Define.Sound.AtPoint].rolloffMode = AudioRolloffMode.Linear;
        audioSources[(int)E02_Define.Sound.AtPoint].maxDistance = 20f;
    }

    public void Play(AudioClip clip, E02_Define.Sound type = E02_Define.Sound.Effect, float pitch = 1f, float volume = 1f, Vector3 position = new Vector3())
    {
        AudioSource audio = audioSources[(int)type];
        audio.pitch = pitch;
        audio.volume = volume;

        switch (type)
        {
            case E02_Define.Sound.BGM:
                if (audio.isPlaying) audio.Stop();
                audio.clip = clip;
                audio.Play();
                break;

            case E02_Define.Sound.Effect:
                audio.PlayOneShot(clip, volume);
                break;

            case E02_Define.Sound.AtPoint:
                if (audio.isPlaying) audio.Stop();
                audio.clip = clip;
                audio.transform.position = position;
                audio.Play();
                break;
        }
    }

    public void Play(string path, E02_Define.Sound type = E02_Define.Sound.Effect, float pitch = 1f, float volume = 1f, Vector3 position = new Vector3())
    {
        AudioClip source = GetOrAddAudioClip(path, type);

        if (source == null)
        {
            Debug.Log($"'{path}' 오디오 클립을 찾을 수 없습니다.");
            return;
        }

        // 함수를 오버로드할 때는 코드를 복붙하지 말고, 반복되는 코드는 다른 버전의 함수를 호출한다.
        // 그렇지 않으면 수정할 때 모든 함수를 동일하게 수정해야 한다.
        Play(source, type, pitch, volume, position);
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        audioClips.Clear();
    }

    // 매번 사운드를 리소스에서 로드하는 것은 다소 부담이 갈 수 있으니, 로드된 사운드 정보를 저장해두고 사용한다.
    // 단, 오디오 역할을 할 오브젝트가 Don't Destroy 처리되어 있으므로, 메모리가 무한이 축적되는 일을 방지하기 위해 Clear를 통해 씬이 바뀔때마다 저장된 값을 초기화해준다.
    AudioClip GetOrAddAudioClip(string path, E02_Define.Sound type)
    {
        AudioClip audioClip = null;
        if (!path.Contains("Sounds/")) path = $"Sounds/{path}";

        // BGM은 자주 실행되지 않으니 저장하지 않는다.
        if (type == E02_Define.Sound.BGM)
        {
            audioClip = A01_Manager.Resource.Load<AudioClip>(path);
        }
        else if (!audioClips.TryGetValue(path, out audioClip))
        {
            audioClip = A01_Manager.Resource.Load<AudioClip>(path);
            audioClips.Add(path, audioClip);
        }
        return audioClip;
    }
}
