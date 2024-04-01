using UnityEngine;

public class DiceBehavior : MonoBehaviour
{
    private void Start()
    {
        SetInitialState();
    }

    private void SetInitialState()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        int x = Random.Range(0, 360);
        int y = Random.Range(0, 360);
        int z = Random.Range(0, 360);
        Quaternion rotation = Quaternion.Euler(x, y, z);

        x = Random.Range(0, 15);
        y = Random.Range(0, 15);
        z = Random.Range(0, 15);
        Vector3 force = new(x, -y, z);

        x = Random.Range(0, 50);
        y = Random.Range(0, 50);
        z = Random.Range(0, 50);
        Vector3 torque = new(x, y, z);

        transform.rotation = rotation;
        rb.velocity = force;

        rb.maxAngularVelocity = 1000;
        rb.AddTorque(torque, ForceMode.VelocityChange);
    }
}