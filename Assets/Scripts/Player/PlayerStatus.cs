using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;

public class PlayerStatus : MonoBehaviour
{
    [field: SerializeField][field: Range(0, 10)]
    public float WalkSpeed { get; set; }

    [field: SerializeField][field: Range(0, 10)]
    public float RunSpeed { get; set; }

    [field: SerializeField][field: Range(0, 10)]
    public float RotateSpeed { get; set; }

    [field: SerializeField][field: Range(0, 500)]
    public int MaxHP { get; set; }

    // Player state Event -------------
    public ObservableProperty<bool> IsAiming { get; private set; } = new();
    public ObservableProperty<bool> IsMoving { get; private set; } = new();
    public ObservableProperty<bool> IsAttacking { get; private set; } = new();
    // Player stat Event --------------
    public ObservableProperty<int> CurrentHP { get; private set; } = new();
}
