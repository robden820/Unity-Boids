using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    public float speed = 4f;
    public float turnSpeed = 4f;
    public float collisionDistance = 3f;
    public float viewRadius = 280f;
    public float segments = 10f;
    
    void Update()
    {
        Vector3 startPos = transform.position;

        float startAngle = viewRadius * -0.5f;
        float endAngle = viewRadius * 0.5f;
        float increment = viewRadius / segments;

        List<GameObject> nearbyBoids = new List<GameObject>();

        RaycastHit hit;
        for (float i = startAngle; i <= endAngle; i += increment)
        {
            Vector3 targetPos = (Quaternion.Euler(0, i, 0) * transform.forward).normalized * collisionDistance;

            if (Physics.Raycast(startPos, targetPos, out hit, collisionDistance))
            {
                Debug.DrawRay(startPos, targetPos * hit.distance, Color.red);
                
                GameObject hitBoid = hit.transform.gameObject;
                if (hitBoid.tag == "boid")
                {
                    MatchOrientation(hitBoid);
                    nearbyBoids.Add(hitBoid);
                }
                TurnBoid(i);
            }
        }
        
        //CentreFlock(nearbyBoids);
        transform.Translate(Vector3.forward * Time.deltaTime * -speed);
        
    }

    void TurnBoid(float i)
    {
        float turnAngle = 0.25f * turnSpeed;

        if (i > 0)
        {
            turnAngle *= -1;
        }
            transform.Rotate(0.0f, turnAngle, 0.0f, Space.Self);
    }

    void MatchOrientation(GameObject hitBoid)
    {
        Quaternion startOrientation = transform.rotation;
        Quaternion targetOrientation = hitBoid.transform.rotation;

        transform.rotation = Quaternion.RotateTowards(startOrientation, targetOrientation, 0.2f);
    }

    void CentreFlock(List<GameObject> nearbyBoids)
    {
        int numBoids = nearbyBoids.Count;
        Vector3 target = transform.position;

        for (int i = 0; i < numBoids; i++)
        {
            target += nearbyBoids[i].transform.position;
        }
        target = target / (numBoids+1);

        Vector3 direction = Vector3.RotateTowards(transform.forward, target - transform.position, speed * Time.deltaTime * 0.1f, 0.0f);
        Debug.DrawRay(transform.position, direction, Color.blue);
        transform.rotation = Quaternion.LookRotation(direction);
        
    }

    public void setSpeed(float s) => speed = s;
}
