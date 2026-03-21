using UnityEngine;
using Photon.Pun;

public class StaticBranchBehavior : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    [Header("Ajuste de Profundidad")]

    public float distanciaHaciaAtras = 2f; 

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + distanciaHaciaAtras);

        object[] data = info.photonView.InstantiationData;
        if (data != null && data.Length == 1)
        {
            int side = (int)data[0]; 

            if (side == -1) 
            {
                transform.rotation = Quaternion.Euler(-90, 40, 0); 
            }
            else if (side == 1) 
            {
                transform.rotation = Quaternion.Euler(-90, -40, 0); 
            }
            else
            {
                transform.rotation = Quaternion.Euler(-90, 0, 0);
            }
        }
    }
}