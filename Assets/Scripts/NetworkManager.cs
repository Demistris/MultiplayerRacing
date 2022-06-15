using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _playerNameInput;
    [SerializeField] private InputField _roomNameInput;
    [SerializeField] private Text _roomInfoText;
    [SerializeField] private PlayerListEntryInitializer _playerListPrefab;
    [SerializeField] private GameObject _playerListContent;

    [Header("Panels")]
    [SerializeField] private GameObject _loginUIPanel;
    [SerializeField] private GameObject _connectingInfoUIPanel;
    [SerializeField] private GameObject _createRoomUIPanel;
    [SerializeField] private GameObject _creatingRoomInfoUIPanel;
    [SerializeField] private GameObject _gameOptionsUIPanel;
    [SerializeField] private GameObject _insideRoomUIPanel;
    [SerializeField] private GameObject _joinRandomRoomUIPanel;

    private string _gameMode;
    private Dictionary<int, GameObject> _playerListGameObjects;

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

    private void SetupPlayerListGameObject(Player player)
    {
        PlayerListEntryInitializer playerListGameObject = Instantiate(_playerListPrefab);
        playerListGameObject.transform.SetParent(_playerListContent.transform);
        playerListGameObject.transform.localScale = Vector3.one;
        playerListGameObject.Initialize(player.ActorNumber, player.NickName);

        _playerListGameObjects.Add(player.ActorNumber, playerListGameObject.gameObject);
    }

    private void ShowRoomTextInfo()
    {
        _roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + ". Players/Max players: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
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

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
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
            ShowRoomTextInfo();

            if (_playerListGameObjects == null)
            {
                _playerListGameObjects = new Dictionary<int, GameObject>();
            }

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                SetupPlayerListGameObject(player);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetupPlayerListGameObject(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ShowRoomTextInfo();

        Destroy(_playerListGameObjects[otherPlayer.ActorNumber]);
        _playerListGameObjects.Remove(otherPlayer.ActorNumber);
    }

    public override void OnLeftRoom()
    {
        ActivatePanel(_gameOptionsUIPanel.name);

        foreach(GameObject playerListGameObject in _playerListGameObjects.Values)
        {
            Destroy(playerListGameObject);
        }

        _playerListGameObjects.Clear();
        _playerListGameObjects = null;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join failed. " + message);
        OnCreateRoomButtonClicked();
    }

    #endregion
}
