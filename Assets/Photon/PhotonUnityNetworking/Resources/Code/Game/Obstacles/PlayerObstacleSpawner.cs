using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class PlayerObstacleSpawner : MonoBehaviourPun
{
    [Header("Configuración de Obstáculos")]
    public string[] obstaclePrefabNames;

    [Header("Filas de Generación (Relativas al Jugador)")]
    public float[] laneOffsets = { -2f, 0f, 2f }; 
    public float spawnHeightOffset = 15f; 

    [Header("Tiempos")]
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 2.5f;

    private float startX;

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

            int obstaclesToSpawn = Random.Range(1, 3); 
            List<int> availableLanes = new List<int> { 0, 1, 2 };

            for (int i = 0; i < obstaclesToSpawn; i++)
            {
                int randomIndex = Random.Range(0, availableLanes.Count);
                int chosenLane = availableLanes[randomIndex];
                availableLanes.RemoveAt(randomIndex);

                float xPos = startX + laneOffsets[chosenLane];
                Vector3 spawnPosition = new Vector3(xPos, transform.position.y + spawnHeightOffset, transform.position.z);

                string randomPrefab = obstaclePrefabNames[Random.Range(0, obstaclePrefabNames.Length)];

                PhotonNetwork.Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}