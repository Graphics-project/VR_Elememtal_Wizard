using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillControl : MonoBehaviour
{
    public GameObject[] hideSkillButtons;
    public Image[] hideSkillImages;
    public GameObject[] textPros;
    public TextMeshProUGUI[] hideSkillTimeTexts;


    private float[] skillTimes = { 1, 6, 10, 15 };

    private bool[] isHideSkills = { false, false, false, false };
    private float[] getSkillTimes = { 0, 0, 0, 0 };

    void Start()
    {
        for (int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<TextMeshProUGUI>();
            hideSkillButtons[i].SetActive(false);
        }
    }

    void Update()
    {
        HideSkillChk();
    }



    public void SetSkillCooldown(int skillNum, float cooldownTime)
    {
        getSkillTimes[skillNum] = cooldownTime;
    }


    public void HideSkillSetting(int skillNum)
    {
        hideSkillButtons[skillNum].SetActive(true);
        getSkillTimes[skillNum] = skillTimes[skillNum];
        isHideSkills[skillNum] = true;
    }

    private void HideSkillChk()
    {
        for (int i = 0; i < isHideSkills.Length; i++)
        {
            if (isHideSkills[i])
            {
                if (getSkillTimes[i] > 0)
                {
                    getSkillTimes[i] -= Time.deltaTime;

                    if (getSkillTimes[i] < 0)
                    {
                        getSkillTimes[i] = 0;
                        isHideSkills[i] = false;
                        hideSkillButtons[i].SetActive(false);
                    }

                    hideSkillTimeTexts[i].text = getSkillTimes[i].ToString("0.00");

                    float time = getSkillTimes[i] / skillTimes[i];
                    hideSkillImages[i].fillAmount = time;
                }
            }
        }
    }
}
