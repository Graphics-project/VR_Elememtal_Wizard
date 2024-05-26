using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.XR;

using Unity.XR.CoreUtils;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public Spawner spawner1;
    public Spawner spawner2;
    public PoolManager pool;
    public InGameUIController inGameUIController;

    public int level = 0;   // todo : change to private
    public int currentEnemyNum = 0;
    public int kills = 0;

    private float startTime;
    private bool isGameActive;
    public float elapsedTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            StartGame();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isGameActive)
        {
            elapsedTime = Time.time - startTime;
        }
    }

    public void StartGame()
    {
        startTime = Time.time;
        isGameActive = true;
        setLevel(1);
    }

    public void GameOver()
    {
        isGameActive = false;
        //uiManager.ShowGameOverScreen(elapsedTime);
        //SceneManager.LoadScene("1 Start Scene");
    }

    public void setLevel(int level)
    {
        this.level = level;

        if (level == 1)
        {
            spawner2.gameObject.SetActive(false);
            spawner1.gameObject.SetActive(true);
        }
        else if (level == 2)
        {
            spawner1.gameObject.SetActive(false);
            spawner2.gameObject.SetActive(true);
        }
    }

    public void LevelCoroutine()
    {
        pool.DestroyAllChildren();
        StartCoroutine(NextLevelCoroutine());
    }

    IEnumerator NextLevelCoroutine()
    {
        while (currentEnemyNum > 0)
        {
            yield return null;
        }

        setLevel(level + 1);
    }

    public int getCurrentEnemyNum()
    {
        return currentEnemyNum;
    }
    public void incCurrentEnemyNum()
    {
        currentEnemyNum++;
    }
    public void decCurrentEnemyNum()
    {
        currentEnemyNum--;
        kills++;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        inGameUIController.ToggleStopMenu();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        inGameUIController.ToggleStopMenu();
    }

    public void ReturnToMainMenu()
    {
        isGameActive = false;
        SceneManager.LoadScene("New Start Scene");
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
