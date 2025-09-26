using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class TiltMovementController : MonoBehaviour
{
    public float carSpeed = 5;
    private Rigidbody carRigidbody;
    public float tilt;
    
    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        tilt = Input.acceleration.x * 90;
        Vector3 movement = transform.forward * carSpeed * Time.deltaTime;
        carRigidbody.MovePosition(carRigidbody.position + movement);
        Quaternion targetRotation = Quaternion.Euler(0, tilt, 0);
        carRigidbody.MoveRotation(Quaternion.Lerp(carRigidbody.rotation, targetRotation,1 * Time.fixedDeltaTime));
    }
}
