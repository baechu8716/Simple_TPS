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

    // 씬을 넘어갈때 비워주는 역할
    public static void Clear()
    {
        _providers.Clear();
    }

    // 게임오브젝트를 매개변수로 받고 ReferenceProvider클래스를 반환
    public static ReferenceProvider GetProvider(GameObject gameObject)
    {
        if (!_providers.ContainsKey(gameObject)) return null;

        // _providers딕셔너리의 게임오브젝트키값의 value값인 ReferenceProvider클래스를 반환
        return _providers[gameObject];
    }

}
