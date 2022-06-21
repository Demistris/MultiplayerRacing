using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class DeathRaceModeGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] _playerPrefabs;

    private void Start()
    {
        if(!PhotonNetwork.IsConnectedAndReady)
        {
            return;
        }

        object playerSelectionNumber;

        if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerRacingGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
        {
            int randomPosition = Random.Range(-15, 15);

            PhotonNetwork.Instantiate(_playerPrefabs[(int)playerSelectionNumber].name, new Vector3(randomPosition, 0f, randomPosition), Quaternion.identity);
        }    
    }

    public void OnQuitMatchButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
