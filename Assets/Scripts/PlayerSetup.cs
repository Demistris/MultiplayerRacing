using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private CarMovement _carMovement;
    [SerializeField] private GameObject _camera;

    private void Start()
    {
        if (photonView.IsMine)
        {
            _carMovement.enabled = true;
            _camera.SetActive(true);
            return;
        }

        _carMovement.enabled = false;
        _camera.SetActive(false);
    }
}
