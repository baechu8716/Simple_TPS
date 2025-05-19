using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamagable
{


    public bool IsControlActivate { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;
    private Animator _animator;
    private Image _aimImage;
    private InputAction _aimInputAction;
    private InputAction _shootInputAction;
   
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
        _aimInputAction = GetComponent<PlayerInput>().actions["Aim"];
        _shootInputAction = GetComponent<PlayerInput>().actions["Shoot"];

        _hpUI.SetImageFillAmount(1);
        _status.CurrentHP.Value = _status.MaxHP;
        // _mainCamera = Camera.main.gameObject;
    }
    
    private void HandlePlayerControl()
    {
        if (!IsControlActivate) return;

        HandleMovement();
        //HandleAiming();
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

    public void HandleShooting()
    {
        // _shootInputAction.WasPressedThisFrame() => �̹� �����ӿ� ���ȴ°�? (GetKeyDown)
        // _shootInputAction.WasReleasedThisFrame() => �̹� �����ӿ� �������°�? (GetKeyUp)
        // _shootInputAction.IsPressed() => ���� �����ִ°�? (GetKey)


        if (_status.IsAiming.Value && _shootInputAction.IsPressed())
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
        // �̺�Ʈ 
        _status.IsMoving.Value = (moveDir != Vector3.zero);

        Vector3 avatarDir;
        if (_status.IsAiming.Value) avatarDir = camRotateDir;
        else avatarDir = moveDir;

        _movement.SetAvatarRotation(avatarDir);

        // SetAnimationParameter
        if (_status.IsAiming.Value)
        {
            //Vector3 input = _movement.InputDirection;
            //_animator.SetFloat("X", input.x);
            //_animator.SetFloat("Z", input.z);

            _animator.SetFloat("X", _movement.InputDirection.x);
            _animator.SetFloat("Z", _movement.InputDirection.y);
        }
    }

    private void HandleAiming(InputAction .CallbackContext ctx)
    {
        // _status.IsAiming.Value = Input.GetKey(_aimKey);
        _status.IsAiming.Value = ctx.started;
        // ctx.started => Ű�Է��� ���۵ƴ��� // ctx.performed => Ű�Է��� ���������� //ctx.canceled => Ű�Է��� ��ҵƴ���

        
    }

    public void TakeDamage(int value)
    {
        // ü������ ����߸���, 0�� �Ǹ� �÷��̾ �׵��� ó��
        _status.CurrentHP.Value -= value;
        if (_status.CurrentHP.Value <= 0) Dead();
    }

    public void RecoveryHP(int value)
    {
        // ü���� ȸ����Ű��, MaxHP���� �ʰ��Ǵ°��� ���ƾ���
        int hp = _status.CurrentHP.Value + value;

        _status.CurrentHP.Value = Mathf.Clamp(hp, 0, _status.MaxHP);
    }

    public void Dead()
    {
        Debug.Log("�÷��̾� ��� ó��");
    }

    private void SubscribeEvents()
    {
        _status.CurrentHP.Subscribe(SetHpUIGuage);

        _status.IsMoving.Subscribe(SetMoveAnimaion);

        _status.IsAiming.Subscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Subscribe(SetAimAnimation);

        _status.IsAttacking.Subscribe(SetAttackAnimation);

        // input;
        _aimInputAction.Enable();
        _aimInputAction.started += HandleAiming;
        _aimInputAction.canceled += HandleAiming;

    }

    private void UnSubscribeEvents()
    {
        _status.CurrentHP.Unsubscribe(SetHpUIGuage);

        _status.IsMoving.Unsubscribe(SetMoveAnimaion);

        _status.IsAiming.Unsubscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Unsubscribe(SetAimAnimation);

        _status.IsAttacking.Unsubscribe(SetAttackAnimation);

        // inputs
        _aimInputAction.Disable();
        _aimInputAction.started -= HandleAiming;
        _aimInputAction.canceled -= HandleAiming;


    }

    private void SetAimAnimation(bool value)
    {
        // ���� ���ϸ�Ƽ�� ���ӿ�����Ʈ�� ��Ȱ��ȭ ���¶��
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
        // �����ġ / �ִ��ġ
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
//        /// �Ʒ� �޼��忡 ���� �ҽ��ڵ�� ���� ������� ����մϴ�.
//        /// </summary>
//        private void Update()
//        {
//            MoveTest();

//            // IsAiming ����� �׽�Ʈ �ڵ�
//            _status.IsAiming.Value = Input.GetKey(KeyCode.Mouse1);
//        }

//        public void MoveTest()
//        {
//            // (ȸ�� ���� ��) �¿� ȸ���� ���� ���� ��ȯ
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

//            // �������ػ��¶�� �÷��̾ ī�޶� ���� �������� ȸ��
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

