using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerListEntryInitializer : MonoBehaviour
{
    [SerializeField] private Text _playerNameText;
    [SerializeField] private Button _playerReadyButton;
    [SerializeField] private Text _playerReadyText;
    [SerializeField] private Image _playerReadyImage;

    private bool _isPlayerReady;
    private ExitGames.Client.Photon.Hashtable _initialProperties;
    private ExitGames.Client.Photon.Hashtable _newProperties;

    public void Initialize(int playerID, string playerName)
    {
        _playerNameText.text = playerName;

        if(PhotonNetwork.LocalPlayer.ActorNumber != playerID)
        {
            _playerReadyButton.gameObject.SetActive(false);
            return;
        }

        SetupCustomProperties(_initialProperties);

        _playerReadyButton.onClick.AddListener(() =>
        {
            _isPlayerReady = !_isPlayerReady;
            SetPlayerReady(_isPlayerReady);

            SetupCustomProperties(_newProperties);
        });
    }

    private void SetupCustomProperties(ExitGames.Client.Photon.Hashtable propertiesName)
    {
        propertiesName = new ExitGames.Client.Photon.Hashtable() { { MultiplayerRacingGame.PLAYER_READY, _isPlayerReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(propertiesName);
    }

    public void SetPlayerReady(bool isPlayerReady)
    {
        _playerReadyImage.enabled = isPlayerReady;

        if(isPlayerReady)
        {
            _playerReadyText.text = "Ready!";
            return;
        }

        _playerReadyText.text = "Ready?";
    }
}
