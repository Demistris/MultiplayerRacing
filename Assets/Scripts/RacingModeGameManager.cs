using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RacingModeGameManager : MonoBehaviour
{
    public static RacingModeGameManager Instance = null;
    public Text TimeUIText => _timeUIText;
    public GameObject[] FinishOrderUIGameObjects => _finishOrderUIGameObjects;
    public List<GameObject> LapTriggers => _lapTriggers;

    [SerializeField] private GameObject[] _playerPrefabs;
    [SerializeField] private Transform[] _playerPositions;
    [SerializeField] private GameObject[] _finishOrderUIGameObjects;
    [SerializeField] private List<GameObject> _lapTriggers;
    [SerializeField] private Text _timeUIText;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

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

        foreach(GameObject go in _finishOrderUIGameObjects)
        {
            go.SetActive(false);
        }
    }
}
