using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionn : MonoBehaviour
{
    public LayerMask obstructionLayer; // 視界を遮る物体のレイヤー
    public LayerMask enemyLayer; // 敵キャラのレイヤー
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
        // プレイヤーが視界に入った場合
        if (other.CompareTag("Player"))
        {
            // プレイヤーの位置から敵キャラ（監視員）への方向を計算
            Vector3 directionToEnemy = (transform.position - other.transform.position).normalized;

            // プレイヤーから敵キャラへのRayを発射
            RaycastHit2D hit = Physics2D.Raycast(other.transform.position, directionToEnemy, Mathf.Infinity, ~obstructionLayer);

            if (hit.collider != null)
            {
                // Rayが敵キャラに当たった場合（敵キャラのレイヤーをチェック）
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
                    Debug.Log("隠れてる");
                }
            }
            else
            {
                Debug.Log("No obstruction, but no enemy hit.");
            }

            // デバッグ用にRayを視覚化
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
        // プレイヤーが視界内にいる間に継続的にチェックしたい場合
        OnTriggerEnter2D(other); // 同じ処理を呼び出して継続的にチェック
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // プレイヤーが視界から外れたときの処理
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has left the enemy's vision.");
            // 視界から外れたときの処理をここに追加
        }
    }
}
