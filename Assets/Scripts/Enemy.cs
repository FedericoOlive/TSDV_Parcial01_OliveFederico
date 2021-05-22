using System;
using UnityEngine;
using UnityEngine.Events;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
public class Enemy : MonoBehaviour
{
    public delegate void enemyDestroyed(GameObject enemy);
    public static enemyDestroyed enemyDestroyedEvent;

    public enum EnemyType { Normal, Ghost }

    public EnemyType enemyType = EnemyType.Normal;

    [SerializeField] private LayerMask moveBlockLayer;
    [SerializeField] private LayerMask damageables;

    private int blocksMove = 0;
    private int minBlocksMove = 1;
    private int maxBlocksMove = 6;
    private int randMove = 0;
    private int tryMoves = 0;
    private enum Directions { Forward, Right, Back, Left, None }
    private Directions dir = Directions.None;
    private Directions lastDir = Directions.Forward;
    private float distanceMove = 2f;

    private Vector3 posNext;
    private Vector3 direction = Vector3.zero;
    private float moveSpeed = 10f;
    private bool moving;

    private Quaternion rotLast;
    private Quaternion rotCurrent;
    private Quaternion rotation = Quaternion.identity;
    private float rotSpeed = 5f;
    private bool rotating;
    private float rotTime;

    public void SetEnemy(EnemyType typeEnemy,LayerMask layer, float speedMove, float speedRot)
    {
        enemyType = typeEnemy;
        moveBlockLayer = layer;
        moveSpeed = speedMove;
        rotSpeed = speedRot;
    }

    private void Update()
    {
        TryMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerEquals(damageables, other.transform.gameObject.layer))
        {
            other.GetComponent<PlayerController>().DismissLife();
        }
    }

    void TryMove()
    {
        if (!moving && !rotating)
        {
            if (blocksMove <= 0)
            {
                blocksMove = Random.Range(minBlocksMove, maxBlocksMove);
                randMove = Random.Range((int) Directions.Forward, (int) Directions.None);
                tryMoves = 0;
            }
            else
            {
                randMove = (int) CalculateOpositeDirection(dir);
                if (tryMoves > 1)
                {
                    randMove = Random.Range((int) Directions.Forward, (int) Directions.None);
                }
                else if (tryMoves > 0)
                {
                    blocksMove--;
                }
                tryMoves++;
            }

            switch ((Directions)randMove)
            {
                case Directions.Forward:
                    lastDir = dir;
                    dir = Directions.Forward;
                    SetStartRotateAndMove();

                    direction = Vector3.forward;
                    posNext = transform.position + Vector3.forward * distanceMove;
                    break;
                case Directions.Left:
                    lastDir = dir;
                    dir = Directions.Left;
                    SetStartRotateAndMove();

                    direction = Vector3.left;
                    posNext = transform.position + Vector3.left * distanceMove;
                    break;
                case Directions.Back:
                    lastDir = dir;
                    dir = Directions.Back;
                    SetStartRotateAndMove();

                    direction = Vector3.back;
                    posNext = transform.position + Vector3.back * distanceMove;
                    break;
                case Directions.Right:
                    lastDir = dir;
                    dir = Directions.Right;
                    SetStartRotateAndMove();

                    direction = Vector3.right;
                    posNext = transform.position + Vector3.right * distanceMove;
                    break;
                default:
                    Debug.LogError("Random de 4 direcciones excedió el límite.");
                    break;
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
        if (Physics.Raycast(transform.position, transform.forward, distanceMove, moveBlockLayer))
        {
            return false;
        }
        return true;
    }

    void StopMovement()
    {
        moving = false;
        transform.position = posNext;
    }

    Directions CalculateOpositeDirection(Directions direc)
    {
        switch (direc)
        {
            case Directions.Forward:
                direc = Directions.Back;
                break;
            case Directions.Left:
                direc = Directions.Right;
                break;
            case Directions.Back:
                direc = Directions.Forward;
                break;
            case Directions.Right:
                direc = Directions.Left;
                break;
            default:
                break;
        }

        return direc;
    }
    bool LayerEquals(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }


    public void DestroyEnemy()
    {
        if (enemyType == EnemyType.Normal)
            GameManager.Get.amountNormalEnemies--;
        else if (enemyType == EnemyType.Ghost)
            GameManager.Get.amountGhostEnemies--;

        enemyDestroyedEvent?.Invoke(gameObject);
        Destroy(gameObject);
        GameManager.updateUIEvent?.Invoke();
    }
}
