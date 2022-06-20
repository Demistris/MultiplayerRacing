using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LapController : MonoBehaviour
{
    [SerializeField] CarMovement _carMovement;
    [SerializeField] Camera _camera;

    private List<GameObject> _lapTriggers = new List<GameObject>();

    private void Start()
    {
        foreach(GameObject lapTrigger in RacingModeGameManager.Instance.LapTriggers)
        {
            _lapTriggers.Add(lapTrigger);
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
        _carMovement.enabled = false;
        _camera.transform.parent = null;
    }
}
