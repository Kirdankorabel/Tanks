using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBot_ : Tank
{
    private int w;
    private int h;
    public bool attakPlayer;
    private int[,] cMap;
    private int[,] Map;
    private float lastShotTime;

    public override void Start()
    {
        w = Generator.Width;
        h = Generator.Height;
        base.Start();
        Map = Generator.GetMap();
        cMap = new int[w, h];

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                cMap[i, j] = Map[i, j];
            }
        }
    }

    public new void Update()
    {
        Move();
        base.Update();
    }

    public void OnTriggerStay(Collider coll)
    {
        GameObject otherGO = coll.gameObject;
        if ((this.tag == "RedTank" && Tank.Contains(Tank.BlueTeamTags, otherGO.tag) ||
            (this.tag == "BlueTank" && Tank.Contains(Tank.RedTeamTags, otherGO.tag)) ) 
            && Time.time > lastShotTime)
        {
            lastShotTime = Time.time + 0.5f;
            TempFire(directions[Facing]);
        }
    }

    public void PathFinder()//волновой алгоритм поиска пути
    {
        // позиция танка
        pos = transform.position;
        rY = (int)Mathf.Round(pos.y);
        rX = (int)Mathf.Round(pos.x);

        while (cMap[rX, rY] > -1)// заканчивается когда клетка под объектом помечена
        {
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (x == pX && y == pY && cMap[x, y] > -1) cMap[x, y] = -1;

                    if (cMap[x, y] < -1)
                    {
                        cMap[x, y] = cMap[x, y] - 1;
                    }

                    if (cMap[x, y] == -1)
                    {
                        cMap[x, y]--;
                        if (cMap[x - 1, y] == 0) cMap[x - 1, y] = 2;
                        if (cMap[x + 1, y] == 0) cMap[x + 1, y] = 2;
                        if (cMap[x, y + 1] == 0) cMap[x, y + 1] = 2;
                        if (cMap[x, y - 1] == 0) cMap[x, y - 1] = 2;
                    }
                }
            }
            for (int x = w - 1; x > 0; x--)
            {
                for (int y = h - 1; y > 0; y--)
                {
                    if (cMap[x, y] == 2)
                    {
                        cMap[x, y] = -1;
                    }
                }
            }
        }
    }

    public GameObject TargetsFinder(params string[] tags)
    {
        List<GameObject> targets = new List<GameObject>();
        List<float> distanses = new List<float>();
        for (int i = 0; i < tags.Length; i++)
        {
            GameObject[] t;
            string tag = tags[i];
            t = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject target in t)
            {
                targets.Add(target);
            }
        }

        foreach (GameObject target in targets)
        {
            float distanse = Mathf.Abs(target.transform.position.magnitude - transform.position.magnitude);
            distanses.Add(distanse);
        }
        int indexMin = IndexOfMin(distanses);

        GameObject tg = targets[indexMin];

        return tg;
    }

    private void Move()//направление на клетку, помечунную -1
    {
        if (cMap[rX, rY] == -1)
        {
            for (int x = 0; x < w - 1; x++)
            {
                for (int y = 0; y < h - 1; y++)
                {
                    if (cMap[x, y] < 0) cMap[x, y]++;
                }
            }
            if (cMap[rX + 1, rY] == -1) dirHeld = 0;
            if (cMap[rX, rY + 1] == -1) dirHeld = 1;
            if (cMap[rX - 1, rY] == -1) dirHeld = 2;
            if (cMap[rX, rY - 1] == -1) dirHeld = 3;
        }
    }

    public static int IndexOfMin(List<float> self)
    {
        float min = self[0];
        int minIndex = 0;

        for (int i = 1; i < self.Count; ++i)
        {
            if (self[i] < min)
            {
                min = self[i];
                minIndex = i;
            }
        }
        return minIndex;
    }

    public int pX { get; set; }
    public int pY { get; set; }
    public int rX { get; set; }
    public int rY { get; set; }
}
