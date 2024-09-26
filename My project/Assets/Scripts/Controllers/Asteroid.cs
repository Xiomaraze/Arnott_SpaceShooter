using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float moveSpeed;
    public float arrivalDistance;
    public float maxFloatDistance;
    bool arrived = true;
    Vector3 destination;
    Vector3 velo;

    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
        velo = Vector3.zero;
    }

    public void AsteroidMovement()
    {
        if (arrived == true)
        {
            //pulling a random number within the designated 
            float ranX = Random.Range(-maxFloatDistance, maxFloatDistance);
            float ranY = Random.Range(-maxFloatDistance, maxFloatDistance);
            ranX = transform.position.x + ranX;
            ranY = transform.position.y + ranY;
            destination = new Vector3(ranX, ranY, 0);
            arrived = false;
        }
        Vector3 temp = transform.position - destination;
        float tempMag = temp.magnitude;
        if (tempMag < arrivalDistance)
        {
            arrived = true;
        }
        else
        {
            velo = temp.normalized * moveSpeed * Time.deltaTime;
            transform.position += velo * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AsteroidMovement();
        
    }
}
