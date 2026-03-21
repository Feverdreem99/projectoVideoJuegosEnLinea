using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class AvatarSelection : MonoBehaviour
{
    [Header("Elementos de la UI")]
    public GameObject avatarSelectionPanel; 
    
    [Header("Modelos 3D en la Escena")]
    public GameObject[] avatar3DModels; 

    private int currentAvatarIndex = 0;

    void Start()
    {
        Update3DModelVisibility(0);
    }

    public void OpenSelectionPanel()
    {
        avatarSelectionPanel.SetActive(true);
    }

    public void ToggleSelectionPanel()
    {
        bool isActive = avatarSelectionPanel.activeSelf;
        avatarSelectionPanel.SetActive(!isActive);
    }

    public void CloseSelectionPanel()
    {
        avatarSelectionPanel.SetActive(false);
    }

    public void SelectAvatar(int index)
    {
        currentAvatarIndex = index;
        
        Update3DModelVisibility(index);

        SyncAvatarWithPhoton();
        CloseSelectionPanel();
    }

    private void Update3DModelVisibility(int activeIndex)
    {
        for (int i = 0; i < avatar3DModels.Length; i++)
        {
            if (avatar3DModels[i] != null)
            {
                avatar3DModels[i].SetActive(i == activeIndex);
            }
        }
    }

    private void SyncAvatarWithPhoton()
    {
        Hashtable playerProperties = new Hashtable();
        playerProperties["AvatarIndex"] = currentAvatarIndex;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        
        Debug.Log("Avatar elegido y sincronizado: " + currentAvatarIndex);
    }
}