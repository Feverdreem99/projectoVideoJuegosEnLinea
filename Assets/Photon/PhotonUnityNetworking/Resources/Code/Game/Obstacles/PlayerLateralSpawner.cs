using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class PlayerLateralSpawner : MonoBehaviourPun
{
    [Header("Enemigos Voladores")]
    public string[] flyingEnemyPrefabs; 

    [Header("Filas de Generación (Alturas)")]
    public float[] laneYOffsets = { 2f, 4f, 6f }; 
    public float spawnXDistance = 5f; 

    [Header("Tiempos (Modo Frenético)")]
    public float minSpawnTime = 0.5f;
    public float maxSpawnTime = 1.5f;

    private float startX;
    private int lastLane = -1; 

    void Start()
    {
        if (photonView.IsMine)
        {
            startX = transform.position.x;
            StartCoroutine(SpawnRoutine());
        }
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3f); 

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

            int chosenLane = Random.Range(0, laneYOffsets.Length);
            
            if (chosenLane == lastLane) 
            {
                chosenLane = (chosenLane + 1) % laneYOffsets.Length;
            }
            lastLane = chosenLane; 

            int side = Random.Range(0, 2) == 0 ? -1 : 1; 
            
            float spawnX = startX + (spawnXDistance * side);
            
            float spawnY = transform.position.y + laneYOffsets[chosenLane];
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, transform.position.z);

            int flightDirection = side * -1; 
            string randomPrefab = flyingEnemyPrefabs[Random.Range(0, flyingEnemyPrefabs.Length)];

            object[] initData = new object[1]; 
            initData[0] = flightDirection; 

            PhotonNetwork.Instantiate(randomPrefab, spawnPosition, Quaternion.identity, 0, initData);
        }
    }
}