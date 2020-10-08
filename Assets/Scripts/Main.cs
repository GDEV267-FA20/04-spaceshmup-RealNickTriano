﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;                                // A singleton for Main

    public GlobalControl script;

    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]

    public GameObject[] prefabEnemies;              // Array of Enemy prefabs

    public GameObject[] prefabHeroes;       // Array of Hero prefabs

    public GameObject newHero;          // Selected hero by player

    public float enemySpawnPerSecond = 0.5f; // # Enemies/second

    public float enemyDefaultPadding = 1.5f; // Padding for position

    public WeaponDefinition[] weaponDefinitions;

    public GameObject prefabPowerUp;                              // a


    public WeaponType[] powerUpFrequency = new WeaponType[] {       // b


                                    WeaponType.blaster, WeaponType.blaster,


                                    WeaponType.spread,  WeaponType.shield };

    


    private BoundsCheck bndCheck;

    public void shipDestroyed(Enemy e)
    {                                   // c


        // Potentially generate a PowerUp


        if (Random.value <= e.powerUpDropChance)
        {                           // d


            // Choose which PowerUp to pick


            // Pick one from the possibilities in powerUpFrequency


            int ndx = Random.Range(0, powerUpFrequency.Length);               // e


            WeaponType puType = powerUpFrequency[ndx];




            // Spawn a PowerUp


            GameObject go = Instantiate(prefabPowerUp) as GameObject;


            PowerUp pu = go.GetComponent<PowerUp>();


            // Set it to the proper WeaponType


            pu.SetType(puType);                                            // f




            // Set it to the position of the destroyed ship


            pu.transform.position = e.transform.position;


        }


    }
    /*public void button0Pressed()
    {
        bool Button0 = true;
        invoke("setHero");
    }
    public void button1Pressed()
    {
        bool Button1 = true;
        invoke("setHero");
    }
    public void button2Pressed()
    {
        bool Button2 = true;
        invoke("setHero");
    }
    public void setHero()
    {
        if (Button0)
        {
            newHero = prefabHeroes[0];
        }
        else if (Button1)
        {
            newHero = prefabHeroes[1];
        }
        else if (Button2)
        {
            newHero = prefabHeroes[2];
        }
        else
        {

        }
    }*/

    void Awake()
    {
        script = GameObject.Find("GlobalObject").GetComponent<GlobalControl>();
        Debug.Log(script.buttonPressed);

        Invoke("SpawnHero", 0f);

        S = this;

        // Set bndCheck to reference the BoundsCheck component on this GameObject

        bndCheck = GetComponent<BoundsCheck>();




        // Invoke SpawnEnemy() once (in 2 seconds, based on default values)

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        // A generic Dictionary with WeaponType as the key


        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();         // a


        foreach (WeaponDefinition def in weaponDefinitions)
        {              // b


            WEAP_DICT[def.type] = def;


        }

    }

    public void SpawnHero()
    {
        
        newHero = prefabHeroes[(int)script.buttonPressed];
        
        GameObject go = Instantiate<GameObject>(newHero);

        Vector3 pos = Vector3.zero;

        go.transform.position = pos;


    }

    public void SpawnEnemy()
    {

        // Pick a random Enemy prefab to instantiate

        int ndx = Random.Range(0, prefabEnemies.Length);

        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);



        // Position the Enemy above the screen with a random x position

        float enemyPadding = enemyDefaultPadding;
        print(go.GetComponent<BoundsCheck>());
        if (go.GetComponent<BoundsCheck>() != null)
        {

            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);

        }



        // Set the initial position for the spawned Enemy                    

        Vector3 pos = Vector3.zero;

        float xMin = -bndCheck.camWidth + enemyPadding;

        float xMax = bndCheck.camWidth - enemyPadding;

        pos.x = Random.Range(xMin, xMax);

        pos.y = bndCheck.camHeight + enemyPadding;

        go.transform.position = pos;



        // Invoke SpawnEnemy() again

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

    }

    public void DelayedRestart(float delay)
    {

        // Invoke the Restart() method in delay seconds

        Invoke("Restart", delay);

    }



    public void Restart()
    {

        // Reload _Scene_0 to restart the game

        SceneManager.LoadScene("_Scene_0");

    }

    /// <summary>


    /// Static function that gets a WeaponDefinition from the WEAP_DICT static


    /// protected field of the Main class.


    /// </summary>


    /// <returns>The WeaponDefinition or, if there is no WeaponDefinition with


    /// the WeaponType passed in, returns a new WeaponDefinition with a


    /// WeaponType of none..</returns>


    /// <param name="wt">The WeaponType of the desired WeaponDefinition</param>


    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {    // a


        // Check to make sure that the key exists in the Dictionary


        // Attempting to retrieve a key that didn't exist, would throw an error,


        // so the following if statement is important.


        if (WEAP_DICT.ContainsKey(wt))
        {                                     // b


            return (WEAP_DICT[wt]);
        }
        // This returns a new WeaponDefinition with a type of WeaponType.none,


        //   which means it has failed to find the right WeaponDefinition


        return (new WeaponDefinition());                                    // c


    }


}
