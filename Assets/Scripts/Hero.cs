using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S; // Singleton 

    bool firstButtonDown;
    float firstButtonDownTime;

    [Header("Set in Inspector")]

    // These fields control the movement of the ship

    public float speed = 30;

    public float rollMult = -45;

    public float pitchMult = 30;

    public float gameRestartDelay = 2f;

    public GameObject projectilePrefab;

    public float projectileSpeed = 40;

    public Weapon[] weapons;

    public Animator myAnimationController;


    [Header("Set Dynamically")]

    

    [SerializeField]

    private float _shieldLevel = 1; // Remember the underscore

    private bool aniTrigger;


    // This variable holds a reference to the last triggering GameObject

    private GameObject lastTriggerGo = null;

    // Declare a new delegate type WeaponFireDelegate


    public delegate void WeaponFireDelegate();                               


    // Create a WeaponFireDelegate field named fireDelegate.


    public WeaponFireDelegate fireDelegate;



    void Start()
    {

        if (S == null)
        {


            S = this; // Set the Singleton                                   

        }

        //fireDelegate += TempFire;

        // Reset the weapons to start _Hero with 1 blaster

        ClearWeapons();

        weapons[0].SetType(WeaponType.blaster);


    }
    

    // Update is called once per frame
    void Update()
    {
        if (!(Input.GetButtonDown("d")))
        {
            Move();
        }
        Dodge(aniTrigger);
        if (aniTrigger)
        {
            myAnimationController.ResetTrigger("EnterDodge");
        }

       


        // Use the fireDelegate to fire Weapons


        // First, make sure the button is pressed: Axis("Jump")


        // Then ensure that fireDelegate isn't null to avoid an error


        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {            // d


            fireDelegate();                                                  


        }

        

    }
    /*Steps to have a double tap of key trigger animation:
     * 1.When first key gets pressed 
     *      a. hold boolean value as true
     *      b. Get time of initial key down
     * 2.If the first key is pressed = true AND Time.time minus b is less then .5?
     * Then, activate animation
     * 3.At the end probably need to set boolean false otherwise it will keep triggering 2
     * 
     * 
     */
    bool Dodge(bool trigger)
    {
        

        if (Input.GetButtonDown("d") && !firstButtonDown)
        {
            firstButtonDownTime = Time.time;
            firstButtonDown = true;
            return trigger =  false;
        }
        else if (Input.GetButtonDown("d") && firstButtonDown)
        {
            if (firstButtonDownTime - Time.time < .5f)
            {
                //clear these values for the next check
                firstButtonDownTime = 0f;
                firstButtonDown = false;

                //Trigger animation
                myAnimationController.SetTrigger("EnterDodge");
                return trigger =true;
                Debug.Log("Animation Triggered");
            }
            else
            {
                //Didnt press quickly enough
                firstButtonDownTime = 0f;
                firstButtonDown = false;
                return trigger = false;

            }
            return trigger =  false;
        }

        return trigger =  false;
    }

    void Move()
    {
        // Pull in information from the Input class

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");



        // Change transform.position based on the axes

        Vector3 pos = transform.position;

        pos.x += xAxis * speed * Time.deltaTime;

        pos.y += yAxis * speed * Time.deltaTime;

        transform.position = pos;



        // Rotate the ship to make it feel more dynamic                      

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

    }

    /* void TempFire()
     {                                                        

         GameObject projGO = Instantiate<GameObject>(projectilePrefab);

         projGO.transform.position = transform.position;

         Rigidbody rigidB = projGO.GetComponent<Rigidbody>();

         //        rigidB.velocity = Vector3.up * projectileSpeed;                    




         Projectile proj = projGO.GetComponent<Projectile>();                


         proj.type = WeaponType.blaster;


         float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;


         rigidB.velocity = Vector3.up * tSpeed;


     }*/



    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;

        GameObject go = rootT.gameObject;

        //print("Triggered: " + go.name);

        // Make sure it's not the same triggering go as last time

        if (go == lastTriggerGo)
        {                                           

            return;

        }

        lastTriggerGo = go;



        if (go.tag == "Enemy")
        {  // If the shield was triggered by an enemy

            shieldLevel--;        // Decrease the level of the shield by 1

            Destroy(go);          // … and Destroy the enemy                 

        }
        else if (go.tag == "PowerUp")
        {

            // If the shield was triggered by a PowerUp

            AbsorbPowerUp(go);
        }
        else
        {

            print("Triggered by non-Enemy: " + go.name);

        }

    }

    public void AbsorbPowerUp(GameObject go)
    {

        PowerUp pu = go.GetComponent<PowerUp>();

        switch (pu.type)
        {

            case WeaponType.shield:                                          // a

                shieldLevel++;

                break;



            default:                                                         // b

                if (pu.type == weapons[0].type)
                { // If it is the same type  // c

                    Weapon w = GetEmptyWeaponSlot();

                    if (w != null)
                    {

                        // Set it to pu.type

                        w.SetType(pu.type);

                    }

                }
                else
                { // If this is a different weapon type               // d

                    ClearWeapons();

                    weapons[0].SetType(pu.type);

                }

                break;



        }

        pu.AbsorbedBy(this.gameObject);

    }

    public float shieldLevel
    {

        get
        {

            return (_shieldLevel);                                          

        }

        set
        {

            _shieldLevel = Mathf.Min(value, 4);                             

            // If the shield is going to be set to less than zero

            if (value < 0)
            {                                                 

                Destroy(this.gameObject);

                // Tell Main.S to restart the game after a delay

                Main.S.DelayedRestart(gameRestartDelay);

            }

        }

    }

    Weapon GetEmptyWeaponSlot()
    {

        for (int i = 0; i < weapons.Length; i++)
        {

            if (weapons[i].type == WeaponType.none)
            {

                return (weapons[i]);

            }

        }

        return (null);

    }



    void ClearWeapons()
    {

        foreach (Weapon w in weapons)
        {

            w.SetType(WeaponType.none);

        }

    }

}
