using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private CarMovement _carMovement;
    [SerializeField] private Camera _camera;

    private void Start()
    {
        if(photonView.IsMine)
        {
            _carMovement.enabled = true;
            _camera.enabled = true;
            return;
        }

        _carMovement.enabled = false;
        _camera.enabled = false;
    }
}
