using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public GameObject[] skills; // Inspector에서 할당

    void Start()
    {
        // PlayerPrefs에서 선택된 캐릭터의 인덱스를 불러옴
        int selectedCharacterIndex = PlayerPrefs.GetInt("selectedCharacter", 0); // 기본값으로 0을 사용

        foreach (var skill in skills)
        {
            skill.SetActive(false);
        }

        // 선택된 캐릭터만 활성화
        if (skills.Length > selectedCharacterIndex)
        {
            skills[selectedCharacterIndex].SetActive(true);
        }
        else
        {
            Debug.LogError("선택된 캐릭터 인덱스가 배열 범위를 벗어났습니다.");
        }
    }
}
