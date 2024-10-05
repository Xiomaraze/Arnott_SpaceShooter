﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Transform> asteroidTransforms;
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public Transform bombsTransform;

    private Vector3 velocity = Vector3.zero;
    public float targetSpeed = 3f;
    public float timeTakenToMaxSpeed = 2f;
    string speed;

    private float acceleration = 1f;
    bool forBack;
    bool leftRight;

    public GameObject powerupPrefab;

    private void Start()
    {
        acceleration = targetSpeed / timeTakenToMaxSpeed;
    }

    public void EnemyRadar(float radius, int circlePoints)
    {
        float circleAngles = 360 / circlePoints;
        float dist = Vector2.Distance(transform.position, enemyTransform.position);
        float tempX = 0;
        float tempY = 0;
        Color proxColour = Color.white;

        List<Vector2> circled = new List<Vector2>();
        //draw the red circle here
        //m really tired so this is requiring more brain power than i have available, but i can do it
        for (int i = 1; i < circlePoints; i++)
        {
            float angle = circleAngles * i;
            angle = angle * 100;
            angle = Mathf.Round(angle);
            angle = angle / 100;
            //bellow just asking for the angle returned as if it were in the pos x pos y axis area while keeping in mind where the x and y on the full angle would actually end up
            if (angle > 270)
            {
                //the y will be negative but the X pos
                angle -= 270;
                tempX = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
                tempY = Mathf.Sin(Mathf.Deg2Rad * angle) * radius * -1;
            }
            else if (angle > 180)
            {
                //the x and y will be neg
                angle -= 180;
            }
            else if (angle > 90)
            {
                //the x will be neg, the y pos
                angle -= 90;
            }
            else
            {
                tempX = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
                tempY = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            }
            tempX += transform.position.x;
            tempY += transform.position.y;
            circled.Add(new Vector2(tempX, tempY));
        }
        if (dist < radius)
        {
            //draw the red circle
            proxColour = Color.red;        }
        else
        {
            //draw the green circle here
        }
        for (int i = 0; i < circled.Count - 1; i++)
        {
            if (i == 0)
            {
                Debug.DrawLine(circled[0], circled[circled.Count - 1], proxColour);
            }
            else
            {
                Debug.DrawLine(circled[i], circled[i - 1], proxColour);
            }
        }
    }

    public void PlayerMovement()
    {
        //Tasks 1A, 1B, 1C
        //Tasks 1A and 1B were completed in class


        //velocity = Vector3.zero;
        //if ((Input.GetKey(KeyCode.D)) | (Input.GetKey(KeyCode.RightArrow)))
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //moving right
            velocity += Vector3.right * acceleration * Time.deltaTime;
            leftRight = true;
        }
        //else if ((Input.GetKey(KeyCode.A)) | (Input.GetKey(KeyCode.LeftArrow)))
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            //moving left
            velocity += Vector3.left * acceleration * Time.deltaTime;
            leftRight = true;
        }
        else
        { 
            if (velocity.magnitude > 0) 
            leftRight = false;
        }
        //if ((Input.GetKey(KeyCode.W)) | (Input.GetKey(KeyCode.UpArrow)))
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //moving up
            velocity += Vector3.up * acceleration * Time.deltaTime;
            forBack = true;
        }
        //else if ((Input.GetKey(KeyCode.S)) | (Input.GetKey(KeyCode.DownArrow)))
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //moving down
            velocity += Vector3.down * acceleration * Time.deltaTime;
            forBack = true;
        }
        else
        {
            forBack = false;
        }
        float temp;
        temp = velocity.magnitude;
        temp = Mathf.Abs(temp);
        if (temp > targetSpeed)
        {
            velocity = velocity.normalized * targetSpeed;
        }
        if ((!leftRight) && (!forBack))
        {
            Vector3 temp2 = velocity - (velocity.normalized * acceleration * Time.deltaTime);
            if ((velocity.magnitude > 0) && (temp2.magnitude < 0))
            {
                velocity = Vector3.zero;
            }
            else if ((velocity.magnitude < 0) && (temp2.magnitude > 0))
            {
                velocity = Vector3.zero;
            }
            else
            {
                velocity = temp2 * Time.deltaTime;
            }
            //velocity = velocity - (velocity.normalized * acceleration * Time.deltaTime);
        }
        transform.position += velocity * Time.deltaTime;
        //speed = velocity.magnitude.ToString();
    }

    void Update()
    {
        PlayerMovement();
        
    }

}
