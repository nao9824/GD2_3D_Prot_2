using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skydome : MonoBehaviour
{
    // ‰ñ“]‘¬“xi–ˆ•b“x”j
    public float rotationSpeed = 10.0f;

    // Update is called once per frame
    void Update()
    {
        // Y²‰ñ‚è‚É‰ñ“]‚³‚¹‚é
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
