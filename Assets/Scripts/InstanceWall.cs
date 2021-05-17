using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InstanceWall : MonoBehaviour
{
    private const int MinOfRand = 3;
    public GameObject pfWallDestroyable;
    Transform floor;

    [SerializeField] private int totalDestWalls;

    public List<GameObject> wallDest = new List<GameObject>();

    private void Awake()
    {
        floor = GameObject.FindGameObjectWithTag("Floor").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        InstantiateAndSetPositionWallDest();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InstantiateAndSetPositionWallDest()
    {
        Vector3 size = floor.localScale;
        for (int i = 0; i < totalDestWalls; i++)
        {
            Vector3Int newPos = Vector3Int.zero;
            do
            {
                newPos.x = Random.Range(1, (int) size.x + 1);
                newPos.z = Random.Range(1, (int) size.z + 1);

            } while (CheckPosAvailable(newPos.x, newPos.z, (int) size.x));

            GameObject a = Instantiate(pfWallDestroyable, newPos, Quaternion.identity, transform);
            wallDest.Add(a);
            wallDest[i].transform.position *= 2;
        }
    }

    bool CheckPosAvailable(int newX, int newZ,int max)
    {
        if (newX < MinOfRand && newZ < MinOfRand)   //  Offset Player.
            return true;

        for (int i = 1; i < max; i++)               // Check Walls Normals.
        {
            if (newX % 2 == 0 && newZ % 2 == 0)
            {
                return true;
            }
        }

        for (int i = 0; i < wallDest.Count; i++)    // Check Wall Destroyed Positions.
        {
            Vector3 pos = wallDest[i].transform.localPosition;
            if (((int) pos.x) == newX && ((int) pos.z) == newZ)
            {
                return true;
            }
        }

        return false;
    }
}
