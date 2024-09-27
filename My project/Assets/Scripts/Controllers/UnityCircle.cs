using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCircle : MonoBehaviour
{
    List <float> angles = new();
    int current = 0;
    public float radius;
    public Vector3 center;
    public float timeWait;
    float timePassed = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            float angled = Random.Range(0, 360);
            angles.Add(angled);

        }
    }

    void ListLoop ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (current == angles.Count - 1)
            {
                current = 0;
            }
            else
            {
                current += 1;
            }
        }
        else
        {   //handles getting the x value from the angle when the radius is 1
            float xVal = Mathf.Cos(Mathf.Deg2Rad * angles[current]);
            xVal += center.x; //adjusts the center of the circle to whereever was specified by the center variable
            //handles getting the y value from the angle when the radius is 2
            float yVal = Mathf.Sin(Mathf.Deg2Rad * angles[current]);
            yVal += center.y; //adjusts the center of the circle to whereever was specified by the center variable
            Vector3 point = new(xVal, yVal);
            //changes the radius of the circle so the X and Y values discovered reflect it
            point = point * radius;
            Debug.DrawLine(Vector3.zero, point);
        }
        timePassed += Time.deltaTime;
        if (timePassed >= timeWait)
        {
            int newangle = 0;
            foreach (float angle in angles)
            {
                angles[newangle] = Random.Range(0, 360);
                newangle += 1;
            }
            timePassed = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ListLoop();
    }
}
