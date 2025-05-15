using DesignPattern;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    [SerializeField] private PooledObjectTest _prefab;
    private ObjectPool _pool;
    private PooledObject _temp;

    

    //private void Awake()
    //{
    //    _pool = new ObjectPool<AudioManager>(_audioManagerPrefab, 10);
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _temp = _pool.PopPool();
            _temp.transform.parent = transform;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _temp.ReturnPool();
        }
    }
}
