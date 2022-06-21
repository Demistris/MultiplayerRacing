using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _firePosition;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private DeathRacePlayer _deathRacePlayerProperties;

    private float _fireRate;
    private float _fireTimer = 0f;

    private void Start()
    {
        _fireRate = _deathRacePlayerProperties.FireRate;
    }

    private void Update()
    {
        if(Input.GetKey("space"))
        {
            if(_fireTimer > _fireRate)
            {
                Fire();
                _fireTimer = 0f;
            }
        }

        if (_fireTimer < _fireRate)
        {
            _fireTimer += Time.deltaTime;
        }
    }

    private void Fire()
    {
        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        Bullet bullet = Instantiate(_bulletPrefab, _firePosition.position, Quaternion.identity);
        bullet.Initialize(ray.direction, _deathRacePlayerProperties.BulletSpeed, _deathRacePlayerProperties.Damage);
    }
}
