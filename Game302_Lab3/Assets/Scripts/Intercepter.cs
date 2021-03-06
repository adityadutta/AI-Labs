﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intercepter : MonoBehaviour {
    public GameObject target;
    public float speed;
    public float maxAngularSpeed;
    Vector3 velocity = Vector3.zero;
    Vector3 orientation = Vector3.up;

    // Use this for initialization
    void Start () {
                
    }
	
	// Update is called once per frame
	void Update () {
        // To do: Complete Update() to intercept the target
        // * recommended steps: 
        // 1. implement the chasing that you did in Lab3.1
        float dt = Time.deltaTime;

        // To do: Get the direction(dir) towards the target. The direction should be a unit vector.
        Vector3 dir = target.transform.position - transform.position;
        dir = dir.normalized;


        //orientation = dir;

        Vector3 pos = transform.position;
        Vector3 velocity = orientation * speed;
        // To do: Update the current position (pos) using S_final = S_initial + v * t
        //pos += velocity * dt;

        Vector3 vr = target.GetComponent<Meteor>().velocity - velocity;
        Vector3 sr = target.transform.position - transform.position;
        float time = sr.magnitude / vr.magnitude;
        Vector3 st = target.transform.position + target.GetComponent<Meteor>().velocity * time;
        dir = st - transform.position;

        // 2. limit the rotation (orientation). The rotation angle cannot be over the max angle based on maxAngularSpeed.
        // Update orientation 
        float desiredAngle = Mathf.Acos(Vector3.Dot(dir, orientation) / (dir.magnitude * orientation.magnitude)) * Mathf.Rad2Deg;
        //Debug.Log(desiredAngle);
        float angularVelocity = maxAngularSpeed * dt;

        if (desiredAngle < angularVelocity)
        {
            orientation = dir;
        }
        else
        {

            Vector3 a = new Vector3(-orientation.y, orientation.x, orientation.z);

            if (Vector3.Dot(a, dir) > 0)
            {
                angularVelocity *= 1;
            }
            else if (Vector3.Dot(a, dir) < 0)
            {
                angularVelocity *= -1;
            }

            Vector3 rotatedOrientation = Quaternion.Euler(0, 0, angularVelocity) * orientation;
            orientation = rotatedOrientation;
            orientation = orientation.normalized;
        }


        // 3. calculate the predicted moving position and chase the predicted position instead of the actual position of the target.
        velocity = dir.normalized * speed;
        pos += velocity * dt;

        UpdateOrientation();
        transform.position = pos;

    }

    void UpdateOrientation()
    {
        Vector3 angle = new Vector3(0, 0, -Mathf.Atan2(orientation.x, orientation.y) * Mathf.Rad2Deg);
        transform.eulerAngles = angle;
    }
}
