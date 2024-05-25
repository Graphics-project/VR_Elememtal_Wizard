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


    




    // skill variables 
    private Vector3 meteor_startPos_offset = new Vector3(0, 30, 0);
    private Quaternion meteor_rotation_offset =  Quaternion.Euler(60, 0, 0);


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

            }
            else if (elementType == 1)
            {

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
    //  [2nd skill]
    //  SecondSkill()
    // -------------------------------------------
    void SpawnMeteor()
    {
        Vector3 startPos = player.transform.TransformPoint(meteor_startPos_offset);
        Vector3 forwardDirection = player.transform.forward;
        forwardDirection.y = 0;
        forwardDirection.Normalize(); 

        Quaternion meteorAngle = Quaternion.LookRotation(forwardDirection) * meteor_rotation_offset;
        GameObject objVFX = Instantiate(skillToSpawn, startPos, meteorAngle);

    }


    // -------------------------------------------
    //  spawnProjectile()
    // -------------------------------------------
    




}
