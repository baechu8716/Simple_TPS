using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsControlActivate { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;
    private Animator _animator;

    [SerializeField] private CinemachineVirtualCamera _aimCamera;
    [SerializeField] private Gun _gun;
    [SerializeField] private KeyCode _aimKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode _shootKey = KeyCode.Mouse0;
   
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
        // _mainCamera = Camera.main.gameObject;
    }
    
    private void HandlePlayerControl()
    {
        if (!IsControlActivate) return;

        HandleMovement();
        HandleAiming();
        HandleShooting();
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
        // �̺�Ʈ 
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

    private void SubscribeEvents()
    {
        _status.IsMoving.Subscribe(SetMoveAnimaion);

        _status.IsAiming.Subscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Subscribe(SetAimAnimation);

        _status.IsAttacking.Subscribe(SetAttackAnimation);
    }

    private void UnSubscribeEvents()
    {
        _status.IsMoving.Unsubscribe(SetMoveAnimaion);

        _status.IsAiming.Unsubscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Unsubscribe(SetAimAnimation);

        _status.IsAttacking.Unsubscribe(SetAttackAnimation);
    }

    private void SetAimAnimation(bool value) => _animator.SetBool("IsAim", value);

    private void SetMoveAnimaion(bool value) => _animator.SetBool("IsMove", value);

    private void SetAttackAnimation(bool value) => _animator.SetBool("IsAttack", value);
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

