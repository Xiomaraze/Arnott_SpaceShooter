using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    Vector3 velocity;
    public float speed;
    public Transform player;

    public void EnemyMovement()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        velocity = direction * speed * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }

    private void Update()
    {
        EnemyMovement();
        Debug.Log(transform.position);
    }

}
