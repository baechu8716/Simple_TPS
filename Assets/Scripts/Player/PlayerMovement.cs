using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _avatar;
    [SerializeField] private Transform _aim;

    private Rigidbody _rigid;
    private PlayerStatus _playerStatus;

    [Header("Mouse Config")]
    // 마우스 최소각도
    [SerializeField][Range(-90, 0)] private float _minPitch;
    // 마우스 최대각도
    [SerializeField][Range(0, 90)] private float _maxPitch;
    // 마우스 감도
    [SerializeField][Range(0, 5)] private float _mouseSensitivity = 1;

    private Vector2 _currentRotation;
    public Vector2 InputDirection { get; private set; }
    public Vector2 MouseDirection { get; private set; }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _rigid = GetComponent<Rigidbody>();
        _playerStatus = GetComponent<PlayerStatus>();
    }

    public Vector3 SetMove(float moveSpeed)
    {
        Vector3 moveDirection = GetMoveDirection();

        Vector3 velocity = _rigid.velocity;
        velocity.x = moveDirection.x * moveSpeed;
        velocity.z = moveDirection.z * moveSpeed;

        _rigid.velocity = velocity;

        return moveDirection;
    }

    public Vector3 SetAimRotation()
    {
        // 마우스 방향 
        //Vector3 mouseDir = GetMoveDirection();
        //// 마우스 회전 값 설정
        _currentRotation.x += MouseDirection.x; // 마우스 x축의 경우라면 제한x
        // 마우스 y축의 경우엔 각도 제한 o
        _currentRotation.y = Mathf.Clamp
            (
            _currentRotation.y + MouseDirection.y, 
            _minPitch, 
            _maxPitch
            );

        // 캐릭터 오브젝트의 경우에는 Y축 회전만 반영
        transform.rotation = Quaternion.Euler
            (
            0,
            _currentRotation.x,
            0
            );
        // 에임의 경우 상하 회전 반영
        Vector3 currentEuler = _aim.localEulerAngles;
        _aim.localEulerAngles = new Vector3(_currentRotation.y, currentEuler.y, currentEuler.z);

        // 회전 방향 벡터 반환
        Vector3 rotateDirVector = transform.forward;
        rotateDirVector.y = 0;
        return rotateDirVector.normalized;
    }

    public void SetAvatarRotation(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        _avatar.rotation = Quaternion.Lerp(_avatar.rotation, targetRotation, _playerStatus.RotateSpeed * Time.deltaTime);

    }
    

    public Vector3 GetMoveDirection()
    {
        //Vector2 input = InputDirection;

        Vector3 direction = (transform.right * InputDirection.x) +
            (transform.forward * InputDirection.y);

        return direction.normalized;
    }



    public void OnMove(InputValue value)
    {
        InputDirection = value.Get<Vector2>();

    }

    public void OnRotate(InputValue value)
    {
        Vector2 mouseDir = value.Get<Vector2>();
        mouseDir.y *= -1;
        MouseDirection = mouseDir * _mouseSensitivity;
         
    }

    //public Vector3 GetInputDirection()
    //{
    //    float x = Input.GetAxisRaw("Horizontal");
    //    float z = Input.GetAxisRaw("Vertical");

    //    return new Vector3(x, 0, z);
    //}

    //private Vector2 GetMouseDirection()
    //{
    //    float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
    //    float mouseY = -Input.GetAxis("Mouse Y") * _mouseSensitivity;

    //    return new Vector2(mouseX, mouseY);
    //}
}
