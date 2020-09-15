using UnityEngine;

    public class GameObjectBehaviour : MonoBehaviour, IPoolableAware, IReusable
    {
        Poolable poolable;
        void IPoolableAware.PoolableAwake(Poolable p)
        {
            poolable = p;
        }

        void IReusable.Reuse()
        {
            Remove();
        }

        protected void Score(int score)
        {
            global::Score.Earn(score);
        }

        protected virtual void OnDisable()
        {
            CancelInvokeRemove();
        }

        public void InvokeRemove(float time)
        {
            Invoke("Remove", time);
        }

        public void CancelInvokeRemove()
        {
            CancelInvoke("Remove");
        }

        public void Remove()
        {
            if (poolable)
                poolable.Reuse();
            else
                DefaltDestroy();
        }

        public static void RemoveFromGame(GameObject victim)
        {
            GameObjectBehaviour handler = victim.GetComponent<GameObjectBehaviour>();
            if (handler)
                handler.Remove();
            else
                DeleteObj(victim);
        }

        protected virtual void DefaltDestroy()
        {
            DeleteObj(gameObject);
        }

        static void DeleteObj(GameObject gameObject)
        {
            Destroy(gameObject);
        }
    }