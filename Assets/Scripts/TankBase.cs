using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankBase : MonoBehaviour
{
    public GameObject[] tankPrefabs;
    public GameObject playerPrefab;

    private int health = 10;
    private int maxTanks = 30;

    public int numTanksOnMap;

    public int NumTanks { get; set; }
    public int NumTanksOnMap { get; set; }

    private bool pl = true;

    void Avake()
    {
        NumTanks = 0;
        NumTanksOnMap = 0;
        maxTanks = 30;
    }

    void Update()
    {
        numTanksOnMap = NumTanksOnMap;
        if (this.tag == "RedBase" && pl)
        {
            GameObject player = Instantiate<GameObject>(playerPrefab);
            player.transform.position = transform.position + Vector3.left;
            player.transform.SetParent(this.transform);

            NumTanks++;
            NumTanksOnMap++;
            pl = false;
        }
        if (NumTanksOnMap < 2 && NumTanks < maxTanks)
        {
            int i = NumTanks % 3;
            GameObject tank = Instantiate<GameObject>(tankPrefabs[i]);
            if (NumTanks % 2 == 0) tank.transform.position = transform.position + Vector3.left;
            else tank.transform.position = transform.position - Vector3.left;

            tank.transform.SetParent(this.transform);

            NumTanks++;
            NumTanksOnMap++;
        }

        GameObject redTank = GameObject.FindGameObjectWithTag("RedTank");// костыль. иначе не спавнит красные танки
        if (redTank == null && this.tag == "RedBase")
        {
            int i = NumTanks % 3;
            GameObject tank = Instantiate<GameObject>(tankPrefabs[i]);
            if (NumTanks % 2 == 0) tank.transform.position = transform.position + Vector3.left;
            else tank.transform.position = transform.position - Vector3.left;

            tank.transform.SetParent(this.transform);
        }

    }
        
    void OnTriggerEnter(Collider coll)
{
        GameObject otherGO = coll.gameObject;
        if ((otherGO.tag == "RedProjectile" && this.tag == "BlueBase") ||
            (otherGO.tag == "BlueProjectile" && this.tag == "RedBase") )
        {
            Projectile pr = otherGO.GetComponent<Projectile>();
            health -= pr.Damage;
            if (health <= 0)
            {
                Destroy(this.gameObject);
                if (this.tag == "BlueBase") Controller.BlueLose = true;
                else Controller.RedLose = true;
            }
            Destroy(otherGO);
        }
    }
}
