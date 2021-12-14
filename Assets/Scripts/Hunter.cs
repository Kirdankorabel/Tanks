using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : TankBot_
{
    private int width;
    private int height;
    float timeInstantiatePoint;
    int x;
    int y;
    float speed_;

    private int[,] Map;

    new void Start()
    {
        base.Start();
        Map = Generator.GetMap();
        speed_ = Speed;
        width = Generator.Width;
        height = Generator.Height;
    }

    new void Update()
    {
        string[] tags_;
        if (this.tag == "BlueTank")
        {
            tags_ = new string[] { "Player", "RedTank" };
        }
        else
        {
            tags_ = new string[] { "BlueTank" };
        }
                
        if (pX == rX && pY == rY) Speed = 0;
        if (Time.time > timeInstantiatePoint)
        {
            Speed = speed_;
            timeInstantiatePoint = Time.time + 10f;
            InstantiateTargetPoint();
        }

        if (rX == pX && rY == pY)
        {
            Speed = 0;
            GameObject go = TargetsFinder(tags_);
            Vector3 pPos = go.transform.position;
            pY = (int)Mathf.Round(pPos.y);
            pX = (int)Mathf.Round(pPos.x);
        }

        PathFinder();
        base.Update();
    }

    void InstantiateTargetPoint()
    {
        int x = Random.Range(4, width - 4);
        int y = Random.Range(4, height - 4);
        pX = x;
        pY = y;
        int n = Map[x, y + 1] + Map[x, y - 1] + Map[x - 1, y] + Map[x + 1, y];// проверка на наличие рядом укрытия
        if (Map[x, y] == 1|| n < 1)
        {
            InstantiateTargetPoint();
        }
    }
}
