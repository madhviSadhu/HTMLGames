using UnityEngine;

    public class ExplosionBehaviour : GameObjectBehaviour
    {
        ParticleSystem explosionPS;
        AudioSource explosionAudio;

        void Awake()
        {
            explosionPS = GetComponent<ParticleSystem>();
            explosionAudio = GetComponent<AudioSource>();
        }

        [System.Obsolete]
        void OnEnable()
        {
            explosionPS.Play();
            explosionAudio.Play();
            InvokeRemove(explosionPS.startLifetime);
        }

        protected override void DefaltDestroy()
        {
            gameObject.SetActive(false);
        }
    }