using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    private bool onPause;
    public TextMeshProUGUI lifes;
    public TextMeshProUGUI bombs;
    public TextMeshProUGUI enemies;
    public TextMeshProUGUI timer;
    private float timerTime;
    void Start()
    {
        GameManager.Get.GameStart();
        GameManager.updateUIEvent += UpdateUI;
        UpdateUI();
    }
    void Update()
    {
        timerTime += Time.deltaTime;
        Timer();
    }
    void UpdateUI()
    {
        lifes.text = "x" + GameManager.Get.Lifes();
        bombs.text = "x" + GameManager.Get.RemainingBombs();
        enemies.text = GameManager.Get.RemainingEnemies() + " / " + GameManager.Get.TotalEnemies();
    }
    void Timer()
    {
        timer.text = timerTime.ToString("F2");
    }
    public void PauseButton()
    {
        onPause = !onPause;
        if (onPause)
        {
            Time.timeScale = 0;

        }
        else
        {
            Time.timeScale = 1;
        }
    }
}