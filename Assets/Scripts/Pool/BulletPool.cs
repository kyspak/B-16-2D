using Combat;
using UnityEngine;


namespace Pool
{
    public class BulletPool : MonoBehaviour, IPool<Bullet>
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private int _prewarmCount = 20;
        [SerializeField] private int _maxSize = 100;
        [SerializeField] private Transform _container;

        private ObjectPool<Bullet> _pool;

        private void Awake()
        {
            if (!_container)
                _container = transform;

            _pool = new ObjectPool<Bullet>(
                factory: CreateBullet,
                destroyAction: bullet => Destroy(bullet.gameObject),
                prewarmCount: _prewarmCount,
                maxSize: _maxSize);

        }

        public Bullet Get()
        {
            Bullet bullet = _pool.Get();
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        
        public void Release(Bullet bullet) => _pool.Release(bullet);

        private Bullet CreateBullet()
        {
            Bullet bullet = Instantiate(_bulletPrefab, _container);
            bullet.OwnerPool = this;
            return bullet;
        }

        private void OnDestroy() => _pool.ReleaseAll();
    }
}