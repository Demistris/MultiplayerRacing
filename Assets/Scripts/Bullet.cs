using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    public void Initialize(Vector3 direction, float speed, float damage)
    {
        transform.forward = direction;
        _rigidbody.velocity = direction * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
