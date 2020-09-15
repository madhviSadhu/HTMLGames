using UnityEngine;

[System.Serializable]
class RandomVector3
{
    [Range(0, 5)]
    [SerializeField]
    float minScale = 1.0f;

    [Range(0, 5)]
    [SerializeField]
    float maxScale = 1.5f;

    public Vector3 Randomize()
    {
        return Vector3X.randomValue(minScale, maxScale);
    }
}

public static class Vector3X
{
    public static Vector3 randomValue(float min, float max)
    {
        float r = Random.Range(min, max);
        return new Vector3(r, r, r);
    }
}

public static class Viewport
{
    public static Vector3 GetRandomWorldPositionXY()
    {
        Vector3 randomXY = new Vector3(Random.value, Random.value);
        return ViewportToWorldPointXY(randomXY);
    }

    static Vector3 ViewportToWorldPointXY(Vector3 viewPoint)
    {
        Vector3 world = Camera.main.ViewportToWorldPoint(viewPoint);
        world.z = 0; 
        return world;
    }
}

public static class getRigidbody
{
    public static void Reset(Rigidbody rb)
    {
        rb.position = Vector3.zero;
        rb.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public static void SetRandomForce(Rigidbody rb, float maxForce)
    {
        Vector3 randomForce = maxForce * Random.insideUnitSphere;
        rb.velocity = Vector3.zero;
        rb.AddForce(randomForce);
    }

    public static void SetRandomTorque(Rigidbody rb, float maxTorque)
    {
        Vector3 randomTorque = maxTorque * Random.insideUnitSphere;
        rb.angularVelocity = Vector3.zero;
        rb.AddTorque(randomTorque);
    }
}

public static class Score
{
    public delegate void PointsAdded(int points);

    public static event PointsAdded onEarn;
    public static int earned { get; private set; }

    public static void Reset()
    {
        earned = 0;
        Invoke_onEarn(0);
    }

    public static void Earn(int points)
    {
        earned += points;
        Invoke_onEarn(points);
    }

    public static void Tally()
    {
        Invoke_onEarn(0);
    }

    static void Invoke_onEarn(int points)
    {
        if (onEarn != null)
            onEarn(points);
    }

    public static void LevelCleared(int level)
    {
        Earn(level * 100);
    }
}

public static class ShipInput
{
    public static bool IsShooting()
    {
        return Input.GetButtonDown("Fire1");
    }

    public static bool IsHyperspacing()
    {
        return Input.GetButtonDown("Fire2");
    }

    public static float GetTurnAxis()
    {
        return Input.GetAxis("Horizontal");
    }

    public static float GetForwardThrust()
    {
        float axis = Input.GetAxis("Vertical");
        return Mathf.Clamp01(axis);
    }
}