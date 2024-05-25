using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.XR;

using Unity.XR.CoreUtils;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public Spawner spawner;
    public PoolManager pool;
    //public UIManager uiManager;

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
        //uiManager.ShowPauseMenu();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        //uiManager.HidePauseMenu();
    }
}
