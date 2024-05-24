using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody BulletRigidbody;
    private Collider bulletCollider;

    public int deal;

    void Awake()
    {
        deal = 1;
    }

    void Start()
    {
        BulletRigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyAI target = other.GetComponent<EnemyAI>();
        //PlayerController player = other.GetComponent<PlayerController>();

        if (gameObject.tag == "EnemyProjectile" && other.tag == "player")
        {
            //player.setDamageState();
        }

        if (gameObject.tag == "PlayerProjectile" && (other.tag == "Ghost" || other.tag == "Golem" || other.tag == "Mummy"))
        {
            target.TakeDamage(deal);
        }
        else if (gameObject.tag == "SlowDownProjectile" && (other.tag == "Ghost" || other.tag == "Golem" || other.tag == "Mummy"))
        {
            target.TakeDamage(deal);
            target.SlowDown();
        }

    }
}
