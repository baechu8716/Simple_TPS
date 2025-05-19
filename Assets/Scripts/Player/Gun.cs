using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//public abstract class Weapon : MonoBehaviour
//{
//    public abstract void Attack(); 
//}
public class Gun : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField][Range(0, 100)] private float _attackRange;
    [SerializeField] private int _shootDamage;
    [SerializeField] private float _shootDelay;
    [SerializeField] private AudioClip _shootSFX;

    private CinemachineImpulseSource _impulse;
    private Camera _camera;
    private bool _canShoot { get => _currentCount <= 0;  }
    private float _currentCount;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        HandleCanShoot();
    }

    private void Init()
    {
        _camera = Camera.main;
        _impulse = GetComponent<CinemachineImpulseSource>();
    }

    public bool Shoot()
    {
        if (!_canShoot) return false;

        PlayShootSound();
        PlayCameraEffect();
        PlayShootEffect();
        _currentCount = _shootDelay;

        // TODO : Ray 발사 -> 반화받은 대상에게 데미지 부여. 몬스터 구현시 같이 구현
        IDamagable target = RayShoot();

        if (target == null) return true;

        target.TakeDamage(_shootDamage);

        // ------------------------------------------ 

        return true;
    }

    private void HandleCanShoot()
    {
        if (_canShoot) return;
        _currentCount -= Time.deltaTime;
    }

    private IDamagable RayShoot()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _attackRange, _targetLayer))
        {
            // TODO : 몬스터를 어떻게 구현하는가에 따라 다름

            // ex : IDamagable를 상속받는 몬스터
            // ??? 이 부분을 어떻게 우회해야 할까
            return ReferenceRegistry.GetProvider(hit.collider.gameObject).
                GetAs<NormalMonster>();

            // ---------------------------------
        }

        return null;
    }

    private void PlayShootSound()
    {
        SFXController sfx = GameManager.Instance.Audio.GetSFX();
        sfx.Play(_shootSFX);
    }

    private void PlayCameraEffect()
    {
        _impulse.GenerateImpulse();
    }
    
    private void PlayShootEffect()
    {
        // TODO : 총구 화염 효과. 파티클로 구현 예정
    }
}
