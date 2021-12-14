using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assistant : TankBot_
{
    public bool inBattle = false;
    private float timeInBattle;
    private GameObject go;
    private Vector3 pPos;

    new void Start()
    {
        timeInBattle = 0;
        go = new GameObject();
        pPos = new Vector3();
        base.Start();
    }

    void FixedUpdate()
    {
        if (inBattle == false && Time.time > timeInBattle)
        {
            go = TargetsFinder("Player");
            pPos = go.transform.position;
            this.pY = (int)Mathf.Round(pPos.y);
            this.pX = (int)Mathf.Round(pPos.x);
        }
        if (inBattle == true)
        {
            timeInBattle = Time.time + 5f;
            go = TargetsFinder("BlueTank");
            pPos = go.transform.position;
            this.pY = (int)Mathf.Round(pPos.y);
            this.pX = (int)Mathf.Round(pPos.x);
            inBattle = false;
        }

        PathFinder();
        base.Update();
    }



}
