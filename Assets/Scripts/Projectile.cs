using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 startPosition;
    public int damage;

    void Awake()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (Mathf.Abs(startPosition.magnitude - transform.position.magnitude) > RangeAttack) Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider coll)
    {
        GameObject otherGO = coll.gameObject;
        if (otherGO.tag == "Wall") Destroy(this.gameObject);  
    }

    public int Damage 
    { 
        get { return damage; }
        set { damage = value; }
    }
    public float RangeAttack { get; set; }
}

