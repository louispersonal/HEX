using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioClip[] _tracks;
    [SerializeField] AudioSource _musicSource;
    private int _currentTrackIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_musicSource.isPlaying)
        {
            _musicSource.clip = _tracks[_currentTrackIndex];
            _currentTrackIndex = (_currentTrackIndex + 1) % _tracks.Length;
            _musicSource.Play();
        }
    }
}
