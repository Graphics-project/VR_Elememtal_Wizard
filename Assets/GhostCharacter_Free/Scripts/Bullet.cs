using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody BulletRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        BulletRigidbody = GetComponent<Rigidbody>();

        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
