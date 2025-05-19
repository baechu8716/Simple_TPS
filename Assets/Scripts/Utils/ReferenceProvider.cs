using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �� ������ getComponent�� �޴� �� �ذ�
[DisallowMultipleComponent] // ������Ʈ �ߺ� ���� ����
public class ReferenceProvider : MonoBehaviour
{
    // ������Ʈ ����
    [SerializeField] private Component _component;

    private void Awake()
    {
        ReferenceRegistry.Register(this);
    }

    private void OnDestroy()
    {
        ReferenceRegistry.Unregister(this);
    }

    // GetComponetó�� ���׸����� ������ ������Ʈ�� ��ӹ޾ƾ� �Ѵ�.
    public T GetAs<T>() where T : Component
    {
        // ����ó�� �ʿ�
        return _component as T;
    }
}
