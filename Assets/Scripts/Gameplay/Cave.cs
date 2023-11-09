using System.Collections;
using UnityEngine;

//Spawns wolf

public class Cave : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);

            GameObject wolf = WolfPooler.Instance.Pool.Get();
            wolf.transform.position = transform.position;
        }
    }
}
