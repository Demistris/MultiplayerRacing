using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private MachineGunBullet _bulletPrefab;
    [SerializeField] private Transform _firePosition;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private DeathRacePlayer _deathRacePlayerProperties;

    private void Update()
    {
        if(Input.GetKey("space"))
        {
            Fire();
        }
    }

    private void Fire()
    {
        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        MachineGunBullet bullet = Instantiate(_bulletPrefab, _firePosition.position, Quaternion.identity);
        bullet.Initialize(ray.direction, _deathRacePlayerProperties.BulletSpeed, _deathRacePlayerProperties.Damage);
    }
}
