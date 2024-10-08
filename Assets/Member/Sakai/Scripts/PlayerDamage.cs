using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    // 点滅の間隔（秒）
    public float blinkInterval = 0.2f;
    // 点滅する合計時間（秒）
    public float blinkDuration = 3f;
    // プレイヤーのSpriteRendererを保持
    private SpriteRenderer spriteRenderer;
    // 点滅中かどうかのフラグ
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
        // SpriteRendererコンポーネントを取得
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
            // Spriteの表示/非表示を切り替える
            spriteRenderer.enabled = !spriteRenderer.enabled;
            // blinkInterval秒待つ
            yield return new WaitForSeconds(blinkInterval);
        }

        // 点滅が終わったらSpriteを表示状態に戻す
        spriteRenderer.enabled = true;
        isBlinking = false;
    }

    // ダメージを処理する関数
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
