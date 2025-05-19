using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController_KJS : MonoBehaviour
{
    public bool IsControlActivate { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;

    [SerializeField] private CinemachineVirtualCamera _aimCamera;

    [SerializeField] private KeyCode _aimKey = KeyCode.Mouse1;

    private void Awake() => Init();
    private void OnEnable() => SubscribeEvents();
    //private void Update() => HandlePlayerControl();
    private void OnDisable() => UnsubscribeEvents();

    /// <summary>
    /// 초기화용 함수, 객체 생성시 필요한 초기화 작업이 있다면 여기서 수행한다.
    /// </summary>
    private void Init()
    {
        _status = GetComponent<PlayerStatus>();
        _movement = GetComponent<PlayerMovement>();
        // _mainCamera = Camera.main.gameObject;
    }

    //private void HandlePlayerControl()
    //{
    //    if (!IsControlActivate) return; 

    //    HandleMovement();
    //    HandleAiming();
    //}

    //private void HandleMovement()
    //{
    //    Vector3 camRotateDir = _movement.SetAimRotation();

    //    float moveSpeed;
    //    if (_status.IsAiming.Value) moveSpeed = _status.WalkSpeed;
    //    else moveSpeed = _status.RunSpeed;

    //    Vector3 moveDir = _movement.SetMove(moveSpeed);
    //    _status.IsMoving.Value = (moveDir != Vector3.zero);

    //    Vector3 avatarDir;
    //    if (_status.IsAiming.Value) avatarDir = camRotateDir;
    //    else avatarDir = moveDir;

    //    _movement.SetAvatarRotation(avatarDir);
    //}

    private void HandleAiming()
    {
        _status.IsAiming.Value = Input.GetKey(_aimKey);
    }

    public void SubscribeEvents()
    {
        _status.IsAiming.Subscribe(_aimCamera.gameObject.SetActive);
    }

    public void UnsubscribeEvents()
    {
        _status.IsAiming.Unsubscribe(_aimCamera.gameObject.SetActive);
    }
}














