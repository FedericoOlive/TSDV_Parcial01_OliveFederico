using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiGameOver : MonoBehaviour
{
    public TextMeshProUGUI stats;
    int droppedBombs;

    // Start is called before the first frame update
    void Start()
    {
        String res;
        res = GameManager.Get.win ? "You win" : "You lose";
        res += "\n\nLifes: " + GameManager.Get.Lifes();
        res += "\nBombs Dropped: " + GameManager.Get.bombsDropped;

        res += "Total Enemies: " + GameManager.Get.TotalEnemies();
        res += "Total Remain: " + GameManager.Get.amountGhostEnemies + GameManager.Get.amountNormalEnemies;
    }
}
