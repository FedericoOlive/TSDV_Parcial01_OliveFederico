using UnityEngine;

public class PowersUp : MonoBehaviour
{
    public enum PowerUp { MoreExplosion, MoreBombs, MoreLife}

    public PowerUp powerUp = PowerUp.MoreExplosion;
    private PlayerController player;
    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        powerUp = (PowerUp) Random.Range((int) PowerUp.MoreExplosion, (int) PowerUp.MoreLife + 1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player")
        {
            switch (powerUp)
            {
                case PowerUp.MoreExplosion:
                    GameManager.Get.IncreaseBombDistance();
                    Debug.Log("Se aumentó la distancia de la bomba.");
                    break;
                case PowerUp.MoreBombs:
                    GameManager.Get.IncreaseBombs();
                    Debug.Log("Se aumentó la Cantidad de las bombas.");
                    break;
                case PowerUp.MoreLife:
                    player.life++;
                    Debug.Log("Se aumentó una vida.");
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }
    }
}
