using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed = 10.0f;
    Vector3 velocity;

    Vector3 direction = Vector3.zero;

    bool flipped = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
        float dt = Time.deltaTime;
        Vector3 pos = transform.position;

        Vector3 ls = transform.localScale;

        if (pos.x >= 7.0f)
        {
            if (!flipped)
            {
                flipped = true;
                ls.y *= -1;
                transform.localScale = ls;
            }
           
            direction.x--;
        }
        else if (pos.x <= -7.32f)
        {
            if (flipped)
            {
                flipped = false;
                ls.y *= -1;
                transform.localScale = ls;
            }
           
            direction.x++;
        }

        velocity = direction.normalized * speed;
        pos += velocity * dt;
        transform.position = pos;
    }
}
