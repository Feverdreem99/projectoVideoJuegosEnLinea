using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerNameInput : MonoBehaviour
{
    public TMP_InputField nameInput;


    public void OnClick_SetNameAndContinue()
    {
        if (!string.IsNullOrEmpty(nameInput.text))
        {
            PhotonNetwork.NickName = nameInput.text;
        }

    }


}
