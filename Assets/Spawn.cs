using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public GameObject boid;
    [Range(1, 100)] public int boidNumber = 20;
    [Range(4, 7)] public float speed;
    
    void Start()
    {
        Vector3 start;
        start.y = 0.5f;
        for(int i = 0; i < boidNumber; i++)
        {
            start.x = Random.Range(-2.0f, 2.0f);
            start.z = Random.Range(-2.0f, 2.0f);

            float angle = Random.Range(-180.0f, 180.0f);
            Quaternion rotate = Quaternion.Euler(0, angle, 0);

            Instantiate(boid, start, rotate);
            boid.GetComponent<BoidController>().setSpeed(speed);
        } 
    }
 
}       
