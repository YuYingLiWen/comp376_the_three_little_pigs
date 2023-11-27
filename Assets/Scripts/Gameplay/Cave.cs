using System.Collections;
using UnityEngine;

//Spawns wolf

public class Cave : MonoBehaviour
{
    public void Spawn()
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

            // orient the wolf in the direction he's going
            Vector3 direction = new Vector3(0,0,0) - wolf.transform.position; // 0,0,0 is where the house always will be
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            wolf.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
    }
}
