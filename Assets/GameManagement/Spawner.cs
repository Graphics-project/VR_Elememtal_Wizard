using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    Transform pool;
    
    int enemyNum;
    int enemyMax;
    int level;
    float timer;
    float spawnTime;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        //pool = GameManager.instance.pool.transform;
        if (GameManager.instance != null)
        {
            pool = GameManager.instance.pool.transform;
        }
        else
        {
            Debug.LogError("GameManager instance is not set!");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        level = GameManager.instance.level;
        enemyNum = GameManager.instance.getCurrentEnemyNum();

        if (level == 1)
        {
            spawnTime = 1f;
            enemyMax = 30;
        }
        else if (level == 2)
        {
            spawnTime = 0.5f;
            enemyMax = 40;
        }

        if (timer > spawnTime && enemyMax > enemyNum)
        {
            Spawn();
            timer = 0;
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(Random.Range(0, 2));
        if (level == 1)
        {
            enemy.transform.position = spawnPoint[Random.Range(1, 5)].position - gameObject.transform.position;
        }
        else if (level == 2)
        {
            enemy.transform.position = spawnPoint[Random.Range(1, 5)].position;
        }
        //enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

        //if (level == 1)
        //{
        //    int num = Random.Range(1, 6);
        //    if (num < spawnPoints.Length)
        //    {
        //        Vector3 spawnPosition = spawnPoints[num].position;
        //        if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        //        {
        //            enemy.transform.position = hit.position;
        //            agent.enabled = true; // Ensure NavMeshAgent is enabled after setting position
        //        }
        //        else
        //        {
        //            Debug.LogError("Failed to find a valid NavMesh position for level 1");
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogError("Spawn point index out of range: " + num);
        //    }
        //}
        //else if (level == 2)
        //{
        //    int num = Random.Range(6, 10);
        //    if (num < spawnPoints.Length)
        //    {
        //        Vector3 spawnPosition = spawnPoints[num].position;
        //        if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        //        {
        //            enemy.transform.position = hit.position;
        //            agent.enabled = true; // Ensure NavMeshAgent is enabled after setting position
        //        }
        //        else
        //        {
        //            Debug.LogError("Failed to find a valid NavMesh position for level 2");
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogError("Spawn point index out of range: " + num);
        //    }
        //}
        GameManager.instance.incCurrentEnemyNum();
    }
}
