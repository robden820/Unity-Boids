using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    public float speed;
    public float turnSpeed = 4f;
    public float collisionDistance = 3f;
    public float viewRadius = 240f;
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
                TurnBoid();
                GameObject hitBoid = hit.transform.gameObject;
                if (hitBoid.tag == "boid")
                {
                    MatchOrientation(hitBoid);
                    nearbyBoids.Add(hitBoid);
                }
            }
        }
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        CentreFlock(nearbyBoids);
    }

    void TurnBoid()
    {
            float angle = 10.0f * Random.Range(-1.0f, 1.0f);
            transform.Rotate(0.0f, angle, 0.0f, Space.Self);
    }

    void MatchOrientation(GameObject hitBoid)
    {
        Quaternion startOrientation = transform.rotation;
        Quaternion targetOrientation = hitBoid.transform.rotation;

        transform.rotation = Quaternion.RotateTowards(startOrientation, targetOrientation, turnSpeed);

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

        Vector3 direction = Vector3.RotateTowards(transform.forward, target - transform.position, speed * Time.deltaTime, 0.0f);
        Debug.DrawRay(transform.position, direction, Color.blue);
        transform.rotation = Quaternion.LookRotation(direction);
        
    }

    public void setSpeed(float s) => speed = s;

    void OnTriggerExit(Collider container)
    {
        Vector3 position = transform.position;
        if (Mathf.Abs(position.z) > 5){
            position.z *= -1;
        }
        if (Mathf.Abs(position.x) > 5){
            position.x *= -1;
        }

        transform.position = position;
    }
}
