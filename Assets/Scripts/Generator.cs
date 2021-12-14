using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject groundPrefab;
    public GameObject wallPrefab;
    public GameObject redBasePrefab;
    public GameObject blueBasePrefab;

    static public int[,] MAP;
    private GameObject Maze;
    private static int width;
    private static int height;

    void Awake() 
    {
        width = 21;// линейные размеры танков и игрового уменьшены в 2 раза
        height = 21;

        Maze = new GameObject("Maze");
        Transform MAZE = Maze.transform;
        CreateMaze();
        InstanyiateBase();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (MAP[i, j] == 1)
                {
                    GameObject go = Instantiate<GameObject>(wallPrefab);
                    go.transform.SetParent(MAZE);
                    go.transform.position = new Vector3(i, j, 0);
                }
                else
                {
                    GameObject go = Instantiate<GameObject>(groundPrefab);
                    go.transform.SetParent(MAZE);
                    go.transform.position = new Vector3(i, j, 0);
                }
            }
        }
        
    }    

    private void CreateMaze()
    {
        MAP = new int[width, height];
        Transform MAZE = Maze.transform;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                {
                    MAP[i, j] = 1;
                }

                else if (i % 2 == 0 && j % 2 == 0)
                {
                    if (Random.value > 0.1f)
                    {
                        MAP[i, j] = 1;

                        int a = Random.value < 0.5f ? 0 : (Random.value < 0.5f ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < 0.5f ? -1 : 1);
                        MAP[i + a, j + b] = 1;
                    }
                }

                if (MAP[i, j] != 1) MAP[i, j] = 0;
            }
        }
    }

    private void InstanyiateBase()
    {
        int rnd1 = Random.Range(2, 4);
        int rnd2 = Random.Range(2, height - 3);
        GameObject redBase = Instantiate<GameObject>(redBasePrefab);
        redBase.transform.position = new Vector3(rnd1, rnd2, 0);
        GameObject blueBase = Instantiate<GameObject>(blueBasePrefab);
        blueBase.transform.position = new Vector3(width - rnd1 - 1, height - rnd2 - 1, 0);

        for (int i = rnd1 - 1; i < rnd1 + 2; i++)
        {
            for (int j = rnd2 - 1; j < rnd2 + 2; j++)
            {
                MAP[i, j] = 0;                
            }
        }

        for (int i = width - rnd1 - 2; i < width - rnd1 + 1; i++)
        {
            for (int j = height - rnd2 - 2; j < height - rnd2 + 1; j++)
            {
                MAP[i, j] = 0;
            }
        }

        MAP[rnd1, rnd2] = 1;
        MAP[width - rnd1 - 1, height - rnd2 - 1] = 1;
    }

    public static int[,] GetMap ()
    {
        return MAP;
    }

    public static int Width
    { 
        get { return width; }
    }
    public static int Height
    { 
        get { return height; } 
    }
}
