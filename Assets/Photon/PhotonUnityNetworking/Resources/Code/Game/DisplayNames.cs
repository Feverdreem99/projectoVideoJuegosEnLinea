using UnityEngine;
using Photon.Pun;
using TMPro;

public class DisplayNames : MonoBehaviourPunCallbacks
{


    public TMP_Text player1Text;
    public TMP_Text player2Text;

    void Start()
    {
        UpdateNames();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdateNames();
    }

    void UpdateNames()
    {
        var players = PhotonNetwork.PlayerList;

        if (players.Length > 0)
            player1Text.text = players[0].NickName;

        if (players.Length > 1)
            player2Text.text = players[1].NickName;
    }
}
