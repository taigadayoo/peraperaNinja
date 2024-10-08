using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    public float MoveSpeed = 3.0f;

    void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Sin(Time.time) * MoveSpeed, 3.5f, 0);
    }
}
