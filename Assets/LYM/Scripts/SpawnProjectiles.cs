using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{

    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject>();

    private GameObject effectToSpawn;
    private float timeTofire = 0;


    // Start is called before the first frame update
    void Start()
    {
        effectToSpawn = vfx[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton (0) && Time.time >= timeTofire) {
            timeTofire = Time.time + 1 / effectToSpawn.GetComponent<ProjectileMove>().fireRate;
            SpawnVFX();
        }
    }

    void SpawnVFX()
    {
        GameObject vfx;

        if (firePoint != null) {
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, firePoint.transform.rotation);
        }
        else {
            Debug.Log("No Fire Point");
        }
    }
}
