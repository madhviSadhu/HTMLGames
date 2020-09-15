using UnityEngine;
using System.Collections.Generic;
using System;

    public class ObjectPool : ScriptableObject
    {
        [SerializeField]
        [HideInInspector]
        GameObject prefab;

        [SerializeField]
        [HideInInspector]
        Storaging storaing;

        public bool IsEmpty
        {
            get
            {
                return storaing.IsEmpty;
            }
        }

        public void Reusable(Poolable p)
        {
            storaing.Save(p);
        }

        public T GetReusable<T>() where T : IReusable
        {
            return GetReusable().GetComponent<T>();
        }

        public Poolable GetReusable()
        {
            if (storaing.IsEmpty)
                return Clone();
            else
                return (Poolable)storaing.Unsave();
        }

        public static ObjectPool Build(GameObject prefab, int initialClones, int initialCapacity)
        {
            ObjectPool pool = CreateInstance<ObjectPool>();
            pool.Initialize(prefab, initialClones, initialCapacity);
            return pool;
        }

        void Initialize(GameObject prefab, int initialClones, int capacity)
        {
            this.prefab = prefab;
            storaing = Storaging.InfiniteSpace(capacity);
            ParkInitialClones(initialClones);
        }

        void ParkInitialClones(int initialClones)
        {
            for (int i = 0; i < initialClones; ++i)
                storaing.Save(Clone());
        }

        Poolable Clone()
        {
            GameObject clone = Instantiate(prefab);
            var p = Poolable.AddPoolableComponent(clone, this);
            return p;
        }
    }

    [Serializable]
    public class Storaging
    {
        [SerializeField]
        [HideInInspector]
        List<storaged> Stack;

        Storaging()
        {
        }

        public static Storaging InfiniteSpace(int capacity)
        {
            var p = new Storaging();
            p.Stack = new List<storaged>(capacity);
            return p;
        }

        public bool IsEmpty
        {
            get
            {
                return Stack.Count == 0;
            }
        }

        public void Save(storaged p)
        {
            Push(p);
            p.hide();
        }

        public storaged Unsave()
        {
            storaged p = Pop();
            p.unhide();
            return p;
        }

        void Push(storaged p)
        {
            Stack.Add(p);
        }

        storaged Pop()
        {
            var lastIndex = Stack.Count - 1;
            storaged p = Stack[lastIndex];
            Stack.RemoveAt(lastIndex);
            return p;
        }
    }