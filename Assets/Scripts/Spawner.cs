using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoidSettings
{
    public int velocity = 24;
    public int neighborDist = 10;
    public int nearDist = 4;
    public int attractPushDist = 5;

    [Header("These are floats")]
    public float velMatching = 1.5f;
    public float flockCentering = 1f;
    public float nearAvoid = 2f;
    public float attractPull = 1f;
    public float attractPush = 20f;
    public float verticalRecoveryForce = 1.25f;
    public float obstacleAvoidanceWeight = 4000f;
    public float obstacleAvoidanceDistance = 30f;   

    [Header("How quickly the boid turns")]
    public float velocityEasing = 0.08f;
}

public class Spawner : MonoBehaviour
{
    static public BoidSettings SETTINGS;
    static public List<Boid> BOIDS;

    [Header("Inscribed: settings for spawning boids")]
    public GameObject boidPrefab;
    public Transform boidAnchor;
    public int numBoids = 100;
    public float spawnRadius = 100f;
    public float spawnDelay = 0.1f;
    public BoidSettings boidSettings;

    void Awake()
    {
        Spawner.SETTINGS = boidSettings;
        BOIDS = new List<Boid>();

        InstantiateBoid();

    }

    public void InstantiateBoid()
    {
        GameObject go = Instantiate<GameObject>(boidPrefab);
        Vector3 newPosition = Random.insideUnitSphere * spawnRadius;
        newPosition.y = Mathf.Clamp(newPosition.y, 15f, 35f);
        go.transform.position = newPosition;
        Boid b = go.GetComponent<Boid>();
        b.transform.SetParent(boidAnchor);
        BOIDS.Add(b);

        if (BOIDS.Count < numBoids)
        {
            Invoke("InstantiateBoid", spawnDelay);
        }
    }
}
