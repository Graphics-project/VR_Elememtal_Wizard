using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    public GameObject[] skills; // Inspector에서 할당
    public GameObject status;
    public GameObject stopMenu;
    public GameObject gameOverUI;

    void Start()
    {
        stopMenu.SetActive(false);
        gameOverUI.SetActive(false);
        status.SetActive(true);

        // PlayerPrefs에서 선택된 캐릭터의 인덱스를 불러옴
        int selectedCharacterIndex = PlayerPrefs.GetInt("selectedCharacter", 0); // 기본값으로 0을 사용

        foreach (var skill in skills)
        {
            skill.SetActive(false);
        }

        if (skills.Length > selectedCharacterIndex)
        {
            skills[selectedCharacterIndex].SetActive(true);
        }
        else
        {
            Debug.LogError("선택된 캐릭터 인덱스가 배열 범위를 벗어났습니다.");
        }
    }

    public void ToggleStopMenu()
    {
        status.SetActive(!status.activeSelf);
        stopMenu.SetActive(!stopMenu.activeSelf);
    }

    public void setGameOverUI()
    {
        status.SetActive(false);
        stopMenu.SetActive(false);
        gameOverUI.SetActive(true);
    }
}