using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private Neighborhood neighborhood;
    private Rigidbody rigid;

    void Awake()
    {
        neighborhood = GetComponent<Neighborhood>();
        rigid = GetComponent<Rigidbody>();
        vel = Random.onUnitSphere * Spawner.SETTINGS.velocity;

        LookAhead();
        Colorize();
    }
    
    private void FixedUpdate()
    {
        BoidSettings bSet = Spawner.SETTINGS;
        Vector3 sumVel = Vector3.zero;
        int nearAvoidDistance = 10;
        
        Vector3 delta = Attractor.POS - pos;

        if (delta.magnitude > bSet.attractPushDist)
        {
            sumVel += delta.normalized * bSet.attractPull;
        }
        else 
        {
            sumVel -= delta.normalized *bSet.attractPush;
        }

    Vector3 velAvoid = Vector3.zero;
    float avoidDistance = bSet.obstacleAvoidanceDistance; 
    LayerMask obstacleLayer = LayerMask.GetMask("Obstacle"); 

    Collider[] obstacles = Physics.OverlapSphere(transform.position, avoidDistance, obstacleLayer);

    foreach (Collider obstacle in obstacles)
    {
        Vector3 avoidanceDirection = transform.position - obstacle.transform.position;
        if (avoidanceDirection != Vector3.zero)
        {
            avoidanceDirection.Normalize();
        }
        velAvoid += avoidanceDirection;
    }
        Vector3 velAlign = neighborhood.avgVel;
        if (velAlign != Vector3.zero)
        {
            sumVel += velAlign.normalized * bSet.velMatching;
        }

        Vector3 velCenter = neighborhood.avgPos;
        if (velCenter != Vector3.zero)
        {
            velCenter -= transform.position;
            sumVel += velCenter.normalized * bSet.flockCentering;
        }

        if (pos.y < 12.0f)
        {
            float verticalForce = (12.0f - pos.y) * bSet.verticalRecoveryForce;
            vel += new Vector3(0.0f, verticalForce, 0.0f);
        }

        sumVel += velAvoid * bSet.obstacleAvoidanceWeight;
        sumVel.Normalize();
        vel = Vector3.Lerp(vel.normalized, sumVel, bSet.velocityEasing);
        vel *= bSet.velocity;

        LookAhead();
    }

    public Vector3 vel
    {
        get { return rigid.velocity; }
        private set { rigid.velocity = value; }
    }

    public Vector3 pos
    {
        get { return transform.position; }
        private set { transform.position = value; }
    }

    void LookAhead()
    {
        transform.LookAt(pos + rigid.velocity);
    }

    void Colorize()
    {
        Color randColor = Random.ColorHSV(0, 1, 0.5f, 1, 0.5f, 1);
        Renderer [] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer r in rends)
        {
            r.material.color = randColor;
        }

        TrailRenderer tRend = GetComponent<TrailRenderer>();
        tRend.startColor = randColor;
        randColor.a = 0;
        tRend.endColor = randColor;
        tRend.endWidth = 0;
    }

}
