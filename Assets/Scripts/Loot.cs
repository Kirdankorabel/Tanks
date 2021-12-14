using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public int level;
    public string type;
    void Awake()
    {
        level = Level;
        type = Type;
    }

    //Loot(string s, int i)
    //{ }


    //void OnTriggerEnter(Collider coll)
    //{
    //    GameObject otherGO = coll.gameObject;
    //    if (otherGO.tag == "BlueTank")
    //    {
    //        Tank tank = otherGO.GetComponent<Tank>();
    //        tank.Looting(level, type);
    //        Destroy(this);
    //    }
    //}

    public static int Level { get; set; }
    public static string Type { get; set; }
}
