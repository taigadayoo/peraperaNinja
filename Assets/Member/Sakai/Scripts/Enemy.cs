using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float distance = 3.0f; // オブジェクトが移動する最大距離
    public float speed = 2.0f;    // オブジェクトの移動速度

    private Vector3 startPosition;

    void Start()
    {
        // オブジェクトの初期位置を記憶
        startPosition = transform.position;
    }

    void Update()
    {
        // 時間に基づいてオブジェクトの新しい位置を計算
        float newPosition = Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        // オブジェクトの位置を更新
        transform.position = startPosition + new Vector3(newPosition, 0, 0);
    }
}
