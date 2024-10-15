using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int enemyLeft = 10;
    public float timeLeft = 60f;

    private bool isPlaying = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (isPlaying)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f)
            {
                GameOverScene();
            }
        }
    }

    public void EnemyDied()
    {
        enemyLeft--;

        if (enemyLeft <= 0)
        {
            GameOverScene();
        }
    }

    public void GameOverScene()
    {
        isPlaying = false;
        SceneManager.LoadScene("GameOverScene");
    }
}