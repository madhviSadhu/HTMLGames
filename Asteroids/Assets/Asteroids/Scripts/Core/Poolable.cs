using UnityEngine;
using UnityEngine.EventSystems;
using System;

    [Serializable]
    [ExecuteInEditMode]
    public sealed class Poolable : storaged, IReusable
    {
        [SerializeField]
        [HideInInspector]
        ObjectPool pool;

        static bool scriptBuiltInstance;

        void Awake()
        {
            InstanceObj();
            ExecuteEvents.Execute<IPoolableAware>(gameObject, null, (script, ignored) => script.PoolableAwake(this));
        }

        void InstanceObj()
        {
            if (!scriptBuiltInstance)
            {
                DestroyImmediate(this, true);
                throw new InvalidOperationException("Can only be created with AddPoolableComponent");
            }
            scriptBuiltInstance = false;
        }

        void OnEnable()
        {
            gameObject.hideFlags = 0;
        }

        void OnDisable()
        {
            gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        public void Reuse()
        {
            pool.Reusable(this);
        }

        public static Poolable AddPoolableComponent(GameObject newInstance, ObjectPool pool)
        {
            scriptBuiltInstance = true;
            var instance = newInstance.AddComponent<Poolable>();
            instance.pool = pool;
            return instance;
        }
    }

    public interface IPoolableAware : IEventSystemHandler
    {
        void PoolableAwake(Poolable p);
    }

    public interface IReusable
    {
        void Reuse();
    }

    public abstract class storaged : MonoBehaviour
    {
        public virtual void hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void unhide()
        {
            gameObject.SetActive(true);
        }
    }