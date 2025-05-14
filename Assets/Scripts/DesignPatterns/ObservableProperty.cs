using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObservableProperty<T>
{
    private T _value;
    public T Value
    {
        get => _value;
        // 기존 값과 같다면 return
        set
        {
            if (_value.Equals(value)) return;
            _value = value;
            Notify();
        }
    }

    private UnityEvent<T> _onValueChanged = new();
    public ObservableProperty(T value = default)
    {
        _value = value;
    }

    public void Subscribe(UnityAction<T> action)
    {
        _onValueChanged.AddListener(action);
    }

    public void Unsubscribe(UnityAction<T> action)
    {
        _onValueChanged.RemoveListener(action);
    }

    public void UnSubscribeAll()
    {
        _onValueChanged.RemoveAllListeners();
    }

    // 이벤트 실행을 알림
    public void Notify()
    {
        _onValueChanged?.Invoke(Value);
    }
}
