using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private const float spawnRadius = 80f;
    [SerializeField] private GameObject enemy;
    [SerializeField] private int spawnCount;
    
    private float RandomInRadius
    {
        get
        {
            return Random.Range(-spawnRadius, spawnRadius);
        }
    }

    private void Start()
    {
        for(int i = 0; i < spawnCount; i++) { 
            Spawn();
        }
    }

    private void Spawn()
    {
        Vector3 spawnPos = new Vector3(RandomInRadius + transform.position.x, transform.position.y, RandomInRadius + transform.position.z);
        GameObject spawned = Instantiate(enemy, spawnPos, Quaternion.identity);

        spawned.GetComponent<MainEnemy>().spawner = this;
    }

    private IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(3f);

        Spawn();
        Debug.Log("spawned");
    }

    public void NotifyDeath()
    {
        StartCoroutine(SpawnDelay());
    }
}
