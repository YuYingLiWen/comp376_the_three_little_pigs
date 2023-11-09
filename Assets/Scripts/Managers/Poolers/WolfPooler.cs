using UnityEngine;
using UnityEngine.Pool;

public class WolfPooler : GameObjectPooler<GameObject>
{

    private void Awake()
    {
        if (!instance) instance = this;
        else Debug.LogWarning("Multiple " + this.GetType().Name, this);
    }

    protected override void Start()
    {
        pool = new ObjectPool<GameObject>(OnCreate, OnGetFromPool, OnRelease, OnDestruction, collectionCheck, initialAmount);
    }

    protected override GameObject OnCreate()
    {
        var newObject = Instantiate(toPool);
        newObject.transform.parent = transform;
        return newObject;
    }

    protected override void OnGetFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    protected override void OnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }

    protected override void OnDestruction(GameObject obj)
    {
        Destroy(obj);
    }

    private ObjectPool<GameObject> pool;
    public ObjectPool<GameObject> Pool => pool;

    private static WolfPooler instance;
    public static WolfPooler Instance => instance;
}