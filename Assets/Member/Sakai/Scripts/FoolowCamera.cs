using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoolowCamera : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform

    private Vector3 offset; // 初期オフセット

    [SerializeField]
    PlayerController playerController;
    void Start()
    {
        // カメラとプレイヤーの初期オフセットを計算
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // 目標のカメラ位置を計算（Y軸のみプレイヤーに追従）
        Vector3 targetCamPos = new Vector3(transform.position.x, player.position.y + offset.y, transform.position.z);
        if (playerController.CameraFollowOn == true)
        {
            // カメラ位置を即座に更新
            transform.position = targetCamPos;
        }
    }
}
