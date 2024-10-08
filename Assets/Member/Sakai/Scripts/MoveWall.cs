using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour
{
    public float distance = 5.0f; // �ړ����鋗��
    public float speed = 1.0f; // �ړ����x
    public bool horizontal = true; // �����������ǂ���

    private Vector3 startPosition; // �����ʒu
    private Vector3 targetPosition; // �^�[�Q�b�g�ʒu
    private float time; // ���Ԃ̐i�s���Ǘ�����ϐ�

    void Start()
    {
        // �����ʒu���L��
        startPosition = transform.position;
        // �^�[�Q�b�g�ʒu��ݒ�
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
        // ���Ԃ̐i�s���X�V
        time += Time.deltaTime * speed;

        // Lerp���g���ď����ʒu�ƃ^�[�Q�b�g�ʒu�̊Ԃ��ړ�
        transform.position = Vector3.Lerp(startPosition, targetPosition, Mathf.PingPong(time, 1.0f));
    }
}
