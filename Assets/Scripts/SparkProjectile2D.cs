using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SparkProjectile2D : MonoBehaviour
{ 
        [Header("Lifetime")]
        public float lifetime = 6f;
        
        [Header("FX (optional)")]
        public ParticleSystem hitFX;
        
        Vector2 _direction = Vector2.right;
        private float _speed = 10f;
        private float _dieAt;

        public void Initialize(Vector2 dir, float speed)
        {
                _direction = dir.normalized;
                _speed = speed;
                _dieAt = Time.time + lifetime;
        }

        void OnEnable()
        {
                if (_dieAt <= 0f) _dieAt = Time.time + lifetime;
        }

        void Update()
        {
                transform.position += (Vector3)(_direction * _speed * Time.deltaTime);
                if (Time.time >= _dieAt) Destroy(gameObject);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
                //if hit player -> resart level
                if (other.CompareTag("Player"))
                {
                        var respawn = other.GetComponent<PlayerRespawn>();
                        if (respawn != null)
                        {
                                SpawnHitFX();
                                Destroy(gameObject);
                                respawn.KillAndRespawn();
                                return;
                        }
                }
                
                //if hit solid world ->destroy
                if (!other.isTrigger)
                {
                        SpawnHitFX();
                        Destroy(gameObject);
                }
        }

        void SpawnHitFX()
        {
                if (hitFX)
                {
                        var fx = Instantiate(hitFX, transform.position, Quaternion.identity);
                        fx.Play();
                        Destroy(fx.gameObject, 1.5f);
                }
        }
}