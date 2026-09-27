using Pool;
using UnityEngine;

namespace Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private BulletPool _bulletPool;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private float _fireRate = 8f;

        private float _nextFireTime;

        public bool TryFire()
        {
            if (Time.time < _nextFireTime)
                return false;

            _nextFireTime = Time.time + 1f / _fireRate;

            Bullet bullet = _bulletPool.Get();
            bullet.transform.SetLocalPositionAndRotation(_muzzle.position, _muzzle.rotation);
            return true;
        }
    }
}