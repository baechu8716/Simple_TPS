using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 총을 쏠 때마다 getComponent로 받는 것 해결
[DisallowMultipleComponent] // 컴포넌트 중복 적용 방지
public class ReferenceProvider : MonoBehaviour
{
    // 컴포넌트 제공
    [SerializeField] private Component _component;

    private void Awake()
    {
        ReferenceRegistry.Register(this);
    }

    private void OnDestroy()
    {
        ReferenceRegistry.Unregister(this);
    }

    // GetComponet처럼 제네릭으로 받지만 컴포넌트를 상속받아야 한다.
    public T GetAs<T>() where T : Component
    {
        // 예외처리 필요
        return _component as T;
    }
}
