using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    
    int enemyNum;
    int enemyMax;
    int level;
    float timer;
    float spawnTime;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        level = GameManager.instance.level;
        enemyNum = transform.childCount;

        if (level == 0)
        {
            spawnTime = 1f;
            enemyMax = 5;
        }
        else if (level == 1)
        {
            spawnTime = 0.5f;
            enemyMax = 10;
        }
        else
        {
            spawnTime = 0.25f;
            enemyMax = 15;
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
    }
}
