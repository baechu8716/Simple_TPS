using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DesignPattern;

public class SFXController : PooledObject
{
    private AudioSource _audioSource;
    private float _currentCount;

    private void Awake()
    {
        Init();
    }

    //private void Update()
    //{
    //    // 한 프레임이 갱신될때까지의 시간 =  DletaTime
    //    _currentCount -= Time.deltaTime;

    //    if (_currentCount <= 0)
    //    {
    //        _audioSource.Stop();
    //        _audioSource.clip = null;
    //        ReturnPool();
    //    }
    //}

    private void Init()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // 오디오클립 = 음악 파일
    public void Play(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();

        _currentCount = clip.length;
    }
}
