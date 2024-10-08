using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    // �_�ł̊Ԋu�i�b�j
    public float blinkInterval = 0.2f;
    // �_�ł��鍇�v���ԁi�b�j
    public float blinkDuration = 3f;
    // �v���C���[��SpriteRenderer��ێ�
    private SpriteRenderer spriteRenderer;
    // �_�Œ����ǂ����̃t���O
    public bool isBlinking = false;

    SampleSoundManager soundManager;
    PlayerController playerController;
    [SerializeField]
    GameObject Life1;
    [SerializeField]
    GameObject Life2;
    [SerializeField]
    GameObject Life3;

    void Start()
    {
        // SpriteRenderer�R���|�[�l���g���擾
        spriteRenderer = GetComponent<SpriteRenderer>();
        soundManager = FindObjectOfType<SampleSoundManager>();
        playerController = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        LifeManager();
    }

    private IEnumerator Blink()
    {
        isBlinking = true;
        float endTime = Time.time + blinkDuration;

        while (Time.time < endTime)
        {
            // Sprite�̕\��/��\����؂�ւ���
            spriteRenderer.enabled = !spriteRenderer.enabled;
            // blinkInterval�b�҂�
            yield return new WaitForSeconds(blinkInterval);
        }

        // �_�ł��I�������Sprite��\����Ԃɖ߂�
        spriteRenderer.enabled = true;
        isBlinking = false;
    }

    // �_���[�W����������֐�
    public void HandleDamage()
    {
        if (playerController.playerLife > 1)
        {
            soundManager.PlaySe(SeType.SE3);
        }
        if (playerController.playerLife == 1)
        {
            soundManager.PlaySe(SeType.SE4);
        }
        playerController.playerLife--;
        StartCoroutine(Blink());
    }
    public void LifeManager()
    {
        if(playerController.playerLife == 2)
        {
            Life3.SetActive(false);
        }
        if (playerController.playerLife == 1)
        {
            Life2.SetActive(false);
        }
        if (playerController.playerLife == 0)
        {
            Life1.SetActive(false);
        }
    }
}
