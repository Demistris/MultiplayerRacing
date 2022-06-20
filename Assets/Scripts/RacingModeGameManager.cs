using UnityEngine;
using Photon.Pun;

public class RacingModeGameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _playerPrefabs;
    [SerializeField] private Transform[] _playerPositions;

    private void Start()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;

            if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerRacingGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                Vector3 playerPosition = _playerPositions[actorNumber - 1].position;

                PhotonNetwork.Instantiate(_playerPrefabs[(int)playerSelectionNumber].name, playerPosition, Quaternion.identity);
            }
        }
    }
}
