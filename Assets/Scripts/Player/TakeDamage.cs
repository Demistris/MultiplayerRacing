using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] private float _startHealth = 100f;
    [SerializeField] private Image _healthBar;

    private float _health;

    private void Start()
    {
        _health = _startHealth;
        _healthBar.fillAmount = _health / _startHealth;
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

    }
}
