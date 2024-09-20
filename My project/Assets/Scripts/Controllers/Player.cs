using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Transform> asteroidTransforms;
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public Transform bombsTransform;

    Vector3 velocity = new Vector3(0.001f, 0f);

    

    void Update()
    {
        transform.position += velocity;
    }

}
