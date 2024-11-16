using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour
{
    // Start is called before the first frame update
    //okay the mine just got spawned in
    // need to pull it's detection radius
    //and know its target, the enemy
    public float detectionRadius;
    public Transform enemy;
    public GameObject player;
    //should make sure they dont move the speed of light
    public float speed;
    //boom distance here folks
    public float boomDistance;
    float enemyDistance;

    void Start()
    {
        enemyDistance = Vector2.Distance(enemy.position, transform.position);
    }

    void Chase()
    {
        //update the distance between the enemy and the puppy mine
        enemyDistance = Vector2.Distance(enemy.position, transform.position);
        if (enemyDistance <= detectionRadius)
            //if its within the radius (feel like im straight up writting the code to an over eager puppy)
            //i see you? i want to play!
        {
            Movement(true); //reminder: dont name your method and your variable the same thing dummy
            //thats how we ended up with the 4 hour confusion last semester
        }
        else
        {
            Movement(false);
        }
    }

    void Movement (bool chaseThem)
    {
        if (chaseThem)
        {
            transform.position = Vector2.Lerp(transform.position, enemy.position, speed * Time.deltaTime); //yeaaaa scoot your lil booty mine-san
            //i cant believe i just typed that out
        }
        else
        {
            //if you cant see them, sit patiently like a potato please
        }
    }

    //and now, if we catch them

    void Boom ()
    {
        float tempBoomDistance = Vector2.Distance(transform.position, enemy.position);
        if (tempBoomDistance <= boomDistance)
        {
            //and now the boom
            //there needs to be a boom thing, but for now im gonna just delete the mine and remove it from the mine list
            //oh lord help me i have to send a message to another script again
            //this was a nightmare to figure out last year
            player.SendMessage("MineExploded", transform, SendMessageOptions.RequireReceiver);
            Destroy(gameObject);
            //okay i have no idea how to test this now

            //testing got weird
            //but i think it should work
        }
        else
        {
            //well do nothing if its not close enough
        }
    }

    // Update is called once per frame
    void Update()
    {
        //and now we create chaos
        Chase();
        Boom();
        //test the send message thing, madness this is
        //player.SendMessage("ChasedTest", gameObject, SendMessageOptions.RequireReceiver); it works!
    }
}
