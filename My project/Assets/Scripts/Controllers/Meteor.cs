using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Meteor : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 direction = new Vector3(-1, -1);
    Vector2 target;
    public float speed;
    void Start()
    {
        
    }

    void Travel()
    {
        target = transform.position + direction;
        transform.position = Vector2.Lerp(transform.position, target, speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        Travel();
    }
}
