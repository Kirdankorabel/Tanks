using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Tank
{
    private float lastShotTime;

    private KeyCode[] keys = new KeyCode[] {
        KeyCode.RightArrow,
        KeyCode.UpArrow,
        KeyCode.LeftArrow,
        KeyCode.DownArrow };

    public override void Update()
    {
        pos = transform.position;
        dirHeld = -1;
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKey(keys[i])) dirHeld = i;
        }

        base.Update();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastShotTime)
        {
            TempFire(directions[Facing]);
            lastShotTime = Time.time + 0.5f;
        }
    }

    new void OnTriggerEnter(Collider coll)
    {
        GameObject otherGO = coll.gameObject;
        if (otherGO.tag == "BlueProjectile")
        {
            Projectile pr = otherGO.GetComponent<Projectile>();
            health -= pr.Damage;
            if (health < 0)
            {
                Destroy(this.gameObject);// окончание игры
                Controller.RedLose = true;
            }

            Destroy(otherGO);
            try
            {
                GameObject assistant = GameObject.Find("Assistant");

                Assistant ass = assistant.GetComponent<Assistant>();
                ass.inBattle = true;
            }
            catch { }
        }
    }

    void OnTriggerStay(Collider coll)
    {
        GameObject otherGO = coll.gameObject;
        if (otherGO.tag == "Loot")
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Loot loot = otherGO.GetComponent<Loot>();
                int lootLevel = loot.level;
                string lootType = loot.type;
                base.Looting(lootLevel, lootType);
                Destroy(otherGO);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                Destroy(otherGO);
            }
        }
    }
}
