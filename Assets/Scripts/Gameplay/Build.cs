using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{
    public GameObject tower_blueprint_prefab;

    public void Spawn_Tower_Blueprint()
    {
        Debug.Log("inmstantiate bluwpint");
        Instantiate(tower_blueprint_prefab);
    }
}
