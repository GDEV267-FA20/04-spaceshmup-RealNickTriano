using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroModelDisplay : MonoBehaviour
{
    [Header("Set in Inspector")]

    public Vector3 point1;
    public Vector3 point2;

    public float speed;

    [Header("Set Dynamically")]

    public float startTime;
    public float pointsDistance;

// Start is called before the first frame update

    void Start()
    {
        startTime = Time.time;

        pointsDistance = Vector3.Distance(point1, point2);
    }
    





// Update is called once per frame
    void Update()
    {

    float distCovered = (Time.time - startTime) * speed;

    float t = distCovered / pointsDistance;
        

        transform.localPosition = Vector3.Lerp(point1, point2, t);
        Debug.Log(t);


    }

}

