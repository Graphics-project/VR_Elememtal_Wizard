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
    private Vector3 meteor_startPos_offset = new Vector3(0, 20, 0);
    private Vector3 meteor_lastPos_offset = new Vector3(0, -2, 10);



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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            skillNum = 0;
            skillSign = true;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            skillNum = 1;
            skillSign = true;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            skillNum = 2;
            skillSign = true;
        }
        else if (Input.GetKeyDown(KeyCode.R))
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
            EpicSkill();
        }

        // Ultimate Skill(skill 3)
        if (elementType == 2)
        {

        }

    }



    // -------------------------------------------
    //  [1st skill]
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
    //  EpicSkill()
    // -------------------------------------------
    void EpicSkill()
    {
        if (elementType == 0)
        {
            Vector3 startPos = player.transform.TransformPoint(meteor_startPos_offset);

            GameObject objVFX = Instantiate(skillToSpawn, startPos, Quaternion.identity);

            Vector3 endPos = player.transform.TransformPoint(meteor_lastPos_offset);

            RotateTo(objVFX, endPos);
        }
    }


    // -------------------------------------------
    //  [3rd skill]
    //  spawnProjectile()
    // -------------------------------------------
    



    void RotateTo(GameObject obj, Vector3 destination)
    {
        var direction = destination - obj.transform.position;
        var rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }
}
