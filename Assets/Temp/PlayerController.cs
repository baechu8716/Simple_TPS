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
        /// �Ʒ� �޼��忡 ���� �ҽ��ڵ�� ���� ������� ����մϴ�.
        /// </summary>
        private void Update()
        {
            MoveTest();

            // IsAiming ����� �׽�Ʈ �ڵ�
            _status.IsAiming.Value = Input.GetKey(KeyCode.Mouse1);
        }

        public void MoveTest()
        {
            // (ȸ�� ���� ��) �¿� ȸ���� ���� ���� ��ȯ
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

            // TODO : ��ü�� ȸ�� ����
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

