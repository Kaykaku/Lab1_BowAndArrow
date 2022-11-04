using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawn : MonoBehaviour
{

    public GameObject[] TargetPrefabs;

    public float spawnMinX = -2f;
    public float spawnMaxX = 12f;
    public float spawnPosY = 5f;
    public float startDelay = 2f;
    public float spawnInterval = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("Target") == null)
        {
            SpawnRandomTarget();
        }
    }

    //Spawn a Target that generates a random location within X range
    void SpawnRandomTarget()
    {
        int targetIndex = Random.Range(0, TargetPrefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range(spawnMinX, spawnMaxX), spawnPosY, 0);
        GameObject prefab= Instantiate(TargetPrefabs[targetIndex], spawnPos, TargetPrefabs[targetIndex].transform.rotation);
        prefab.transform.parent = transform;
    }
}



