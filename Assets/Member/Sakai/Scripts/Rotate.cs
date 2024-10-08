using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 10f;

    // 回転速度を増加させるレート（角度/秒²）
    public float acceleration = 2f;

    void Update()
    {
        // 回転速度を時間と共に増加させる
        rotationSpeed += acceleration * Time.deltaTime;

        // 現在の速度でオブジェクトを回転させる
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
