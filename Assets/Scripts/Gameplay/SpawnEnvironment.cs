using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnvironment : MonoBehaviour
{
    int numOfTrees = 30;
    int numOfCaves = 3; // wolf caves
    const int numOfStoneMines = 2;

    float halfMapSize; // works as long as map is square

    public GameObject cavePrefab;
    public GameObject stoneMine1Prefab;
    public GameObject stoneMine2Prefab;
    public GameObject tree1Prefab;
    public GameObject tree2Prefab;
    public GameObject tree3Prefab;


    private void Start()
    {
        // Getting the size of the map
        GameObject map = GameObject.FindWithTag("Ground");
        if (map != null)
        {
            Renderer planeRenderer = map.GetComponent<Renderer>();
            if (planeRenderer != null)
            {
                Bounds bounds = planeRenderer.bounds;

                Vector2 mapExtents = bounds.extents; // half of the size of the map
                halfMapSize = mapExtents.x;
            }
            else
            {
                Debug.Log("Error: the plane (map) doesn't have a renderer component that indicates its size");
            }
        }
        else
        {
            Debug.Log("Error: No object with tag Ground (the map) was found to initialize camera bounds");
        }

        Debug.Log("Spawning environment");
        SpawnInitialEnvironment();
    }

    public void SpawnOneTree() // to be called when a tree is choped to replace it
    {
        GameObject tree;

        int treeType = Mathf.RoundToInt(Random.Range(1, 4));
        if (treeType == 1)
        {
            tree = tree1Prefab;
        }
        else if (treeType == 2)
        {
            tree = tree2Prefab;
        }
        else
        {
            tree = tree3Prefab;
        }

        Vector3 treePos = new Vector3(Random.Range(-0.85f * halfMapSize, 0.85f * halfMapSize), Random.Range(-0.85f * halfMapSize, 0.85f * halfMapSize), 0);
        Instantiate(tree, treePos, Quaternion.identity);
    }

    void SpawnInitialEnvironment() // gets called once to spawn caves, trees, mines
    {
        // Trees
        for (int i = 0; i < numOfTrees; i++)
        {
            GameObject tree;

            int treeType = Mathf.RoundToInt(Random.Range(1, 4));
            if (treeType == 1)
            {
                tree = tree1Prefab;
            } 
            else if(treeType == 2)
            {
                tree = tree2Prefab;
            } 
            else
            {
                tree = tree3Prefab;
            }

            // 0.85 because if it was 1 then trees would spawn on the edge of map which looks not cool
            Vector3 treePos = new Vector3(Random.Range(-0.85f*halfMapSize, 0.85f*halfMapSize), Random.Range(-0.85f * halfMapSize, 0.85f * halfMapSize), 0);
            Instantiate(tree, treePos, Quaternion.identity);
        }

        // Caves
        for(int i = 0; i < numOfCaves;i++)
        {
            float longitude = Random.Range(-0.9f * halfMapSize, 0.9f * halfMapSize);

            Vector3 cavePos;

            // caves are spawned on the line that is 0.9*halfMapSize
            if (i % 4 == 0) // top
            {
                cavePos = new Vector3(longitude, 0.9f * halfMapSize, 0);
            }
            else if (i % 4 == 1) // bottom
            {
                cavePos = new Vector3(longitude, -0.9f * halfMapSize, 0);
            } else if (i % 4 == 2) // right
            {
                cavePos = new Vector3(0.9f * halfMapSize, longitude, 0);
            } else // left
            {
                cavePos = new Vector3(-0.9f * halfMapSize, longitude, 0);
            }

            GameObject instantiatedCave =  Instantiate(cavePrefab, cavePos, Quaternion.identity);

            Cave cave = instantiatedCave.GetComponent<Cave>();
            LevelManager.Instance.caves.Add(cave); // so that it's considered by the level manager and used to spawn
            Debug.Log(" LevelManager.Instance.caves: " + LevelManager.Instance.caves.Count);
        }

        // Mines
        for (int i = 0; i < numOfStoneMines; i++)
        {
            float longitude = Random.Range(-0.7f * halfMapSize, 0.7f * halfMapSize);

            // caves are spawned on the line that is 0.7*halfMapSize
            if (i % 4 == 0) // right
            {
                Instantiate(stoneMine1Prefab, new Vector3(0.7f * halfMapSize, longitude, 0), Quaternion.identity);
            }
            else if (i % 4 == 1) // left
            {
                Instantiate(stoneMine2Prefab, new Vector3(-0.7f * halfMapSize, longitude, 0), Quaternion.identity);
            }
            else if (i % 4 == 2) // top
            {
                Instantiate(stoneMine1Prefab, new Vector3(longitude, 0.7f * halfMapSize, 0), Quaternion.identity);
            }
            else // bottom
            {
                Instantiate(stoneMine2Prefab, new Vector3(longitude, -0.7f * halfMapSize, 0), Quaternion.identity);
            }
        }
    }
}
