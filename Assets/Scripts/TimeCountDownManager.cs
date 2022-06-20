using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TimeCountDownManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private CarMovement _carMovement;

    private Text _timeUIText;
    private float _timeToStartRace = 5f;

    private void Awake()
    {
        _timeUIText = RacingModeGameManager.Instance.TimeUIText;
    }

    private void Update()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (_timeToStartRace < 0f)
        {
            photonView.RPC("StartTheRace", RpcTarget.AllBuffered);
            return;
        }

        _timeToStartRace -= Time.deltaTime;
        photonView.RPC("SetTime", RpcTarget.AllBuffered, _timeToStartRace);
    }

    [PunRPC]
    private void SetTime(float time)
    {
        if (time <= 0f)
        {
            _timeUIText.text = "";
            return;
        }

        _timeUIText.text = time.ToString("F1");
    }

    [PunRPC]
    private void StartTheRace()
    {
        _carMovement.EnableControls(true);
        this.enabled = false;
    }
}
