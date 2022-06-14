using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _playerNameInput;

    #region UI Callback Methods

    public void OnLoginButtonClicked()
    {
        string playerName = _playerNameInput.text;

        if(string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Player name is invalid.");
            return;
        }

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #endregion

    #region Photon Callbacks

    public override void OnConnected()
    {
        Debug.Log("Connected to internet.");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon.");
    }

    #endregion
}
