using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public List<Transform> starTransforms;
    public float drawingTime;
    int starCurrent = 0;
    int starNext = 1;
    Vector3 drawPos;

    private void Start()
    {
        drawPos = starTransforms[0].position;
    }

    public void DrawConstellation()
    {
        // if the current star is the last in the list, then reset to the first in the list
        if (starCurrent >= starTransforms.Count)
        {
            starCurrent = 0;
            starNext = 1;
        }
        else
        {
            //Debug.Log(starTransforms[starCurrent].position + " " + drawPos);
         //otherwise check how close the end of the line is to the next star
         Vector3 temp = starTransforms[starNext].position - drawPos;
            //Debug.Log(drawPos + " " + starTransforms[starNext].position);
            float tempMag = temp.magnitude;
            //if its within a certain distance, simply draw the line to the next star and move the list up by 1
            if (temp.magnitude < 0.01f )
            {
                drawPos = starTransforms[starNext].position;
                starCurrent += 1;
                starNext += 1;
            }
            //otherwise, add the distance that would have been moved in the time since the last frame times however long we want it to take
            //then draw the line
            else
            {
                Vector3 temp2 = starTransforms[starNext].position - drawPos;
                temp2 = temp2 * drawingTime * Time.deltaTime;
                drawPos += temp2 * Time.deltaTime;
            }
            Debug.DrawLine(starTransforms[starCurrent].position, drawPos);
        }
        if (starCurrent == 0)
        {
            //do nothing
        }
        else
        {
            //draw the lines between all prior stars in the list
            for (int i = 0; i < starCurrent; i++)
            {
                Debug.DrawLine(starTransforms[i - 1].position, starTransforms[i].position);
            }
        }
        Debug.DrawLine(starTransforms[starCurrent].position, drawPos);
    }

    // Update is called once per frame
    void Update()
    {
        DrawConstellation();
    }
}
