using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoolowCamera : MonoBehaviour
{
    public Transform player; // �v���C���[��Transform

    private Vector3 offset; // �����I�t�Z�b�g

    [SerializeField]
    PlayerController playerController;
    void Start()
    {
        // �J�����ƃv���C���[�̏����I�t�Z�b�g���v�Z
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // �ڕW�̃J�����ʒu���v�Z�iY���̂݃v���C���[�ɒǏ]�j
        Vector3 targetCamPos = new Vector3(transform.position.x, player.position.y + offset.y, transform.position.z);
        if (playerController.CameraFollowOn == true)
        {
            // �J�����ʒu�𑦍��ɍX�V
            transform.position = targetCamPos;
        }
    }
}
