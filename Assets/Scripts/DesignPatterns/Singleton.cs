using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern
{
    // T는 MonoBehaviour를 상속받은 타입만 허용
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // 씬에 존재하는 제네릭타입의 오브젝트를 찾아 할당
                    _instance = FindObjectOfType<T>();
                    DontDestroyOnLoad(_instance);
                }
                return _instance;
            }
        }

        protected void SingletonInit()
        {
            // 이미 인스턴스가 존재하고, 그게 자신이 아니라면
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                //_instance = GetComponent<T>();
                // 안전하게 다운캐스팅
                _instance = this as T;
                DontDestroyOnLoad(_instance);
            }
        }

    }
}



