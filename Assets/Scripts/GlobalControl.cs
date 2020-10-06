using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public float buttonPressed;

    public void Button0()
    {


        Debug.Log("Button 0");

    }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
   
    public void SetButton(float button)
    {
        buttonPressed = button;
    }
    
    public void SavePlayer()
    {
        GlobalControl.Instance.buttonPressed = buttonPressed;
    }

    void Start()
    {
        Debug.Log("im here!");
        buttonPressed = GlobalControl.Instance.buttonPressed;
    }
}
