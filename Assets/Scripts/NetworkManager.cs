using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _playerNameInput;
    [SerializeField] private GameObject _loginUIPanel;
    [SerializeField] private GameObject _connectingInfoUIPanel;
    [SerializeField] private GameObject _createRoomUIPanel;
    [SerializeField] private GameObject _creatingRoomInfoUIPanel;
    [SerializeField] private GameObject _gameOptionsUIPanel;
    [SerializeField] private GameObject _insideRoomUIPanel;
    [SerializeField] private GameObject _joinRandomRoomUIPanel;

    private void Start()
    {
        ActivatePanel(_loginUIPanel.name);
    }

    public void ActivatePanel(string panelNameToBeActivated)
    {
        _loginUIPanel.SetActive(_loginUIPanel.name.Equals(panelNameToBeActivated));
        _connectingInfoUIPanel.SetActive(_connectingInfoUIPanel.name.Equals(panelNameToBeActivated));
        _createRoomUIPanel.SetActive(_createRoomUIPanel.name.Equals(panelNameToBeActivated));
        _creatingRoomInfoUIPanel.SetActive(_creatingRoomInfoUIPanel.name.Equals(panelNameToBeActivated));
        _gameOptionsUIPanel.SetActive(_gameOptionsUIPanel.name.Equals(panelNameToBeActivated));
        _joinRandomRoomUIPanel.SetActive(_joinRandomRoomUIPanel.name.Equals(panelNameToBeActivated));
    }

    #region UI Callback Methods

    public void OnLoginButtonClicked()
    {
        string playerName = _playerNameInput.text;

        if(string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Player name is invalid.");
            return;
        }

        ActivatePanel(_connectingInfoUIPanel.name);

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void OnCancelButtonClicked()
    {
        ActivatePanel(_gameOptionsUIPanel.name);
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
        ActivatePanel(_gameOptionsUIPanel.name);
    }

    #endregion
}
