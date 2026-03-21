using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable; 

public class WindManager : MonoBehaviourPunCallbacks
{
    public static WindManager Instance;

    [Header("Configuración del Viento")]
    public float minTiempoEntreVientos = 5f;
    public float maxTiempoEntreVientos = 12f;
    public float duracionViento = 4f;
    public float fuerzaViento = 20f; 

    [Header("UI del Viento (Canvas)")]
    public GameObject panelViento; 
    public TextMeshProUGUI textoDireccion;    

    [HideInInspector] public float direccionVientoActual = 0f; 

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (panelViento != null) panelViento.SetActive(false);
        if (textoDireccion != null) textoDireccion.text = "";

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(CicloDeViento());
        }
    }

    System.Collections.IEnumerator CicloDeViento()
    {
        while (true)
        {
            float tiempoEspera = Random.Range(minTiempoEntreVientos, maxTiempoEntreVientos);
            yield return new WaitForSeconds(tiempoEspera);

            int dir = Random.Range(0, 2) == 0 ? -1 : 1;
            ActualizarVientoEnRed(dir);

            yield return new WaitForSeconds(duracionViento);

            ActualizarVientoEnRed(0);
        }
    }

    void ActualizarVientoEnRed(int direccion)
    {
        if (PhotonNetwork.InRoom)
        {
            Hashtable hash = new Hashtable();
            hash.Add("WindDir", direccion);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("WindDir"))
        {
            int dir = (int)propertiesThatChanged["WindDir"];
            direccionVientoActual = dir;

            if (dir == 0)
            {
                if (panelViento != null) panelViento.SetActive(false);
                if (textoDireccion != null) textoDireccion.text = "";
            }
            else
            {
                if (panelViento != null) panelViento.SetActive(true);
                if (textoDireccion != null)
                {
                    if (dir == 1)
                    {
                        textoDireccion.text = "VIENTO: >>>";
                        textoDireccion.color = Color.red; 
                    }
                    else
                    {
                        textoDireccion.text = "<<< :VIENTO";
                        textoDireccion.color = Color.blue;
                    }
                }
            }
        }
    }
}