using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.XR;

using Unity.XR.CoreUtils;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public Spawner spawner;
    public PoolManager pool;

    public int level = 0;   // todo : change to private
    public int currentEnemyNum = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public int getCurrentEnemyNum()
    {
        return currentEnemyNum;
    }
    public void incCurrentEnemyNum()
    {
        currentEnemyNum++;
    }
    public void decCurrentEnemyNum()
    {
        currentEnemyNum--;
    }


}
