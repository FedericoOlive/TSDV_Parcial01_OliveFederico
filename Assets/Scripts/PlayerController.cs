using System.Numerics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    private enum Directions { Forward, Right, Back, Left }
    private Directions dir = Directions.Forward;
    private Directions lastDir = Directions.Forward;
    private Vector3 distance = Vector3.one * 2;

    private Vector3 posLast;
    private Vector3 posCurrent;
    private Vector3 posNext;
    private Vector3 direction = Vector3.zero;
    private float moveSpeed;
    private bool moving;
    private float moveTime;

    private Quaternion rotLast;
    private Quaternion rotCurrent;
    private Quaternion rotNext;
    private Quaternion rotation = Quaternion.identity;
    private float rotSpeed = 5f;
    private bool rotating;
    private float rotTime;

    private float BombsMax;
    private float BombsCurrent;

    // Start is called before the first frame update
    void Start()
    {
        distance.y = 0;

        Transform transform1 = transform;

        Quaternion rot = transform1.rotation;
        rotLast = rot;
        rotCurrent = rot;
        rotNext = rot;

        Vector3 pos = transform1.position;
        posLast = pos;
        posCurrent = pos;
        posNext = pos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void Update()
    {
        TryMove();
        TryDropBomb();
    }

    void TryMove()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector3.forward;
            dir = Directions.Forward;

            SetStartRotateAndMove();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector3.left;
            dir = Directions.Left;

            SetStartRotateAndMove();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector3.back;
            dir = Directions.Back;

            SetStartRotateAndMove();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector3.right;
            dir = Directions.Right;

            SetStartRotateAndMove();
        }
        
        RotatePlayer(direction);
        //Move();
    }
    void SetStartRotateAndMove()
    {
        rotation = Quaternion.Euler(Vector3.up * (int)dir * 90);

        rotating = true;
        rotTime = 0;
        rotCurrent = rotation;
        rotLast = transform.rotation;
    }

    void RotatePlayer(Vector3 vecDir)
    {
        if (rotating)
        {
            rotTime += Time.deltaTime * rotSpeed;
            transform.rotation = Quaternion.Lerp(rotLast, rotCurrent, rotTime);
            Debug.Log("Rotando");

            if ((transform.rotation == rotation && rotTime > 0.5f) || rotTime > 1)
            {
                rotating = false;
                Debug.Log("Parar Rotacion");
            }
        }
    }

    void Move()
    {
        MovePlayer(direction);
        switch (dir)
        {
            case Directions.Forward:

                MovePlayer(direction);

                lastDir = Directions.Forward;

                break;
            case Directions.Back:

                MovePlayer(Vector3.back);

                lastDir = Directions.Back;

                break;
            case Directions.Left:

                MovePlayer(Vector3.left);

                lastDir = Directions.Left;

                break;
            case Directions.Right:

                MovePlayer(Vector3.right);

                lastDir = Directions.Right;

                break;
            default:
                Debug.LogWarning("Out of directions");
                break;
        }

        
    }

    void MovePlayer(Vector3 vecDir)
    {
        transform.position += vecDir;
    }

    void TryDropBomb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }
}
