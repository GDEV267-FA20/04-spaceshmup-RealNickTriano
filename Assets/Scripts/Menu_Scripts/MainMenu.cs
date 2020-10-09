using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GlobalControl s;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SelectHero0()
    {
        s = GameObject.Find("GlobalObject").GetComponent<GlobalControl>();
        s.SetButton(0);
        Debug.Log("Set button to 0");
    }

    public void SelectHero1()
    {
        s = GameObject.Find("GlobalObject").GetComponent<GlobalControl>();
        s.SetButton(1);
        Debug.Log("Set button to 1");
    }

    public void SelectHero2()
    {
        s = GameObject.Find("GlobalObject").GetComponent<GlobalControl>();
        s.SetButton(2);
        Debug.Log("Set button to 2");
    }


}
