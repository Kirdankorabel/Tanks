using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CannonType
{
    none,
    PowerCannon,
    FastCannons
}

[System.Serializable]
public class TankDefinition
{
    public CannonType cannonType = CannonType.none;
    // реализовать тип врага
    public int hullLevel;
    public int towerLevel;
    public int cannonLevel;
}
public class Tank : MonoBehaviour
{
    [Header("Tank")]
    public Loot lootPrefab;
    public Projectile projectilePrefab;
    public GameObject prefabTrack;
    public GameObject[] prefabHulls;
    public GameObject[] prefabTowers;
    public GameObject[] prefabPowerCannons;
    public GameObject[] prefabFastCannons;
    public GameObject visibilityAreaPrefab;

    public int health;
    public int damage;        
    public float accuracy;
    public float baseSpeed = 4;
    public TankDefinition def;
    
    private float rangeAttack;
    private int facing = 0;
    private Tank tank;
    private float speed;
    private int numTanks = 0;
    private bool isDestroy = false;

    private Rigidbody rigid;
    private GameObject track;
    private GameObject hull;
    private GameObject tower;
    private GameObject cannon;
    private TankBase parentBase;    

    public static string[] RedTeamTags = { "RedTank", "Player", "RedBase" };
    public static string[] BlueTeamTags = { "BlueTank", "BlueBase" };
    public static float projectileSpeed = 5;

    public static Vector3[] directions = new Vector3[] {
        Vector3.right,
        Vector3.up,
        Vector3.left,
        Vector3.down };

    public static int[] rotations = new int[] {
        0,
        90,
        180,
        270};

    public virtual void Awake()
    {
        facing = 0;
        rigid = GetComponent<Rigidbody>();
    }

    public virtual void Start()
    {
        GameObject b = null;
        if (this.tag == "BlueTank") b = GameObject.FindGameObjectWithTag("BlueBase");
        else  b = GameObject.FindGameObjectWithTag("RedBase");
        
        parentBase = b.GetComponent<TankBase>();
        numTanks = parentBase.NumTanks;
        InstantiateTank();
    }

    public virtual void Update()
    {
        Vector3 vel = Vector3.zero;
        if (dirHeld > -1)
        {
            vel = directions[dirHeld];
            transform.rotation = Quaternion.Euler(0, 0, rotations[dirHeld]);
            facing = dirHeld;
        }
        rigid.velocity = vel * Speed;        
    }

    void OnTriggerEnter(Collider coll)
    {
        GameObject otherGO = coll.gameObject;
        if ((otherGO.tag == "RedProjectile" && Contains(BlueTeamTags, this.tag)) ||
            (otherGO.tag == "BlueProjectile" && Contains(RedTeamTags, this.tag)))
        {
            Projectile pr = otherGO.GetComponent<Projectile>();
            health -= pr.Damage;
            Destroy(otherGO);

            if (health < 1 && isDestroy == false)
            {
                TankDestroyer();//
                isDestroy = true;
            }
        }
        else if (otherGO.tag == "Loot")
        {
            Loot loot = otherGO.GetComponent<Loot>();
            int lootLevel = loot.level;
            string lootType = loot.type;
            Looting(lootLevel, lootType);
            Destroy(otherGO);
        }
    }

    public void ItemDroper(Loot loot)
    {
        int l = loot.level;
        string t = loot.type;
        GameObject item;
        switch (t)
        {
            case "hull":
                item = Instantiate<GameObject>(prefabHulls[l]);
                break;
            case "tower":
                item = Instantiate<GameObject>(prefabTowers[l]);
                break;
            default:
                if (def.cannonType == CannonType.FastCannons)
                {
                    item = Instantiate<GameObject>(prefabPowerCannons[l]);
                }
                else item = Instantiate<GameObject>(prefabFastCannons[l]);
                break;
        }
        item.transform.SetParent(loot.transform);
        loot.transform.position = this.transform.position;
        Collider col = loot.GetComponent<Collider>();
        col.isTrigger = true;
    }

    public void TempFire(Vector3 vel)
    {
        Vector3 vec;
        Projectile projGO = Instantiate<Projectile>(projectilePrefab);        
        vec = transform.position;
        projGO.transform.position = vec;
        projGO.RangeAttack = rangeAttack;
        projGO.startPosition = vec;
        projGO.Damage = damage;

        projGO.transform.rotation = Quaternion.Euler(0, 0, rotations[facing]);
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        rigidB.velocity = vel * projectileSpeed;

        if (def.cannonType == CannonType.FastCannons)
        {
            Projectile projGO2 = Instantiate<Projectile>(projectilePrefab);
            vec = transform.position;
            projGO2.transform.position = vec;
            projGO2.RangeAttack = rangeAttack;
            projGO2.startPosition = vec;
            projGO2.Damage = damage;            
            Rigidbody rigidB2 = projGO2.GetComponent<Rigidbody>();
            rigidB2.velocity = vel * projectileSpeed;
            projGO2.transform.rotation = Quaternion.Euler(0, 0, rotations[facing]);

            if (facing == 1 || facing == 3)
            {
                projGO2.transform.position = projGO2.transform.position + new Vector3(0.1f, 0, 0);
            projGO.transform.position = projGO.transform.position + new Vector3(-0.1f, 0, 0);
            }
            else if (facing == 0 || facing == 2)
            {
                projGO2.transform.position = projGO2.transform.position + new Vector3(0, 0.1f, 0);
                projGO.transform.position = projGO.transform.position + new Vector3(0, -0.1f, 0);
            }

            if (this.tag == "BlueTank")
            {
                projGO2.tag = "BlueProjectile";
                projGO2.GetComponent<Renderer>().material.color = Color.blue;
            }
            else if (this.tag == "RedTank") projGO.tag = "RedProjectile";

            if (Random.value < (1 - accuracy)) projGO2.transform.position = new Vector3(0, 0, -1) + projGO2.transform.position;// промах в зависимости от башни
        }

        if (Random.value < (1 - accuracy)) projGO.transform.position = new Vector3(0, 0, -1) + projGO.transform.position;

        if (this.tag == "BlueTank")
        {
            projGO.tag = "BlueProjectile";
            projGO.GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (this.tag == "RedTank") projGO.tag = "RedProjectile";
    }

    public void Looting(int lootLevel, string lootType)
    {
        switch (lootType)
        {
            case "hull":
                if (lootLevel > this.def.hullLevel)
                {
                    Transform TANK = tank.transform;
                    Destroy(hull);
                    this.def.hullLevel = lootLevel;
                    hull = Instantiate<GameObject>(prefabHulls[lootLevel]);
                    hull.transform.SetParent(TANK);
                    hull.transform.position = TANK.transform.position;
                    hull.transform.rotation = Quaternion.Euler(0, rotations[facing], 0);
                }
                break;
            case "tower":
                if (lootLevel > this.def.towerLevel)
                {
                    Transform TANK = tank.transform;
                    Destroy(tower);
                    this.def.towerLevel = lootLevel;
                    tower = Instantiate<GameObject>(prefabTowers[lootLevel]);
                    tower.transform.SetParent(TANK);
                    tower.transform.position = TANK.transform.position;
                    tower.transform.rotation = Quaternion.Euler(0, rotations[facing], 0);
                }
                break;
            case "FastCannon":
                if (lootLevel > this.def.cannonLevel)
                {
                    Transform TANK = tank.transform;
                    Destroy(cannon);
                    this.def.cannonLevel = lootLevel;
                    cannon = Instantiate<GameObject>(prefabFastCannons[def.cannonLevel]);
                    cannon.transform.SetParent(TANK);
                    cannon.transform.position = TANK.transform.position;
                    cannon.transform.rotation = Quaternion.Euler(0, rotations[facing], 0);
                }
                break;
            case "PowerCannon":
                if (lootLevel > this.def.cannonLevel)
                {
                    Transform TANK = tank.transform;
                    Destroy(cannon);
                    this.def.cannonLevel = lootLevel;
                    cannon = Instantiate<GameObject>(prefabPowerCannons[def.cannonLevel]);
                    cannon.transform.SetParent(TANK);
                    cannon.transform.position = TANK.transform.position;
                    cannon.transform.rotation = Quaternion.Euler(0, rotations[facing], 0);
                }
                break;
        }
        charactersDeterminator();
    }

    private void charactersDeterminator()
    {
        // определение характеристик танка
        health = def.hullLevel * 2 + 2;
        speed = baseSpeed - baseSpeed / 4 * def.hullLevel;
        accuracy = 0.7f + def.towerLevel * 0.1f;
        if (def.cannonType == CannonType.FastCannons)
        {
            rangeAttack = 3 + def.cannonLevel;
            damage = 1 + def.cannonLevel;
        }
        else if (def.cannonType == CannonType.PowerCannon)
        {
            rangeAttack = 4 + 2 * def.cannonLevel;
            damage = 2 + 2 * def.cannonLevel;
        }

        // создание области видимости танка
        GameObject visibilityArea = Instantiate<GameObject>(visibilityAreaPrefab);
        visibilityArea.transform.SetParent(this.transform);
        visibilityArea.transform.position = this.transform.position + new Vector3(-rangeAttack / 2 - 0.5f, 0, 0);
        visibilityArea.transform.localScale = new Vector3(rangeAttack, 0, 0);

        Def = def;
    }

    private void InstantiateTank()
    {
        tank = this;
        if (Random.value > 0.5f) def.cannonType = CannonType.FastCannons;
        else def.cannonType = CannonType.PowerCannon;
        Vector3 spawnPosition = transform.position;
        // создание танка
        tank.transform.position = spawnPosition;
        Transform TANK = tank.transform;

        // создание траков
        track = Instantiate<GameObject>(prefabTrack);
        track.transform.SetParent(TANK);
        track.transform.position = TANK.transform.position;

        // создание корпуса
        def.hullLevel = numTanks % 3;
        hull = Instantiate<GameObject>(prefabHulls[def.hullLevel]);
        hull.transform.SetParent(TANK);
        hull.transform.position = TANK.transform.position;

        // создание башни
        def.towerLevel = (numTanks + 1) % 3;
        tower = Instantiate<GameObject>(prefabTowers[def.towerLevel]);
        tower.transform.SetParent(TANK);
        tower.transform.position = TANK.transform.position;

        // создание орудия
        if (def.cannonType == CannonType.PowerCannon)
        {
            def.cannonLevel = (numTanks + 1) % 3;
            cannon = Instantiate<GameObject>(prefabPowerCannons[def.cannonLevel]);
            cannon.transform.SetParent(TANK);
            cannon.transform.position = TANK.transform.position;
        }
        else
        {
            def.cannonLevel = (numTanks + 2) % 3;
            cannon = Instantiate<GameObject>(prefabFastCannons[def.cannonLevel]);
            cannon.transform.SetParent(TANK);
            cannon.transform.position = TANK.transform.position;
        }

        charactersDeterminator();
    }

    private Loot DropItems(Tank tank)
    {
        Loot loot = Instantiate<Loot>(lootPrefab);
        int value = Random.Range(0, 3);
        string t = null;
        int l = 0;
        switch (value)
        {
            case 0:
                t = "hull";
                l = tank.Def.hullLevel;
                break;
            case 1:
                t = "tower";
                l = tank.Def.towerLevel;
                break;
            case 2:
                if (tank.def.cannonType == CannonType.FastCannons) t = "FastCannon";
                else t = "PowerCannon";
                l = tank.Def.cannonLevel;
                break;
        }
        loot.type = t;
        loot.level = l;
        return loot;
    }

    private void TankDestroyer()
    {
        try
        {
            Loot loot = DropItems(this);
            ItemDroper(loot);
            this.parentBase.NumTanksOnMap--;
            Destroy(tank.gameObject);            
        }

        catch
        {
            Destroy(tank.gameObject);
        }
    }

    public static bool Contains(string[] strings, string s)
    {
        foreach (string str in strings)
        {
            if (str == s)
            {
                return true;
            }
        }
        return false;
    }

    public TankDefinition Def { get; set; }

    public int Facing 
    {
        get { return facing; }
        set { facing = value; }
    }

    public float Speed 
    {
        get { return speed; }
        set { speed = value; }
    }

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public int dirHeld { get; set; }
}
