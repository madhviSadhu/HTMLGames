using UnityEngine;
public class BulletBehaviour : GameObjectBehaviour
{
    public float bulletLife = 1f;
    Rigidbody rb;

    public virtual void Fire(Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        transform.position = position;
        transform.rotation = rotation;
        rb.velocity = velocity;
    }

    void Awake() { rb = GetComponent<Rigidbody>(); }
    void OnEnable() { InvokeRemove(bulletLife); }
}
