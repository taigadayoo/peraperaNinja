using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float distance = 3.0f; // �I�u�W�F�N�g���ړ�����ő勗��
    public float speed = 2.0f;    // �I�u�W�F�N�g�̈ړ����x

    private Vector3 startPosition;

    void Start()
    {
        // �I�u�W�F�N�g�̏����ʒu���L��
        startPosition = transform.position;
    }

    void Update()
    {
        // ���ԂɊ�Â��ăI�u�W�F�N�g�̐V�����ʒu���v�Z
        float newPosition = Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        // �I�u�W�F�N�g�̈ʒu���X�V
        transform.position = startPosition + new Vector3(newPosition, 0, 0);
    }
}
