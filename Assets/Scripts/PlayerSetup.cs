using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private CarMovement _carMovement;
    [SerializeField] private LapController _lapController;
    [SerializeField] private GameObject _camera;
    [SerializeField] private TextMeshProUGUI _playerNameText;

    private void Start()
    {
        SetPlayerUI();

        if (photonView.IsMine)
        {
            _carMovement.enabled = true;
            _lapController.enabled = true;
            _camera.SetActive(true);
            return;
        }

        _carMovement.enabled = false;
        _lapController.enabled = false;
        _camera.SetActive(false);
    }

    public void DisableMovement()
    {
        _carMovement.enabled = false;
        _camera.transform.parent = null;
    }

    private void SetPlayerUI()
    {
        if(_playerNameText == null)
        {
            return;
        }

        _playerNameText.text = photonView.Owner.NickName;

        if(photonView.IsMine)
        {
            _playerNameText.gameObject.SetActive(false);
        }
    }
}
