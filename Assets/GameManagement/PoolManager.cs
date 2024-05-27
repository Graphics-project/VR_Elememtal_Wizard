using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;
    Spawner spawner;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        for (int i=0; i<pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
        spawner = GameManager.instance.spawner;
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        //foreach (GameObject item in pools[index])
        //{
        //    if (!item.activeSelf)
        //    {
        //        select = item;
        //        select.SetActive(true);
        //        break;
        //    }
        //}

        if (!select)
        {
            select = Instantiate(prefabs[index], spawner.GetSpawnPosition(), Quaternion.identity);
            pools[index].Add(select);
        }


        return select;
    }

    public void DestroyAllChildren()
    {
        int childCount = transform.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Destroy(child);
            GameManager.instance.setCurrentEnemyNumZero();
        }

    }
}
