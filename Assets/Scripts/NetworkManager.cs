using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _playerNameInput;
    [SerializeField] private InputField _roomNameInput;

    [SerializeField] private GameObject _loginUIPanel;
    [SerializeField] private GameObject _connectingInfoUIPanel;
    [SerializeField] private GameObject _createRoomUIPanel;
    [SerializeField] private GameObject _creatingRoomInfoUIPanel;
    [SerializeField] private GameObject _gameOptionsUIPanel;
    [SerializeField] private GameObject _insideRoomUIPanel;
    [SerializeField] private GameObject _joinRandomRoomUIPanel;

    private string _gameMode;

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
        _insideRoomUIPanel.SetActive(_insideRoomUIPanel.name.Equals(panelNameToBeActivated));
        _joinRandomRoomUIPanel.SetActive(_joinRandomRoomUIPanel.name.Equals(panelNameToBeActivated));
    }

    public void SetGameMode(string gameMode)
    {
        _gameMode = gameMode;
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

    public void OnCancelButtonClicked(string panelNameToBeActivated)
    {
        ActivatePanel(panelNameToBeActivated);
    }

    public void OnCreateRoomButtonClicked()
    {
        if(_gameMode == null)
        {
            return;
        }

        ActivatePanel(_creatingRoomInfoUIPanel.name);

        string roomName = _roomNameInput.text;

        if(string.IsNullOrEmpty(roomName))
        {
            roomName = "Room " + Random.Range(1000, 10000);
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 3;

        string[] roomPropertiesInLobby = { "gm" }; //gm = game mode

        //Two game modes
        //1. Racing = rc
        //2. Death race = dr

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", _gameMode } };

        roomOptions.CustomRoomPropertiesForLobby = roomPropertiesInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void OnJoinRandomRoomButtonClicked(string gameMode)
    {
        _gameMode = gameMode;

        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", gameMode } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
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

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + ". Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);

        ActivatePanel(_insideRoomUIPanel.name);

        if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gm"))
        {
            object gameModeName;

            if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gm", out gameModeName))
            {
                Debug.Log(gameModeName.ToString());
            }
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join failed. " + message);
        OnCreateRoomButtonClicked();
    }

    #endregion
}
