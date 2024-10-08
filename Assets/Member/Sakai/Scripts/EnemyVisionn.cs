using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionn : MonoBehaviour
{
    public LayerMask obstructionLayer; // ���E���Ղ镨�̂̃��C���[
    public LayerMask enemyLayer; // �G�L�����̃��C���[
    [SerializeField]
    GameObject visionEffect;
    PlayerDamage playerDamage;
    SceneMana sceneMana;
    private void Start()
    {
        playerDamage = FindObjectOfType<PlayerDamage>();
        sceneMana = FindObjectOfType<SceneMana>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // �v���C���[�����E�ɓ������ꍇ
        if (other.CompareTag("Player"))
        {
            // �v���C���[�̈ʒu����G�L�����i�Ď����j�ւ̕������v�Z
            Vector3 directionToEnemy = (transform.position - other.transform.position).normalized;

            // �v���C���[����G�L�����ւ�Ray�𔭎�
            RaycastHit2D hit = Physics2D.Raycast(other.transform.position, directionToEnemy, Mathf.Infinity, ~obstructionLayer);

            if (hit.collider != null)
            {
                // Ray���G�L�����ɓ��������ꍇ�i�G�L�����̃��C���[���`�F�b�N�j
                if (((1 << hit.collider.gameObject.layer) & enemyLayer) != 0)
                {
                    if(!playerDamage.isBlinking)
                    {
                        StartCoroutine(BikkuriEffect());
                        playerDamage.HandleDamage();
                    }
                }
                else
                {
                    Debug.Log("�B��Ă�");
                }
            }
            else
            {
                Debug.Log("No obstruction, but no enemy hit.");
            }

            // �f�o�b�O�p��Ray�����o��
            Debug.DrawRay(other.transform.position, directionToEnemy * 10f, Color.blue, 1.0f);
        }
    }
    private IEnumerator BikkuriEffect()
    {
        visionEffect.SetActive(true);

        yield return new WaitForSeconds(0.6f);

        visionEffect.SetActive(false);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        // �v���C���[�����E���ɂ���ԂɌp���I�Ƀ`�F�b�N�������ꍇ
        OnTriggerEnter2D(other); // �����������Ăяo���Čp���I�Ƀ`�F�b�N
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // �v���C���[�����E����O�ꂽ�Ƃ��̏���
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has left the enemy's vision.");
            // ���E����O�ꂽ�Ƃ��̏����������ɒǉ�
        }
    }
}
