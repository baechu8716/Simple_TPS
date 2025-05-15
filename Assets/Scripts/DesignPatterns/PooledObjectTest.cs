using DesignPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObjectTest : PooledObject
{
    public float time = 3;
    public float current = 0;

    private void OnEnable()
    {
        current = time;
    }
    private void Update()
    {
        if (current >= 0)
        {
            current -= Time.deltaTime;
        }
        else
        {
            ReturnPool();
        }
    }
}
