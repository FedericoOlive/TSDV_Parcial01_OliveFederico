using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if (GameManager.Get.noMoreEnemies)
            {
                GameManager.Get.win = true;
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}
