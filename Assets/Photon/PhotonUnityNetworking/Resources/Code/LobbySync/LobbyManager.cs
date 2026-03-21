using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Elementos de la UI")]
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    
    public TextMeshProUGUI statusText; 
    public TMP_InputField codeInputField; 

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        statusText.text = "Conectando al servidor...";
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        statusText.text = "Conectado. Elige una opción.";
    }

    public void CreateRoom()
    {
        string roomCode = Random.Range(1000, 9999).ToString(); 
        RoomOptions options = new RoomOptions { MaxPlayers = 2 }; 
        PhotonNetwork.CreateRoom(roomCode, options);
        statusText.text = "Creando sala...";
    }

    public void JoinRoomWithCode()
    {
        string joinCode = codeInputField.text;
        if (!string.IsNullOrEmpty(joinCode))
        {
            PhotonNetwork.JoinRoom(joinCode);
            statusText.text = "Buscando sala " + joinCode + "...";
        }
        else
        {
            statusText.text = "Por favor, ingresa un código válido.";
        }
    }

    public override void OnJoinedRoom()
    {
        statusText.text = "Sala: " + PhotonNetwork.CurrentRoom.Name;
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        statusText.text = "Error: Código incorrecto o sala llena.";
    }
}