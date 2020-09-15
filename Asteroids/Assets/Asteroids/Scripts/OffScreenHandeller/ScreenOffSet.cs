using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class ScreenOffSet : MonoBehaviour
    {
        [SerializeField]
        private float padding = 0.1f;
        private float top;
        private float bottom;
        private float left;
        private float right;

        protected Vector3 viewportPos;
        protected bool isOffscreen;

        public virtual void Awake()
        {
            top = 0.0f - padding;
            bottom = 1.0f + padding;
            left = 0.0f - padding;
            right = 1.0f + padding;
        }

        public virtual void Update()
        {
            viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            isOffscreen = false;

            // check x
            if (viewportPos.x < left)
            {
                viewportPos.x = right;
                isOffscreen = true;
            }
            else if (viewportPos.x > right)
            {
                viewportPos.x = left;
                isOffscreen = true;
            }
            // check y
            if (viewportPos.y < top)
            {
                viewportPos.y = bottom;
                isOffscreen = true;
            }
            else if (viewportPos.y > bottom)
            {
                viewportPos.y = top;
                isOffscreen = true;
            }
        }
    }