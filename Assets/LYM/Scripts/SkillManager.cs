using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject firePoint;
    public GameObject player;

    // skills
    public List<GameObject> fireSkills = new List<GameObject>();
    public List<GameObject> iceSkills = new List<GameObject>();
    public List<GameObject> earthSkills = new List<GameObject>();



    // 0: fire
    // 1: ice
    // 2: earth
    private int elementType = 0;


    private float timeTofire = 0;

    private GameObject skillToSpawn;
    private List<GameObject> currentSkills = new List<GameObject>();
    private bool skillSign = false;

    private int skillNum = 0;


    




    // skill variables (offset)

    // Fire
    private Vector3 fireBreath_startPos_offset = new Vector3(0, 0, 5);
    private Quaternion fireBreath_rotation_offset = Quaternion.identity;

    private Vector3 fireTornado_startPos_offset = new Vector3(0, 0, 10);
    private Quaternion fireTornado_rotation_offset = Quaternion.Euler(0, -90, 0);

    private Vector3 meteor_startPos_offset = new Vector3(0, 30, 0);
    private Quaternion meteor_rotation_offset = Quaternion.Euler(60, 0, 0);



    // Ice
    private Vector3 iceLance_startPos_offset = new Vector3(0, 10, 0);
    private Quaternion iceLance_rotation_offset = Quaternion.Euler(45, 0, 0);


    // Earth



    // Start is called before the first frame update
    void Start()
    {
        SetElementSkillType(elementType);
    }

    // Update is called once per frame
    void Update()
    {
        skillSelect();

        if (skillSign && Time.time >= timeTofire)
        {
            skillUse();
        }
        skillSign = false;
    }


    // ===========================================
    //   Functions
    // ===========================================

    // -------------------------------------------
    //  SetElementSkillType()
    // -------------------------------------------

    void SetElementSkillType(int elementType)
    {
        if (elementType == 0)
        {
            currentSkills = fireSkills;
        }
        else if (elementType == 1)
        {
            currentSkills = iceSkills;
        }
        else if (elementType == 2)
        {
            currentSkills = earthSkills;
        }

    }




    // -------------------------------------------
    //  skillSelect()
    // -------------------------------------------
    void skillSelect()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            skillNum = 0;
            skillSign = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            skillNum = 1;
            skillSign = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            skillNum = 2;
            skillSign = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            skillNum = 3;
            skillSign = true;
        }

        skillToSpawn = currentSkills[skillNum];
    }

    // -------------------------------------------
    //  skillUse()
    // -------------------------------------------
    void skillUse()
    {

        // Basic Skill(skill 1)
        if (skillNum == 0)
        {
            BasicSkill();
        }

        // Epic SKill(skill 2)
        if (skillNum == 1)
        {
            if (elementType == 0)
            {
                // ice lance
                SpawnSimpleSkills(fireBreath_startPos_offset, fireBreath_rotation_offset);
            }
            else if (elementType == 1)
            {
                // ice lance
                SpawnSimpleSkills(iceLance_startPos_offset, iceLance_rotation_offset);
            }
            else if (elementType == 2)
            {

            }
        } 

        // Ultimate Skill(skill 3)
        if (skillNum == 2)
        {
            if (elementType == 0)
            {
                // Fire Tornado
                SpawnSimpleSkills(fireTornado_startPos_offset, fireTornado_rotation_offset);
            }
            else if (elementType == 1)
            {

            }
            else if (elementType == 2)
            {

            }
        } 

        if (skillNum == 3)
        {
            if (elementType == 0)
            {
                // Meteor
                SpawnSimpleSkills(meteor_startPos_offset, meteor_rotation_offset);
            }
            else if (elementType == 1)
            {

            }
            else if (elementType == 2)
            {

            }
        }
    }



    // -------------------------------------------
    //  BasicSkill()
    // -------------------------------------------
    void BasicSkill()
    {
        if (firePoint != null)
        {
            timeTofire = Time.time + 1 / skillToSpawn.GetComponent<ProjectileMove>().fireRate;
            Instantiate(skillToSpawn, firePoint.transform.position, firePoint.transform.rotation);
        }
        else
        {
            Debug.Log("No Fire Point");
        }
    }

    // -------------------------------------------
    //  SpawnSimpleSkills()
    // -------------------------------------------
    void SpawnSimpleSkills(Vector3 startPos_offset, Quaternion startRotate_offest)
    {
        Vector3 startPos = player.transform.TransformPoint(startPos_offset);
        Vector3 forwardDirection = player.transform.forward;
        forwardDirection.y = 0;
        forwardDirection.Normalize(); 

        Quaternion startRotate = Quaternion.LookRotation(forwardDirection) * startRotate_offest;
        Instantiate(skillToSpawn, startPos, startRotate);
    }



    // -------------------------------------------
    //  SpawnSimpleSkills2()
    // -------------------------------------------
    //void SpawnSimpleSkills2(Vector3 startPos_offset, Quaternion startRotate_offest)
    //{
    //    Vector3 startPos = player.transform.TransformPoint(startPos_offset);
    //    Quaternion startRotate = startRotate_offest;

    //    Instantiate(skillToSpawn, startPos, startRotate);
    //}



}
