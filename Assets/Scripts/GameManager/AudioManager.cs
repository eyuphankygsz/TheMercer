using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private List<AudioSource> _sources = new List<AudioSource>();
    [SerializeField] private AudioSource _bgSource;
    private int _line;
    private void Awake()
    {
        Instance = this;
        CreateSources(10);
        _bgSource = gameObject.AddComponent<AudioSource>();
        _bgSource.loop = true;
    }

    public void PlayAudio(AudioClip clip)
    {
        if (!_sources[_line].isPlaying)
        {
            _sources[_line].clip = clip;
            _sources[_line].loop = false;
            _sources[_line].Play();
            _line = (_line + 1) % _sources.Count;
        }
        else
        {
            PlayAudio(clip, 1, false);
        }
    }
    public void PlayRepeatAudio(AudioClip clip)
    {
        if (!_sources[_line].isPlaying)
        {
            _sources[_line].loop = true;
            _sources[_line].clip = clip;
            _sources[_line].Play();
            _line = (_line + 1) % _sources.Count;
        }
        else
        {
            PlayAudio(clip, 1, true);
        }
    }
    public void StopAudio(AudioClip clip)
    {
        for (int i = 0; i < _sources.Count; i++)
            if (_sources[i].clip == clip)
                _sources[i].Stop();
    }
    private void PlayAudio(AudioClip clip, int attempt, bool loop)
    {
        if(attempt >= _sources.Count)
        {
            AudioSource s = CreateSources(1);
            s.loop = loop;
            s.clip = clip;
            s.Play();
        }
        else if (!_sources[_line].isPlaying)
        {
            _sources[_line].loop = loop;
            _sources[_line].clip = clip;
            _sources[_line].Play();
            _line = (_line + 1) % _sources.Count;
        }
        else
        {
            _line = (_line + 1) % _sources.Count;
            PlayAudio(clip, attempt + 1, loop);
        }
    }

    private AudioSource CreateSources(int count)
    {
        int oldCount = _sources.Count;
        for (int i = 0; i < count; i++)
        {
            _sources.Add(gameObject.AddComponent<AudioSource>());
            _sources[_line].volume = 0.6f;
            _line++;
        }
        _line = 0;
        return _sources[oldCount];
    }
    public void ChangeBackground(AudioClip clip)
    {
        _bgSource.Stop();
        _bgSource.clip = clip;
        _bgSource.Play();
    }
}
