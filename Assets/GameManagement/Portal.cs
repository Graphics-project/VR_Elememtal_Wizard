using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform targetLocation;
    public int levelToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = targetLocation.position;
            GameManager.instance.setLevel(levelToLoad);
        }
    }
}
