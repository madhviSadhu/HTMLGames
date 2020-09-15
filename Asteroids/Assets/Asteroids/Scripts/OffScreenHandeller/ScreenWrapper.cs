using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class ScreenWrapper : ScreenOffSet
    {
        public override void Awake()
        {
            base.Awake();
        }

        public override void Update()
        {
            base.Update();
            if (isOffscreen)
            {
                transform.position = Camera.main.ViewportToWorldPoint(viewportPos);
            }
        }
    }