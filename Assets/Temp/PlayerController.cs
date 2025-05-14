using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace YTW_Test
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerMovement _movement;
        public PlayerStatus _status;

        /// <summary>
        /// 아래 메서드에 적힌 소스코드와 같은 방식으로 사용합니다.
        /// </summary>
        private void Update()
        {
            MoveTest();

            // IsAiming 변경용 테스트 코드
            _status.IsAiming.Value = Input.GetKey(KeyCode.Mouse1);
        }

        public void MoveTest()
        {
            // (회전 수행 후) 좌우 회전에 대한 벡터 반환
            Vector3 camRotateDir = _movement.SetAimRotation();

            float moveSpeed;
            if (_status.IsAiming.Value == true)
            {
                moveSpeed = _status.WalkSpeed;
            }
            else
            {
                moveSpeed = _status.RunSpeed;
            }

            Vector3 moveDir = _movement.SetMove(moveSpeed);
            _status.IsMoving.Value = (moveDir != Vector3.zero);

            // TODO : 몸체의 회전 구현
            Vector3 avatarDir;
            if(_status.IsAiming.Value)
            {
                avatarDir = camRotateDir;
            }
            else
            {
                avatarDir = moveDir;
            }

            _movement.SetAvatarRotation(avatarDir);
        }
    }
}

