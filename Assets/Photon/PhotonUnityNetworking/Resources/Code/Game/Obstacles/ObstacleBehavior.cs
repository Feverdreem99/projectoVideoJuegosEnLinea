using UnityEngine;
using Photon.Pun;

public class ObstacleBehavior : MonoBehaviourPun
{
    [Header("Movimiento")]
    public float fallSpeed = 8f; 

    [Header("Zona de Destrucción (Límite Inferior)")]
    public float destroyYLimit = -5f; 

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);

        if (photonView.IsMine && transform.position.y < destroyYLimit)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}