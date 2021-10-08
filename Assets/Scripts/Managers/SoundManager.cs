using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    Dictionary<string, AudioClip> _auidoClips = new Dictionary<string, AudioClip>();

    // MP3 Player  -> AudioSource
    // MP3 음원?   -> AudioClip
    // 관객(귀)    -> AudioListener

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }    
        _auidoClips.Clear();
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        if (type == Define.Sound.Bgm)
        {
            AudioClip audioClip = Managers.Resources.Load<AudioClip>(path);
            if (audioClip == null)
            {
                Debug.Log($"AuidoClip Missing ! {path}");
                return;
            }

            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        else
        {
            AudioClip audioClip = Managers.Resources.Load<AudioClip>(path);
            if (audioClip == null)
            {
                Debug.Log($"AuidoClip Missing ! {path}");
                return;
            }

            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }


    AudioClip GetOrAddAuidClip(string path)
    {
        AudioClip audioClip = null;
        if (_auidoClips.TryGetValue(path, out audioClip) == false)
        {
            audioClip = Managers.Resources.Load<AudioClip>(path);
            _auidoClips.Add(path, audioClip);
        }

        return audioClip;
    }
}
