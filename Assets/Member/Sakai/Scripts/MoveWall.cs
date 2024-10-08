using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour
{
    public float distance = 5.0f; // 移動する距離
    public float speed = 1.0f; // 移動速度
    public bool horizontal = true; // 水平方向かどうか

    private Vector3 startPosition; // 初期位置
    private Vector3 targetPosition; // ターゲット位置
    private float time; // 時間の進行を管理する変数

    void Start()
    {
        // 初期位置を記憶
        startPosition = transform.position;
        // ターゲット位置を設定
        if (horizontal)
        {
            targetPosition = startPosition + new Vector3(distance, 0, 0);
        }
        else
        {
            targetPosition = startPosition + new Vector3(0, distance, 0);
        }
    }

    void Update()
    {
        // 時間の進行を更新
        time += Time.deltaTime * speed;

        // Lerpを使って初期位置とターゲット位置の間を移動
        transform.position = Vector3.Lerp(startPosition, targetPosition, Mathf.PingPong(time, 1.0f));
    }
}
