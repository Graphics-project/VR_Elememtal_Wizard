using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public GameObject[] status;
    public GameObject[] skill;

    public int selectedCharacter = 0;

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        status[selectedCharacter].SetActive(false);
        skill[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);
        status[selectedCharacter].SetActive(true);
        skill[selectedCharacter].SetActive(true);
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        status[selectedCharacter].SetActive(false);
        skill[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter - 1 + characters.Length) % characters.Length;
        characters[selectedCharacter].SetActive(true);
        status[selectedCharacter].SetActive(true);
        skill[selectedCharacter].SetActive(true);
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Demo_01");
    }
}
