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
        pool = GameManager.instance.pool.transform;
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
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        GameManager.instance.incCurrentEnemyNum();
    }
}
