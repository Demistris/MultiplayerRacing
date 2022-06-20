using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] _selectablePlayers;
    [SerializeField] private int _playerSelectionNumber = 0;

    private void Start()
    {
        ActivatePlayer(_playerSelectionNumber);
    }

    public void NextPlayer()
    {
        _playerSelectionNumber += 1;

        if(_playerSelectionNumber >= _selectablePlayers.Length)
        {
            _playerSelectionNumber = 0;
        }

        ActivatePlayer(_playerSelectionNumber);
    }

    public void PreviousPlayer()
    {
        _playerSelectionNumber -= 1;

        if (_playerSelectionNumber < 0)
        {
            _playerSelectionNumber = _selectablePlayers.Length - 1;
        }

        ActivatePlayer(_playerSelectionNumber);
    }

    private void ActivatePlayer(int x)
    {
        foreach(GameObject selectablePlayer in _selectablePlayers)
        {
            selectablePlayer.SetActive(false);
        }

        _selectablePlayers[x].SetActive(true);
    }
}
