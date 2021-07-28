using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] balloons;

    private float timeBtwSpawns;
    public float startTimeBtwSpawns;

    public float minTimeBetweenSpawns;
    public float decrease;
    private Transform randomSpawnPoint;

    int whichSide = 0;

    void Update()
    {
        if (GameManager.Instance.isRockLeft == true)
        {
            if (timeBtwSpawns <= 0)
            {
                whichSide = Random.Range(0, 2);

                if (whichSide == 0) //upperSpawnPoints
                {
                    randomSpawnPoint = spawnPoints[Random.Range(0, 3)];
                }
                else //lowerSpawnPoints
                {
                    randomSpawnPoint = spawnPoints[Random.Range(3, spawnPoints.Length)]; // (3 (inclusive),6 (exclusive))
                }

                GameObject randomBalloon = balloons[Random.Range(0, balloons.Length)];

                Instantiate(randomBalloon, randomSpawnPoint.position, Quaternion.identity);

                if (startTimeBtwSpawns > minTimeBetweenSpawns)
                {
                    startTimeBtwSpawns -= decrease;
                }

                timeBtwSpawns = startTimeBtwSpawns;
            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
    }

    public int getSpawnPointSide()
    {
        return whichSide;
    }
}
