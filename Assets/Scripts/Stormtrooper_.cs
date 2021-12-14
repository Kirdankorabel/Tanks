using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stormtrooper_ : TankBot_
{
    void FixedUpdate()
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
        GameObject go = TargetsFinder(tags_);
        Vector3 pPos = go.transform.position;
        this.pY = (int)Mathf.Round(pPos.y);
        this.pX = (int)Mathf.Round(pPos.x);

        PathFinder();
        base.Update();
    }
}
