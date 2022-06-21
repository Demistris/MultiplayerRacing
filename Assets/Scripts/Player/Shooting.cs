using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _firePosition;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private DeathRacePlayer _deathRacePlayerProperties;
    [SerializeField] private bool _isUsingLaser;
    [SerializeField] private LineRenderer _lineRenderer;

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
        if(_isUsingLaser)
        {
            RaycastHit hit;
            Ray rayLaser = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

            if(Physics.Raycast(rayLaser, out hit, 200))
            {
                if(!_lineRenderer.enabled)
                {
                    _lineRenderer.enabled = true;
                }

                _lineRenderer.startWidth = 0.3f;
                _lineRenderer.endWidth = 0.1f;

                _lineRenderer.SetPosition(0, _firePosition.position);
                _lineRenderer.SetPosition(1, hit.point);

                StopAllCoroutines();
                StartCoroutine(DisableAfter(0.3f));
            }

            return;
        }

        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        Bullet bullet = Instantiate(_bulletPrefab, _firePosition.position, Quaternion.identity);
        bullet.Initialize(ray.direction, _deathRacePlayerProperties.BulletSpeed, _deathRacePlayerProperties.Damage);
    }

    private IEnumerator DisableAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _lineRenderer.enabled = false;
    }
}
