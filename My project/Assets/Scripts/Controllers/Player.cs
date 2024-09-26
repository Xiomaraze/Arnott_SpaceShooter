using System.Collections;
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

    private void Start()
    {
        acceleration = targetSpeed / timeTakenToMaxSpeed;
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
