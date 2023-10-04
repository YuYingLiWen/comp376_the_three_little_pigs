using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public partial class GameObjectPooler : MonoBehaviour
{
    private GameObjectPooler instance = null;

    private ObjectPool<GameObject> poolObject1;

    private void Awake()
    {
        if(!instance) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        poolObject1 = new ObjectPool<GameObject>(OnCreateObj1, OnGetObj1, OnReleaseObj1,OnDestroyObj1,true,90) ;

    }
}

public partial class GameObjectPooler : MonoBehaviour
{
    ///
    GameObject OnCreateObj1()
    {
        return new GameObject();
    }

    void OnGetObj1(GameObject obj)
    {
    }

    void OnReleaseObj1(GameObject obj)
    {
    }

    void OnDestroyObj1(GameObject obj)
    {
    }

    public ObjectPool<GameObject> GetPool1() => poolObject1;
}