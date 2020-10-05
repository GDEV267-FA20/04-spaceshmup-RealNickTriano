﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]

    public float speed = 10f;      // The speed in m/s

    public float fireRate = 0.3f;  // Seconds/shot (Unused)

    public float health = 10;

    public int score = 100;      // Points earned for destroying this

    protected BoundsCheck bndCheck;

    void Awake()
    {                                                           

        bndCheck = GetComponent<BoundsCheck>();

    }

    // This is a Property: A method that acts like a field

    public Vector3 pos
    {                                                     


        get
        {

            return (this.transform.position);

        }

        set
        {

            this.transform.position = value;

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (bndCheck != null && bndCheck.offDown)
        {

            Destroy(gameObject);

        }
    }

    public virtual void Move()
    {                                             

        Vector3 tempPos = pos;

        tempPos.y -= speed * Time.deltaTime;

        pos = tempPos;

    }

    void OnCollisionEnter(Collision coll)
    {

        GameObject otherGO = coll.gameObject;

        switch (otherGO.tag)
        {

            case "ProjectileHero":                                           // b

                Projectile p = otherGO.GetComponent<Projectile>();


        // If this Enemy is off screen, don't damage it.

        if (!bndCheck.isOnScreen)
        {                                // c

            Destroy(otherGO);

            break;

        }



        // Hurt this Enemy

        // Get the damage amount from the Main WEAP_DICT.

        health -= Main.GetWeaponDefinition(p.type).damageOnHit;

        if (health <= 0)
        {                                           // d

            // Destroy this Enemy

            Destroy(this.gameObject);

        }

        Destroy(otherGO);                                          // e

        break;



        default:

                print("Enemy hit by non-ProjectileHero: " + otherGO.name); // f

        break;



        }

    }

    
}
