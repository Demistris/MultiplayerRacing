using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    private float _damage;

    public void Initialize(Vector3 direction, float speed, float damage)
    {
        transform.forward = direction;
        _rigidbody.velocity = direction * speed;
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PhotonView photonView = other.gameObject.GetComponent<PhotonView>();

            if (photonView.IsMine)
            {
                photonView.RPC("MakeDamage", RpcTarget.AllBuffered, _damage);
            }
        }

        Destroy(gameObject);
    }
}
