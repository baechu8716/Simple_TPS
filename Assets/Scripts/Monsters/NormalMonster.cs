using DesignPattern;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;

public class NormalMonster : Monster, IDamagable
{
    private bool _IsActivateControl = true;
    private bool _canTracking = true;

    [SerializeField] private int maxHp;
    // 움직이는 애니메이션 추가시 
    private ObservableProperty<bool> IsMoving = new();
    private ObservableProperty<int> CurrentHp;
    private ObservableProperty<bool> IsAttacking = new();

    [Header("Confing Navmesh")]
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _targetTransform;

    private void Awake()
    {
        Init();    
    }

    private void Update()
    {
        HandleControl();    
    }

    private void Init()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.isStopped = true;
    }

    private void HandleControl()
    {
        if (!_IsActivateControl) return;

        HandleMove();

    }

    private void HandleMove()
    {
        if (_targetTransform == null) return;

        if (_canTracking)
        {
            // 정지중인지 확인 만약 정지중라면 false로 바꿈
            _navMeshAgent.SetDestination(_targetTransform.position);
            // IsMoving.Value = true;
        }

        _navMeshAgent.isStopped = !_canTracking;
        IsMoving.Value = _canTracking;
    }

    public void TakeDamage(int value)
    {
        Debug.Log($"{gameObject.name} : {value}데미지 입음");
    }
}
