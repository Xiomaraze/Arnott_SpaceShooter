using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

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

    public GameObject minesPrefab;

    List <GameObject> mines;
    bool bombTrail = false;
    Vector2 prevPosition;
    float angleOfTravel; //assumes player is (0,0) and gets the angle which they are traveling in if 0 degrees is directly right
    public float mineSpawnDistance = 0.1f;
    Vector3 mineSpawn = Vector3.zero;

    public List<GameObject> meteors = new List<GameObject>();

    private void Start()
    {
        acceleration = targetSpeed / timeTakenToMaxSpeed;
        prevPosition = transform.position;
    }

    public void SpawnPowerups(float radius, int numOfPowerups)
    {
        float degSeparation = 360 / numOfPowerups; //this probably gives us a decimal with a ridiculous number of decimal places
        //so were gonna round it to 2 decimal places
        degSeparation *= 100;
        degSeparation = Mathf.Round(degSeparation);
        degSeparation /= 100;
        //should be a nice 2 decimal place number now
        //now we get to assign all the points, what fun help i forgot how to make a list in the last 10 minutes
        List <Vector2> powerupSpawns = new List<Vector2>();
        List <GameObject> powerupObjects = new List<GameObject>();
        float powerupX = 0;
        float powerupY = 0; //assigning pointless x and y values so unity doesnt lose its literal mind
        for (int i = 0; i < numOfPowerups; i++)
        {
            powerupX = Mathf.Cos(Mathf.Deg2Rad * degSeparation * i) * radius;
            powerupY = Mathf.Sin(Mathf.Deg2Rad * degSeparation * i) * radius; //the * i is to make sure its the right angle in total on the circle, since the 0 angle is directly on the x axis
            //add the player transform so its centered on the player
            powerupX += transform.position.x;
            powerupY += transform.position.y;
            //now make it a new vector2 poing to spawn the future powerup at
            powerupSpawns.Add(new Vector2(powerupX, powerupY));
        }
        //now we actually spawn them as a list
        foreach (Vector2 spawn in powerupSpawns)
        {
            powerupObjects.Add(Instantiate(powerupPrefab, new Vector3(spawn.x, spawn.y, 0), Quaternion.identity));
        }
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

    public void SpawnBombTrail()
    {

        //checking if mouse button is being held for bomb trail spawning
        if (Input.GetMouseButton(0))
        {
            bombTrail = true;
        }
        else
        {
            bombTrail = false;
        }
        //bombtrail just handles whether or not the program needs to actually spawn the bombs
        if (bombTrail)
        {
            //need direction the player is moving to spawn appropriate place for bomb
            Vector2 tempTracker = transform.position;
            tempTracker = tempTracker - prevPosition; //shifts previous position as if player is currently at 0,0
            tempTracker.Normalize();
            angleOfTravel = Mathf.Atan2(tempTracker.y, tempTracker.x);
            mineSpawn.x = Mathf.Cos(angleOfTravel);
            mineSpawn.y = Mathf.Sin(angleOfTravel);
            if (mines.Count == 0) //exclusively to handle when no bombs are already existing on screen
            {
                Debug.Log("Ding");
                mines.Add(Instantiate(minesPrefab, mineSpawn, Quaternion.identity));
                //hopefully this just adds the first mine to the list?
            }
            else
            {
                if (Vector2.Distance(transform.position, mines[mines.Count - 1].transform.position) >= mineSpawnDistance) //checks distance between player and the last spawned mine against the required distance
                {
                    //if player is at or further than the min distance, spawn a new mine
                    mines.Add(Instantiate(minesPrefab, mineSpawn, Quaternion.identity));
                }
                else
                {
                    //do nothing if its not far enough yet
                }
            }
        }
    }

    public void MineExploded(Transform mineLocation)
    {
        int removeNum = 999;
        for (int i = 0; i < mines.Count; i++)
        {
            if (mines[i].transform == mineLocation)
            {
                removeNum = i;
            }
        }
        mines.RemoveAt(removeNum);
    }

    public float shieldDistance;
    List<Vector2> shieldPoints = new List<Vector2>(8);// 8 points should be cardinals and inter-cardinals 

    float FindX (float degree, float distance)
    {
        float tempFindX = Mathf.Cos(Mathf.Deg2Rad * degree);
        tempFindX = tempFindX * distance;
        tempFindX += transform.position.x;
        return tempFindX;
    }
    float FindY (float degree, float distance)
    {
        float tempFindY = Mathf.Sin(Mathf.Deg2Rad * degree);
        tempFindY = tempFindY * distance;
        tempFindY += transform.position.y;
        return tempFindY;
    }

    void DrawShield()
    {
        //calculation temporaries
        float tempX = 0f;
        float tempY = 0f;
        shieldPoints.Clear();
        //East, 0 degrees
        shieldPoints.Add(new Vector2(transform.position.x + shieldDistance, transform.position.y));
        //north east, 45 degrees
        tempX = FindX(45f, shieldDistance);
        tempY = FindY(45f, shieldDistance);
        shieldPoints.Add(new Vector2(tempX, tempY));
        // north
        tempX = FindX(90f, shieldDistance);
        tempY = FindY(90f, shieldDistance);
        shieldPoints.Add(new Vector2(tempX, tempY));
        //north west
        tempX = FindX(135f, shieldDistance);
        tempY = FindY(135f, shieldDistance);
        shieldPoints.Add(new Vector2(tempX, tempY));
        // west
        tempX = FindX(180f, shieldDistance);
        tempY = FindY(180f, shieldDistance);
        shieldPoints.Add(new Vector2(tempX, tempY));
        // south west
        tempX = FindX(225f, shieldDistance);
        tempY = FindY(225f, shieldDistance);
        shieldPoints.Add(new Vector2(tempX, tempY));
        //south
        tempX = FindX(270f, shieldDistance);
        tempY = FindY(270f, shieldDistance);
        shieldPoints.Add(new Vector2(tempX, tempY));
        //south east
        tempX = FindX(315f, shieldDistance);
        tempY = FindY(315f, shieldDistance);
        shieldPoints.Add(new Vector2(tempX, tempY));
        // yay all the points are set, now draw it please'
        for (int i = 1; i < shieldPoints.Count; i++)
        {
            Debug.DrawLine(shieldPoints[i], shieldPoints[i - 1], Color.magenta);
        }
        Debug.DrawLine(shieldPoints[shieldPoints.Count - 1], shieldPoints[0], Color.magenta);
    }
    public void DeflectShield()
    {
        //here we go task 2!
        //starting with the basic, this is the shield thing
        DrawShield();
        //woo draw shield is being nice!
        //okay now meteors, im just gonna check it up against the list of meteors
        foreach (GameObject m in meteors)
        {
            float tempDist = Vector2.Distance(transform.position, m.transform.position);
            if (tempDist <= shieldDistance)
            {
                //reflect! new method!
                Vector2 tempShieldpoint = transform.position - m.transform.position;
                tempShieldpoint.Normalize();
                tempShieldpoint = tempShieldpoint * shieldDistance;
                //where its being reflected from
                Vector2 shieldNormal = Vector2.Perpendicular(tempShieldpoint);
                //its normal/perpendicular necessary for the reflect method
                //okay so the indirection will be
                Meteor meteorScript = m.GetComponent<Meteor>();
                Vector2 inDir = meteorScript.direction;
                //so this is then the out direction
                Vector2 outDir = Vector2.Reflect(inDir, shieldNormal);
                Vector3 outDir3 = outDir;
                meteorScript.direction = outDir3;
                //god i hope that works, because theres like no explanation for the reflect method in vector2 on the unity API
            }
            else
            {
                //nothing interesting
            }
        }
    }

    public void AimThisWay ()
    {
        //okay lets get the direction first from the movement which i guess is velocity
        //so now i need the angle from the 0 angle towards the velocity point?
        //im already giving myself a headache here WAIT THERES THE LOOKAT IN THE TRANSFORM STUFF
        Vector3 target = velocity;
        target = transform.position + target;
        transform.LookAt(target);
        //okay so the sprite dissapears, i think because its handling a vector 3 as lookat, but well get there
        //first figuring out how lookat works
        //okay WHY, does everything on the 2D plane rotate around the Z axis, thats silly

        //keely im about to cry, this makes no sense to me. how am i supposed to understand euler
        //i give on this task
    }

    //public void ChasedTest(GameObject orb)
    //{
    //    SpriteRenderer sprite = orb.GetComponent<SpriteRenderer>();
    //    sprite.color = Color.white;
    //}

    //yay testing time
    //i hate myself
    //public List<GameObject> testDestroylist;

    //void TestDestroy (Transform objectTransform)
    //{
    //    Debug.Log(testDestroylist.Count);
    //    for (int i = 0; i < testDestroylist.Count;i++)
    //    {
    //        if (testDestroylist[i].transform == objectTransform)
    //        {
    //            testDestroylist.RemoveAt(i);
    //        }
    //    }
    //    Debug.Log (testDestroylist.Count);
    //}

    void Update()
    {
        PlayerMovement();
        //for (int i = 0; i < testDestroylist.Count; i++)
        //{
        //    Debug.Log(testDestroylist[i].name);
        //}
        //if (testDestroylist.Count == 4)
        //{
        //    TestDestroy(testDestroylist[1].transform);
        //}
        //else
        //{
        //}
        //above commented out stuff was all me testing how removeat worked

        //You will wanna uncomment the below to see how i tried to do what i did
        //its just commented out so i can work on my next task
        //SpawnBombTrail();
        DeflectShield();
    }

}
