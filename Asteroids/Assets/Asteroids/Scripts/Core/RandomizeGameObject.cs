using UnityEngine;

    public class RandomizeGameObject : GameObjectBehaviour
    {
        [SerializeField]
        [Range(0, 200)]
        protected int destructionScore = 100;

        [SerializeField]
        float initialForce = 100f;

        [SerializeField]
        float initialTorque = 100f;

#pragma warning disable 0649
        [SerializeField]
        RandomVector3 randomScale;
#pragma warning restore 0649

        #region Spawning
        public virtual void Spawn()
        {
            SpawnRandomize();
            transform.position = FindOpenPosition();
        }

        public virtual void SpawnAt(Vector3 position)
        {
            SpawnRandomize();
            transform.position = position;
        }

        protected virtual void SpawnRandomize()
        {
            transform.localScale = randomScale.Randomize();
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (rigidbody)
            {
                getRigidbody.SetRandomForce(rigidbody, initialForce);
                getRigidbody.SetRandomTorque(rigidbody, initialTorque);
            }
        }

        Vector3 FindOpenPosition(int layerMask = ~0)
        {
            float x = transform.localScale.x;
            float y = transform.localScale.y;
            float collisionSphereRadius = x > y ? x : y;
            bool overlap = false;
            Vector3 openPosition;
            do
            {
                openPosition = Viewport.GetRandomWorldPositionXY();
                overlap = Physics.CheckSphere(openPosition, collisionSphereRadius, layerMask);
            } while (overlap);
            return openPosition;
        }
        #endregion
    }