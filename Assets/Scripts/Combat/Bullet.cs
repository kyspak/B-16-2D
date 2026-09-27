using Pool;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _speed = 30f;
        [SerializeField] private float _lifetime = 3f;
        [SerializeField] private int _damage = 10;

        private Rigidbody2D _rb;
        private float _lifeTimer;
        
        public BulletPool OwnerPool { get; set; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _lifeTimer -= Time.deltaTime;
            if (_lifeTimer <= 0f)
            {
                OwnerPool?.Release(this);
            }
                
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageble damageable))
                damageable.TakeDamage(_damage);
            
            OwnerPool?.Release(this);
        }

        public void OnSpawned()
        {
            _lifeTimer = _lifetime;
        }

        private void OnEnable()
        {
            if(_rb) 
                _rb.linearVelocityX = _speed;
        }

        public void OnDespawned()
        {
            _rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}