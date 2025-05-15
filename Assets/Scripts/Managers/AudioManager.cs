using DesignPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _bgmSource;
    [SerializeField] private List<AudioClip> _bgmList = new();
    [SerializeField] private SFXController _sfxPrefab;

    private ObjectPool _sfxPool;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _bgmSource = GetComponent<AudioSource>();

        _sfxPool = new ObjectPool(_sfxPrefab, transform, 10);
    }

    public void BgmPlay(int index)
    {
        if (index < _bgmList.Count && index > 0)
        {
            _bgmSource.Stop();
            _bgmSource.clip = _bgmList[index];
            _bgmSource.Play();
        }
    }
    public SFXController GetSFX()
    {
        // 풀에서 꺼내와서 반환
        PooledObject po = _sfxPool.PopPool();
        return po as SFXController;
        // = return _sfxPool.PopPool() as SFXController;
    }
}
