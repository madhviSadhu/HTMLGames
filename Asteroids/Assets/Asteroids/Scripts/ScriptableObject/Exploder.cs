using UnityEngine;
using System.Collections;

    [CreateAssetMenu(fileName = "Exploder", menuName = "Exploder")]
    public class Exploder : ScriptableObject
    {
        public GameObject m_BigExplosionPrefab;
        public GameObject m_SmallExplosionPrefab;
        public GameObject m_ShipExplosionPrefab;
        ObjectPool m_BigExplosionPool;
        ObjectPool m_SmallExplosionPool;
        GameObject shipExplosion;
        bool poolsBuilt;

        void OnDisable()
        {
            poolsBuilt = false;
            DestroyImmediate(m_BigExplosionPool);
            DestroyImmediate(m_SmallExplosionPool);
            shipExplosion = null;
        }

        public void BuildPools()
        {
            if (!poolsBuilt)
            {
                m_BigExplosionPool = ObjectPool.Build(m_BigExplosionPrefab, 5, 5);
                m_SmallExplosionPool = ObjectPool.Build(m_SmallExplosionPrefab, 5, 5);
                m_BigExplosionPool.hideFlags = HideFlags.DontSave;
                m_SmallExplosionPool.hideFlags = HideFlags.DontSave;
                shipExplosion = Instantiate(m_ShipExplosionPrefab);
                shipExplosion.SetActive(false);
                poolsBuilt = true;
            }
        }

        public void Explode(string tag, Vector3 position)
        {
            if (tag == "Ship")
            {
                SpawnShipExplosion(position);
            }
            else if (tag == "AsteroidBig")
            {
                bool isAsteroidBig = (tag == "AsteroidBig");
                SpawnAsteroidExplosion(isAsteroidBig, position);
            }
        }

        void SpawnAsteroidExplosion(bool spawnBig, Vector3 position)
        {
            Poolable explosion = spawnBig ? m_BigExplosionPool.GetReusable() : m_SmallExplosionPool.GetReusable();
            SetPositionRotation(explosion.transform, position);
        }

        void SpawnShipExplosion(Vector3 position)
        {
            SetPositionRotation(shipExplosion.transform, position);
            shipExplosion.SetActive(true);
        }

        void SetPositionRotation(Transform transform, Vector3 position)
        {
            transform.position = position;
            transform.Rotate(RandomEulerZ());
        }
        Vector3 RandomEulerZ() { return new Vector3(0f, 0f, UnityEngine.Random.Range(0, 360)); }
    }
