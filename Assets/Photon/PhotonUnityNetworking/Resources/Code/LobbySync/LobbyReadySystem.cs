using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyReadySystem : MonoBehaviourPunCallbacks
{
    [Header("Elementos de UI")]
    public Button readyButton;
    public TextMeshProUGUI readyButtonText; 
    public TextMeshProUGUI statusText; 

    private bool isReady = false;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true; 
    }

    public void ToggleReady()
    {
        isReady = !isReady; 
        
        if (isReady)
        {
            readyButtonText.text = "¡Esperando...";
            readyButtonText.color = Color.red; 
        }
        else
        {
            readyButtonText.text = "¡Listo!";
            readyButtonText.color = Color.white;
        }

        Hashtable props = new Hashtable();
        props.Add("IsReady", isReady);
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        
        CheckAllPlayersReady();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("IsReady"))
        {
            CheckAllPlayersReady();
        }
    }

    private void CheckAllPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (PhotonNetwork.CurrentRoom.PlayerCount < 2) return;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue("IsReady", out isPlayerReady))
            {
                if (!(bool)isPlayerReady) 
                {
                    return; 
                }
            }
            else
            {
                return; 
            }
        }

        statusText.text = "¡Todos listos! Iniciando partida...";
        
        PhotonNetwork.LoadLevel("Nivel_Arbol"); 
    }
}