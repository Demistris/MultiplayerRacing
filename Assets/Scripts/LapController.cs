using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LapController : MonoBehaviourPun
{
    [SerializeField] private PlayerSetup _playerSetup;

    private List<GameObject> _lapTriggers = new List<GameObject>();
    private int _finishOrder = 0;

    public enum RaiseEventsCode
    {
        FinishedEventCode = 0
    }

    private void Start()
    {
        foreach(GameObject lapTrigger in RacingModeGameManager.Instance.LapTriggers)
        {
            _lapTriggers.Add(lapTrigger);
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == (byte)RaiseEventsCode.FinishedEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            string finishedPlayerNickName = (string)data[0];

            _finishOrder = (int)data[1];

            int viewID = (int)data[2];

            GameObject orderUITextGameObject = RacingModeGameManager.Instance.FinishOrderUIGameObjects[_finishOrder - 1];
            orderUITextGameObject.SetActive(true);

            if(viewID == photonView.ViewID)
            {
                orderUITextGameObject.GetComponent<Text>().text = _finishOrder + ". " + finishedPlayerNickName + " (YOU)";
                orderUITextGameObject.GetComponent<Text>().color = Color.red;
                return;
            }

            orderUITextGameObject.GetComponent<Text>().text = _finishOrder + ". " + finishedPlayerNickName;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_lapTriggers.Contains(other.gameObject))
        {
            int triggerIndex = _lapTriggers.IndexOf(other.gameObject);
            _lapTriggers[triggerIndex].SetActive(false);

            if(other.name == "FinishTrigger")
            {
                GameFinished();
            }
        }
    }

    private void GameFinished()
    {
        _playerSetup.DisableMovement();

        _finishOrder += 1;

        string nickName = photonView.Owner.NickName;
        int viewID = photonView.ViewID;
        object[] data = new object[] { nickName, _finishOrder, viewID};

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOptions = new SendOptions
        {
            Reliability = false
        };

        PhotonNetwork.RaiseEvent((byte)RaiseEventsCode.FinishedEventCode, data, raiseEventOptions, sendOptions);
    }
}
