using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody BulletRigidbody;

    public int deal;
    public float destroyTime = 3f;


    void Start()
    {
        BulletRigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyAI target = other.GetComponent<EnemyAI>();
        Player player = GameManager.instance.player.GetComponent<Player>();

        if (gameObject.tag == "EnemyProjectile" && other.tag == "Player")
        {
            player.TakeDamage(deal);
        }

        if (gameObject.tag == "PlayerProjectile" && (other.tag == "Ghost" || other.tag == "Golem" || other.tag == "Mummy"))
        {
            target.TakeDamage(deal);
        }
        else if (gameObject.tag == "SlowProjectile" && (other.tag == "Ghost" || other.tag == "Golem" || other.tag == "Mummy"))
        {
            target.TakeDamage(deal);
            target.SlowDown();
        }
        else if (gameObject.tag == "StunProjectile" && (other.tag == "Ghost" || other.tag == "Golem" || other.tag == "Mummy"))
        {
            target.TakeDamage(deal);
            target.Stunned();
        }

    }
}
