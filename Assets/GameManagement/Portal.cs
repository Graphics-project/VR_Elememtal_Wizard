using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform targetLocation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = targetLocation.position;
           
        }
    }
}
