using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern
{
    // T�� MonoBehaviour�� ��ӹ��� Ÿ�Ը� ���
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // ���� �����ϴ� ���׸�Ÿ���� ������Ʈ�� ã�� �Ҵ�
                    _instance = FindObjectOfType<T>();
                    DontDestroyOnLoad(_instance);
                }
                return _instance;
            }
        }

        protected void SingletonInit()
        {
            // �̹� �ν��Ͻ��� �����ϰ�, �װ� �ڽ��� �ƴ϶��
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                //_instance = GetComponent<T>();
                // �����ϰ� �ٿ�ĳ����
                _instance = this as T;
                DontDestroyOnLoad(_instance);
            }
        }

    }
}



