using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamagable
{
    public bool IsControlActivate { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;
    private Animator _animator;
    private Image _aimImage;
   
    [SerializeField] private CinemachineVirtualCamera _aimCamera;
    [SerializeField] private Gun _gun;
    [SerializeField] private KeyCode _aimKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode _shootKey = KeyCode.Mouse0;
    [SerializeField] private Animator _aimAinmator;
    [SerializeField] private HPGuageUI _hpUI;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void Update()
    {
        HandlePlayerControl();
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void Init()
    {
        _status = GetComponent<PlayerStatus>();
        _movement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
        _aimImage = _aimAinmator.GetComponent<Image>();

        _hpUI.SetImageFillAmount(1);
        _status.CurrentHP.Value = _status.MaxHP;
        // _mainCamera = Camera.main.gameObject;
    }
    
    private void HandlePlayerControl()
    {
        if (!IsControlActivate) return;

        HandleMovement();
        HandleAiming();
        HandleShooting();

        if(Input.GetKey(KeyCode.Alpha1))
        {
            TakeDamage(1);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            RecoveryHP(1);
        }
    }

    private void HandleShooting()
    {
        if(Input.GetKey(_shootKey) && _status.IsAiming.Value)
        {
            _status.IsAttacking.Value = _gun.Shoot();
        }
        else
        {
            _status.IsAttacking.Value = false;
        }
    }

    private void HandleMovement()
    {
        Vector3 camRotateDir = _movement.SetAimRotation();

        float moveSpeed;
        if (_status.IsAiming.Value) moveSpeed = _status.WalkSpeed;
        else moveSpeed = _status.RunSpeed;

        Vector3 moveDir = _movement.SetMove(moveSpeed);
        // 이벤트 
        _status.IsMoving.Value = (moveDir != Vector3.zero);

        Vector3 avatarDir;
        if (_status.IsAiming.Value) avatarDir = camRotateDir;
        else avatarDir = moveDir;

        _movement.SetAvatarRotation(avatarDir);

        // SetAnimationParameter
        if(_status.IsAiming.Value)
        {
            Vector3 input = _movement.GetInputDirection();
            _animator.SetFloat("X", input.x);
            _animator.SetFloat("Z", input.z);
        }
    }

    private void HandleAiming()
    {
        _status.IsAiming.Value = Input.GetKey(_aimKey);
    }

    public void TakeDamage(int value)
    {
        // 체력으로 떨어뜨리되, 0이 되면 플레이어가 죽도록 처리
        _status.CurrentHP.Value -= value;
        if (_status.CurrentHP.Value <= 0) Dead();
    }

    public void RecoveryHP(int value)
    {
        // 체력을 회복시키되, MaxHP보다 초과되는것을 막아야함
        int hp = _status.CurrentHP.Value + value;

        _status.CurrentHP.Value = Mathf.Clamp(hp, 0, _status.MaxHP);
    }

    public void Dead()
    {
        Debug.Log("플레이어 사망 처리");
    }

    private void SubscribeEvents()
    {
        _status.CurrentHP.Subscribe(SetHpUIGuage);

        _status.IsMoving.Subscribe(SetMoveAnimaion);

        _status.IsAiming.Subscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Subscribe(SetAimAnimation);

        _status.IsAttacking.Subscribe(SetAttackAnimation);
    }

    private void UnSubscribeEvents()
    {
        _status.CurrentHP.Unsubscribe(SetHpUIGuage);

        _status.IsMoving.Unsubscribe(SetMoveAnimaion);

        _status.IsAiming.Unsubscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Unsubscribe(SetAimAnimation);

        _status.IsAttacking.Unsubscribe(SetAttackAnimation);
    }

    private void SetAimAnimation(bool value)
    {
        // 에임 에니메티어 게임오브젝트가 비활성화 상태라면
        if (!_aimImage.enabled)
        {
            _aimImage.enabled = true;
        }
        _animator.SetBool("IsAim", value);
        _aimAinmator.SetBool("IsAim", value);
    } 

    private void SetMoveAnimaion(bool value) => _animator.SetBool("IsMove", value);

    private void SetAttackAnimation(bool value) => _animator.SetBool("IsAttack", value);

    private void SetHpUIGuage(int currentHP)
    {
        // 현재수치 / 최대수치
        float hp =  currentHP / (float)_status.MaxHP;
        _hpUI.SetImageFillAmount(hp);
    }
}
//namespace YTW_Test
//{
//    public class PlayerController : MonoBehaviour
//    {
//        public PlayerMovement _movement;
//        public PlayerStatus _status;

//        /// <summary>
//        /// 아래 메서드에 적힌 소스코드와 같은 방식으로 사용합니다.
//        /// </summary>
//        private void Update()
//        {
//            MoveTest();

//            // IsAiming 변경용 테스트 코드
//            _status.IsAiming.Value = Input.GetKey(KeyCode.Mouse1);
//        }

//        public void MoveTest()
//        {
//            // (회전 수행 후) 좌우 회전에 대한 벡터 반환
//            Vector3 camRotateDir = _movement.SetAimRotation();

//            float moveSpeed;
//            if (_status.IsAiming.Value == true)
//            {
//                moveSpeed = _status.WalkSpeed;
//            }
//            else
//            {
//                moveSpeed = _status.RunSpeed;
//            }

//            Vector3 moveDir = _movement.SetMove(moveSpeed);
//            _status.IsMoving.Value = (moveDir != Vector3.zero);

//            // 에임조준상태라면 플레이어를 카메라가 보는 방향으로 회전
//            Vector3 avatarDir;
//            if(_status.IsAiming.Value)
//            {
//                avatarDir = camRotateDir;
//            }
//            else
//            {
//                avatarDir = moveDir;
//            }

//            _movement.SetAvatarRotation(avatarDir);
//        }
//    }
//}

