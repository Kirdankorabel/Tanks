using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : TankBot_
{
    new void Update()
    {
        string[] tags_;
        if (this.tag == "BlueTank")
        {
            tags_ = new string[] { "RedBase" };
        }
        else if (this.tag == "RedTank")
        {
            tags_ = new string[] { "BlueBase" };
        }
        else tags_ = null;
        GameObject go = TargetsFinder(tags_);
        Vector3 pPos = go.transform.position;
        pY = (int)Mathf.Round(pPos.y);
        pX = (int)Mathf.Round(pPos.x);

        PathFinder();
        base.Update();
    }
}
