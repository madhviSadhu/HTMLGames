using UnityEngine;

    public class PowerupBehaviour : RandomizeGameObject
    {
        protected GameObject ship;
        public void SetReceiver(GameObject receiver) { ship = receiver; }

        [Range(5, 30)]
        public int showTime = 10;

        [Range(5, 30)]
        public int powerDuration = 10;

        public bool isVisible;

        void Start() { HideInScene(); }

        protected virtual void OnCollisionEnter(Collision otherCollision)
        {
            GameObject otherObject = otherCollision.gameObject;
            if (otherObject == ship)
            {
                Score(destructionScore);
                Remove();
                GrantPower();
            }
        }

        public virtual void GrantPower()
        {
            CancelInvoke("DenyPower");
            Invoke("DenyPower", powerDuration);
        }

        public virtual void DenyPower() { }

        public void ActivateTemporarily()
        {
            ShowInScene();
            InvokeRemove(showTime);
        }

        void ShowInScene()
        {
            Spawn();
            SetVisibility(true);
        }

        void HideInScene()
        {
            CancelInvokeRemove();
            SetVisibility(false);
        }

        protected override void DefaltDestroy()
        {
            HideInScene();
        }

        void SetVisibility(bool isVisible)
        {
            this.isVisible = isVisible;
            gameObject.GetComponent<Renderer>().enabled = this.isVisible;
            gameObject.GetComponent<Collider>().enabled = this.isVisible;
        }
    }