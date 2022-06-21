using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;

public class TakeDamage : MonoBehaviourPun
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject _playerGraphic;
    [SerializeField] private GameObject _playerUI;
    [SerializeField] private GameObject _playerWeaponHolder;
    [SerializeField] private GameObject _deathPanelUIPrefab;
    [SerializeField] private CarMovement _carMovement;
    [SerializeField] private Shooting _shooting;

    [Header("Health")]
    [SerializeField] private float _startHealth = 100f;
    [SerializeField] private Image _healthBar;

    private float _health;
    private GameObject _deathPanelUIGameObject;

    private void Start()
    {
        RegainHealth();
    }

    [PunRPC]
    public void MakeDamage(float damage)
    {
        _health -= damage;

        _healthBar.fillAmount = _health / _startHealth;

        if(_health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        _playerGraphic.SetActive(false);
        _playerUI.SetActive(false);
        _playerWeaponHolder.SetActive(false);

        if(photonView.IsMine)
        {
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        GameObject canvas = GameObject.Find("Canvas");

        if(_deathPanelUIGameObject == null)
        {
            _deathPanelUIGameObject = Instantiate(_deathPanelUIPrefab, canvas.transform);
        }
        else
        {
            _deathPanelUIGameObject.SetActive(true);
        }

        Text respawnTimeText = _deathPanelUIGameObject.transform.Find("RespawnTimeText").GetComponent<Text>();
        
        float respawnTime = 8f;

        respawnTimeText.text = respawnTime.ToString(".00");

        while(respawnTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            respawnTime -= 1f;
            respawnTimeText.text = respawnTime.ToString(".00");

            MovingAndShooting(false);
        }

        _deathPanelUIGameObject.SetActive(false);
        MovingAndShooting(true);

        int randomPosition = Random.Range(-20, 20);
        transform.position = new Vector3(randomPosition, 0f, randomPosition);

        photonView.RPC("RegainHealth", RpcTarget.AllBuffered);
    }

    private void MovingAndShooting(bool activity)
    {
        _carMovement.enabled = activity;
        _shooting.enabled = activity;
    }

    [PunRPC]
    private void RegainHealth()
    {
        _health = _startHealth;
        _healthBar.fillAmount = _health / _startHealth;

        _playerGraphic.SetActive(true);
        _playerUI.SetActive(true);
        _playerWeaponHolder.SetActive(true);
    }
}
