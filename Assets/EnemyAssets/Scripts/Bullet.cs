using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody BulletRigidbody;
    private Collider bulletCollider;
    // Start is called before the first frame update

    void Awake()
    {
        bulletCollider = GetComponent<Collider>();
        bulletCollider.enabled = false;
        Invoke("EnableCollider", 0.5f);
    }
    void EnableCollider()
    {
        bulletCollider.enabled = true;
    }
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
