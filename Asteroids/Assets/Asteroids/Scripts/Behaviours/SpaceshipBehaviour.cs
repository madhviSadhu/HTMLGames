using UnityEngine;
    public class SpaceshipBehaviour : GameObjectBehaviour
    {
        public enum Weapons { Default, Fast, Backwards, Spread, Count }
        public float thrust = 1000f;
        public float torque = 500f;
        public float maxSpeed = 20f;
        public float powerupThrust = 2000f;
        public float powerupSpeed = 30f;
        public GameObject bulletPrefab;
        public AudioSource hyperAudio;
        public AudioSource shootAudio;
        float thrustInput;
        float turnInput;
        Rigidbody rb;

        [HideInInspector]
        public bool hasThrustPowerup;
        public int activeWeapon = (int)Weapons.Default;
        const int bulletSpeed = 25;

        ObjectPool bulletPool;
        Transform nozzle;
        Vector3 forward() { return nozzle.up; }


        void Reset()
        {
            thrustInput = 0f;
            turnInput = 0f;
        }
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            bulletPool = ObjectPool.Build(bulletPrefab, initialClones: 10, initialCapacity: 10);
            nozzle = transform.Find("BulletSpawnPoint");
        }

        void OnEnable()
        {
            Reset();
            hasThrustPowerup = false;
            activeWeapon = (int)Weapons.Default;
        }

        void Update()
        {
            if (ShipInput.IsShooting() || ShipInput.IsHyperspacing())
            {
                Shoot();
            }
            turnInput = ShipInput.GetTurnAxis();
            thrustInput = ShipInput.GetForwardThrust();
        }

        public void Shoot()
        {
            shootAudio.Play();
            switch (activeWeapon)
            {
                case (int)Weapons.Default:
                    ShootDefault();
                    break;
                case (int)Weapons.Fast:
                    ShootFast();
                    break;
                case (int)Weapons.Backwards:
                    ShootBackwards();
                    break;
                case (int)Weapons.Spread:
                    ShootSpread();
                    break;
                default:
                    ShootDefault();
                    break;
            }
        }


        void FixedUpdate()
        {
            Move();
            Turn();
            ClampSpeed();
        }

        void ClampSpeed()
        {
            float clampSpeed = hasThrustPowerup ? powerupSpeed : maxSpeed;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, clampSpeed);
        }

        void HyperSpace()
        {
            getRigidbody.Reset(rb);
            transform.position = Viewport.GetRandomWorldPositionXY();
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(1, 360));
            hyperAudio.Play();
        }

        void Move()
        {
        float useThrust = hasThrustPowerup ? powerupThrust : thrust;
        Vector3 thrustForce = thrustInput * useThrust * Time.deltaTime * forward();
        rb.AddForce(thrustForce);
        }

        void Turn()
        {
            float turn = turnInput * torque * Time.deltaTime;
            Vector3 zTorque = transform.forward * -turn;
            rb.AddTorque(zTorque);
        }
        void ShootDefault() { FireBullet(forward()); }

        void ShootFast() { FireBullet(forward(), bulletSpeed * 2); }

        void ShootBackwards() { ShootDefault(); FireBullet(-forward()); }

        void ShootSpread()
        {
            for (int i = -1; i <= 1; i++)
            {
                float zDegrees = 15f;
                Vector3 direction = Quaternion.Euler(0, 0, i * zDegrees) * forward();
                FireBullet(direction);
            }

        }

        void FireBullet(Vector3 direction, float speedScalar = bulletSpeed)
        {
            direction = (direction * speedScalar) + rb.velocity;
            Bullet().Fire(nozzle.position, nozzle.rotation, direction);
        }
        public static SpaceshipBehaviour Spawn(GameObject prefab)
        {
            GameObject clone = Instantiate(prefab);
            var existingShip = clone.GetComponent<SpaceshipBehaviour>();
            return existingShip ? existingShip : clone.AddComponent<SpaceshipBehaviour>();
        }

        public virtual void Recover()
        {
            if (!IsAlive)
            {
                ResetTransform();
                gameObject.SetActive(true);
                ResetRigidbody();
            }
        }

        public virtual bool IsAlive
        {
            get
            {
                return gameObject.activeSelf;
            }
        }

        public void ResetTransform()
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        public void ResetRigidbody()
        {
            getRigidbody.Reset(GetComponent<Rigidbody>());
        }

        protected override void DefaltDestroy()
        {
            gameObject.SetActive(false);
        }

        public static bool ActiveShield(GameObject ship)
        {
            return ship.transform.Find("Shield").gameObject.activeSelf;
        }

        protected virtual void OnTriggerEnter(Collider bulletCollider)
        {
            if (ActiveShield(gameObject))
            {
                RemoveFromGame(bulletCollider.gameObject);
                return;
            }

            CollisionsBehaviour.exploder.Explode(gameObject.tag, transform.position);
            DefaltDestroy();
        }

        BulletBehaviour Bullet()
        {
            return bulletPool.GetReusable<BulletBehaviour>();
        }
    }