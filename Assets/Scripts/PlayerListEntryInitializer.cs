using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerListEntryInitializer : MonoBehaviour
{
    [SerializeField] private Text _playerNameText;
    [SerializeField] private Button _playerReadyButton;
    [SerializeField] private Image _playerReadyImage;

    public void Initialize(int playerID, string playerName)
    {
        _playerNameText.text = playerName;
    }
}
