using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    public Transform planetTransform;

    //note to self, stop trying to write our custom methods before start

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //the orbiting stuff starts here
    public void OrbitalMotion(float radius, float speed, Transform target)
    {
        List<Vector2> orbitPoints = new List<Vector2>();
        //gonna make it 16 points it orbits between
        float orbitAngle = 360 / 16;
        orbitAngle = Mathf.Deg2Rad * orbitAngle;
        float orbitTempX = 0;
        float orbitTempY = 0;
        //woo heres the points being added
        for (int i = 1; i < 17; i++)
        {
            orbitTempX = Mathf.Cos(i * orbitAngle) * radius;
            orbitTempX += target.position.x;
            orbitTempY = Mathf.Sin(i * orbitAngle) * radius;
            orbitTempY += target.position.y;
            orbitPoints.Add(new Vector2(orbitTempX, orbitTempY));
        }
        //checking that its the right number of points here
        //Debug.Log(orbitPoints.Count);
        //okay i added it to player to test, and it SHOULD be disabled but if it isnt... oops?

    }

    // Update is called once per frame
    void Update()
    {

    }
}
