using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    public float speed = 2.0f;
    Vector3 velocity;

    public GameObject target;
    public float maxAngularSpeed;

    public Transform rayCastPoint1;
    public Transform rayCastPoint2;
    public float avoidanceDistance = 5.0f;
    public float rayCastRange = 1.0f;
    public float Timer = 5.0f;

    private bool ray1Active = false;
    private bool ray2Active = false;
    private bool avoid = false;

    Vector3 targetDir = Vector3.zero;
    Vector3 WallHitTarget = Vector3.zero;

    Vector3 orientation = Vector3.up;

    private bool missileLaunched = false;

    // Use this for initialization
    void Start()
    {
        
    }

    //// Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            missileLaunched = true;
            GameManager.Instance.PlayMissileLaunchSound();
        }

        if (!missileLaunched)
        {
            Vector3 direction = Vector3.zero;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                direction.x = 1;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                direction.x = -1;
            }

            float dt = Time.deltaTime;
            Vector3 pos = transform.position;
            velocity = direction.normalized * speed;
            pos += velocity * dt;
            transform.position = pos;
        }
        else
        {
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Enemy");
            }

            if (avoid == false)
            {
                targetDir = target.transform.position - transform.position;
            }
            else
            {
                targetDir = WallHitTarget - transform.position;
            }

            if (avoid == true)
            {
                if (targetDir.magnitude < 1.0)
                {
                    avoid = false;
                }
            }

            LaunchMissile();

            CheckWall();
        }
    }


    void LaunchMissile()
    {
        float dt = Time.deltaTime;

        // To do: Get the direction(dir) towards the target. The direction should be a unit vector.
        Vector3 dir = targetDir;
        dir = dir.normalized;


        //orientation = dir;

        Vector3 pos = transform.position;
        Vector3 velocity = orientation * speed;
        // To do: Update the current position (pos) using S_final = S_initial + v * t
        pos += velocity * dt;

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

        UpdateOrientation();

        transform.position = pos;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    void UpdateOrientation()
    {
        Vector3 angle = new Vector3(0, 0, -Mathf.Atan2(orientation.x, orientation.y) * Mathf.Rad2Deg);
        transform.eulerAngles = angle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyMissile();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            GameManager.Instance.AddScore();
            Destroy(collision.gameObject);
            GameManager.Instance.RespawnEnemy();
            DestroyMissile();
        }
    }

    private void DestroyMissile()
    {
        GameManager.Instance.PlayMissileDestroySound();
        GameManager.Instance.RespawnMissile();
        Destroy(this.gameObject);
    }


    private void CheckWall()
    {
        Debug.DrawRay(rayCastPoint1.transform.position, orientation * rayCastRange, Color.blue);
        Debug.DrawRay(rayCastPoint2.transform.position, orientation * rayCastRange, Color.blue);

        RaycastHit2D ray1Hit = Physics2D.Raycast(rayCastPoint1.transform.position, orientation, rayCastRange);
        RaycastHit2D ray2Hit = Physics2D.Raycast(rayCastPoint2.transform.position, orientation, rayCastRange);

        if (ray1Hit == true && ray2Active == false)
        {
            Debug.Log(ray1Hit.collider.tag);
            if (ray1Hit.collider.tag ==  "Obstacle")
            {
                Vector3 point = ray1Hit.point + ray1Hit.normal;
                avoid = true;
                WallHitTarget = point;
                Debug.DrawRay(ray1Hit.point, point * rayCastRange, Color.green, 5.0f);
                ray1Active = true;
            }
        }

        if (ray2Hit == true && ray1Active == false)
        {
            Debug.Log(ray2Hit.collider.tag);
            if (ray2Hit.collider.CompareTag("Obstacle"))
            {
                Vector3 point = ray2Hit.point + ray2Hit.normal;
                avoid = true;
                WallHitTarget = point;
                Debug.DrawRay(ray2Hit.point, point * rayCastRange, Color.green, 5.0f);
                ray2Active = true;
            }
        }

        if (ray1Active == true || ray2Active == true)
        {
            Timer -= Time.deltaTime;
        }

        if (Timer <= 0)
        {
            Timer = 5;
            ray1Active = false;
            ray2Active = false;
        }
    }
}
