using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S; // Singleton                              



    [Header("Set in Inspector")]

    // These fields control the movement of the ship

    public float speed = 30;

    public float rollMult = -45;

    public float pitchMult = 30;



    [Header("Set Dynamically")]

    public float shieldLevel = 1;

    // This variable holds a reference to the last triggering GameObject

    private GameObject lastTriggerGo = null;



    void Awake()
    {

        if (S == null)
        {


            S = this; // Set the Singleton                                   

        }
        else
        {

            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");

        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;

        GameObject go = rootT.gameObject;

        //print("Triggered: " + go.name);

        // Make sure it's not the same triggering go as last time

        if (go == lastTriggerGo)
        {                                           // c

            return;

        }

        lastTriggerGo = go;                                                  // d



        if (go.tag == "Enemy")
        {  // If the shield was triggered by an enemy

            shieldLevel--;        // Decrease the level of the shield by 1

            Destroy(go);          // … and Destroy the enemy                 // e

        }
        else
        {

            print("Triggered by non-Enemy: " + go.name);                       // f

        }

    }
}
