using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReferenceRegistry
{
    private static Dictionary<GameObject, ReferenceProvider> _providers { get; set; } = new();

    public static void Register(ReferenceProvider referenceProvider)
    {
        if (_providers.ContainsKey(referenceProvider.gameObject)) return;

        _providers.Add(referenceProvider.gameObject, referenceProvider);
        
    }

    public static void Unregister(ReferenceProvider referenceProvider)
    {
        if (!_providers.ContainsKey(referenceProvider.gameObject)) return;

        _providers.Remove(referenceProvider.gameObject);
    }

    // ���� �Ѿ�� ����ִ� ����
    public static void Clear()
    {
        _providers.Clear();
    }

    // ���ӿ�����Ʈ�� �Ű������� �ް� ReferenceProviderŬ������ ��ȯ
    public static ReferenceProvider GetProvider(GameObject gameObject)
    {
        if (!_providers.ContainsKey(gameObject)) return null;

        // _providers��ųʸ��� ���ӿ�����ƮŰ���� value���� ReferenceProviderŬ������ ��ȯ
        return _providers[gameObject];
    }

}
