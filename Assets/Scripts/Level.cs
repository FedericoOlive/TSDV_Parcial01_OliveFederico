using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
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
        if (GameManager.Get.Lifes() <= 0)
        {
            SceneManager.LoadScene("GameOver");
            GameManager.Get.win = false;
        }
        lifes.text = "x" + GameManager.Get.Lifes();
        bombs.text = "x" + GameManager.Get.RemainingBombs();
        enemies.text = GameManager.Get.RemainingEnemies() + " / " + GameManager.Get.TotalEnemies();
    }
    void Timer()
    {
        timer.text = timerTime.ToString("F2");
    }
}