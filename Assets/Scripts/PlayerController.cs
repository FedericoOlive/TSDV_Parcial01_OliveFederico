using UnityEditor.UI;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    private enum Directions { Forward, Right, Back, Left, None }
    private Directions dir = Directions.None;
    private Directions lastDir = Directions.Forward;
    private float distanceMove = 2;
    public LayerMask rayCastLayer;

    private Vector3 posNext;
    private Vector3 direction = Vector3.zero;
    [SerializeField] private float moveSpeed = 10f;
    private bool moving;

    private Quaternion rotLast;
    private Quaternion rotCurrent;
    private Quaternion rotation = Quaternion.identity;
    [SerializeField] private float rotSpeed = 5f;
    private bool rotating;
    private float rotTime;

    [Header("Bombs")]
    public GameObject pfBomb;
    [SerializeField] [Tooltip("Maximum simultaneous bombs")] private int BombsMax;
    private int BombsCurrent;

    void Start()
    {
        Transform transform1 = transform;

        Quaternion rot = transform1.rotation;
        rotLast = rot;
        rotCurrent = rot;

        Vector3 pos = transform1.position;
        posNext = pos;

        dir = Directions.None;
    }

    private void Update()
    {
        TryMove();
        TryDropBomb();
    }

    void TryMove()
    {
        if (!moving && !rotating)
        {
            if (Input.GetKey(KeyCode.W))
            {
                lastDir = dir;
                dir = Directions.Forward;
                SetStartRotateAndMove();

                direction = Vector3.forward;
                posNext = transform.position + Vector3.forward * distanceMove;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                lastDir = dir;
                dir = Directions.Left;
                SetStartRotateAndMove();

                direction = Vector3.left;
                posNext = transform.position + Vector3.left * distanceMove;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                lastDir = dir;
                dir = Directions.Back;
                SetStartRotateAndMove();

                direction = Vector3.back;
                posNext = transform.position + Vector3.back * distanceMove;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                lastDir = dir;
                dir = Directions.Right;
                SetStartRotateAndMove();

                direction = Vector3.right;
                posNext = transform.position + Vector3.right * distanceMove;
            }
        }

        RotatePlayer();
        Move();
    }
    void SetStartRotateAndMove()
    {
        rotation = Quaternion.Euler(Vector3.up * (int)dir * 90);

        rotating = true;
        rotTime = 0;
        rotCurrent = rotation;
        rotLast = transform.rotation;
        Debug.Log("Iniciar Movimiento de: " + transform.position + "  a: " + posNext);

        moving = false;
    }

    void RotatePlayer()
    {
        if (rotating)
        {
            if (dir == lastDir || rotTime > 1)
            {
                rotating = false;
                moving = CheckMoveAvailable();
                Debug.Log("Parar Rotacion.");
            }
            else
            {
                rotTime += Time.deltaTime * rotSpeed;
                transform.rotation = Quaternion.Lerp(rotLast, rotCurrent, rotTime);
                Debug.Log("Rotando.");
            }
        }
    }

    void Move()
    {
        if (moving)
        {
            Debug.Log("Dirección anterior:" + lastDir + "   Mi dirección: " + dir);
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
            switch (dir)
            {
                case Directions.Forward:

                    if (transform.position.z > posNext.z)
                    {
                        StopMovement();
                    }

                    break;
                case Directions.Left:

                    if (transform.position.x < posNext.x)
                    {
                        StopMovement();
                    }

                    break;
                case Directions.Back:

                    if (transform.position.z < posNext.z)
                    {
                        StopMovement();
                    }

                    break;
                case Directions.Right:

                    if (transform.position.x > posNext.x)
                    {
                        StopMovement();
                    }

                    break;
            }
        }
    }
    
    bool CheckMoveAvailable()
    {
        if (Physics.Raycast(transform.position, transform.forward, distanceMove, rayCastLayer))
        {
            Debug.Log("No se puede mover en esa direccion.");
            return false;
        }

        return true;
    }

    void StopMovement()
    {
        moving = false;
        Debug.Log("Parar Movimiento.");
        transform.position = posNext;
    }

    void TryDropBomb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Espacio Apretado.");
            if (BombsCurrent < BombsMax)
            {
                Debug.Log("Tira Bomba");
                BombsCurrent++;
                Instantiate(pfBomb, posNext, Quaternion.identity, GameObject.Find("Bombs RefName").transform);
            }
            else
            {
                Debug.Log("No se pueden tirar más bombas.");
            }
        }
    }
}
