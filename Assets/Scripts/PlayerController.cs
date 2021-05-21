using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    private enum Directions { Forward, Right, Back, Left, None }
    private Directions dir = Directions.None;
    private Directions lastDir = Directions.Forward;
    private float distanceMove = 2f;
    private float distanceRay = 2;
    public LayerMask moveBlockLayer;

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
    [SerializeField] [Tooltip("Maximum simultaneous bombs")] private int bombsMax;

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
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                lastDir = dir;
                dir = Directions.Forward;
                SetStartRotateAndMove();

                direction = Vector3.forward;
                posNext = transform.position + Vector3.forward * distanceMove;
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                lastDir = dir;
                dir = Directions.Left;
                SetStartRotateAndMove();

                direction = Vector3.left;
                posNext = transform.position + Vector3.left * distanceMove;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                lastDir = dir;
                dir = Directions.Back;
                SetStartRotateAndMove();

                direction = Vector3.back;
                posNext = transform.position + Vector3.back * distanceMove;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
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
            }
            else
            {
                rotTime += Time.deltaTime * rotSpeed;
                transform.rotation = Quaternion.Lerp(rotLast, rotCurrent, rotTime);
            }
        }
    }

    void Move()
    {
        if (moving)
        {
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
        if (Physics.Raycast(transform.position, transform.forward, distanceRay, moveBlockLayer))
        {
            //Debug.Log("No se puede mover en esa direccion.");
            return false;
        }

        return true;
    }

    void StopMovement()
    {
        moving = false;
        transform.position = posNext;
    }

    void TryDropBomb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.Get.bombsCurrent < bombsMax)
            {
                Instantiate(pfBomb, TransformRoundPosition(transform.position), Quaternion.identity, GameObject.Find("Bombs RefName").transform);
            }
            else
            {
                Debug.Log("No se pueden tirar más bombas.");
            }
        }
    }

    Vector3 TransformRoundPosition(Vector3 posGlobal)
    {
        Vector3 res = Vector3.zero;
        
        if (Mathf.RoundToInt(posGlobal.x) % 2 == 0)
        {
            res.x = Mathf.RoundToInt(posGlobal.x);
        }
        else
        {
            if (posGlobal.x > Mathf.RoundToInt(posGlobal.x))
            {
                res.x = Mathf.RoundToInt(posGlobal.x) + 1;
            }
            else
            {
                res.x = Mathf.RoundToInt(posGlobal.x) - 1;
            }
        }

        if (Mathf.RoundToInt(posGlobal.z) % 2 == 0)
        {
            res.z = Mathf.RoundToInt(posGlobal.z);
        }
        else
        {
            if (posGlobal.z > Mathf.RoundToInt(posGlobal.z))
            {
                res.z = Mathf.RoundToInt(posGlobal.z) + 1;
            }
            else
            {
                res.z = Mathf.RoundToInt(posGlobal.z) - 1;
            }
        }


        return res;
    }
}