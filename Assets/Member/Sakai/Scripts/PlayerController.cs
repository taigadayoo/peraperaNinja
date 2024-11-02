using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ペラエフェクトのゲームオブジェクト
    [SerializeField]
    GameObject peraEffect;

    // プレイヤーのライフポイント
    public int playerLife = 3;

    // プレイヤーの移動速度
    public float moveSpeed = 5f;

    // プレイヤーの移動範囲を制限するための最小座標と最大座標
    public Vector2 minPosition;
    public Vector2 maxPosition;

    // スケール変化の目標値
    public float targetScaleX = 0.5f;
    public float targetScaleFrontX = 3f;
    public float targetScaleFrontY = 4f;
    public float scaleDuration = 2f;

    // プレイヤーの移動に関する変数
    private Rigidbody2D rb;
    private Vector2 moveInput;

    // カメラがプレイヤーを追従するかどうか
    public bool CameraFollowOn = true;

    // スケーリング処理中かどうか
    private bool isScaling = false;

    // 初期スケールを保持
    private Vector3 nomalScale;

    // プレイヤーのアニメーションコントローラ
    private Animator anim;

    // 状態を表すフラグ
    private bool OnPera = false;
    private bool OnSide = false;
    private bool OnFront = false;
    private bool OnDead = false;

    // シーン管理およびサウンド管理オブジェクト
    SceneMana sceneMana;
    SampleSoundManager soundManager;

    // 初期化処理
    void Start()
    {
        CameraFollowOn = true;
        rb = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        nomalScale = this.transform.localScale;
        sceneMana = FindObjectOfType<SceneMana>();
        soundManager = FindObjectOfType<SampleSoundManager>();
    }

    // フレーム毎の更新処理
    void Update()
    {
        SidePera();  // ペラエフェクトの状態を更新
        PlayerAnim(); // アニメーションの状態を更新

        // 移動入力の取得
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // エフェクト位置をプレイヤーの頭上に設定
        float effectPos = this.transform.position.y + 1.4f;
        peraEffect.transform.position = new Vector2(this.transform.position.x, effectPos);

        // 移動ベクトルを正規化して速度を一定に
        moveInput = new Vector2(moveX, moveY).normalized;

        // 「R」キーでペラ状態の切り替え
        if (Input.GetKeyDown(KeyCode.R) && !isScaling && !OnPera)
        {
            OnPera = true;
            StartCoroutine(PeraEffect());
            soundManager.PlaySe(SeType.SE2);
            moveSpeed = 2;
        }
        else if (Input.GetKeyDown(KeyCode.R) && !isScaling && OnPera)
        {
            moveSpeed = 5;
            OnPera = false;
        }

        // ライフが0で死亡処理を実行
        if (playerLife == 0 && !OnDead)
        {
            sceneMana.ChangeDead();
            OnDead = true;
        }
    }

    // 物理更新処理
    void FixedUpdate()
    {
        // プレイヤーの新しい位置を計算
        Vector2 newPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

        // 移動範囲を制限
        newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);

        // プレイヤーの位置を更新
        rb.MovePosition(newPosition);
    }

    // 特定のタグのエリア内にいるかどうかの判定
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CameraStop")
        {
            CameraFollowOn = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CameraStop")
        {
            CameraFollowOn = true;
        }
    }

    // ペラエフェクトの一時的な表示
    private IEnumerator PeraEffect()
    {
        peraEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        peraEffect.SetActive(false);
    }

    // スケールを徐々に変更するコルーチン
    private IEnumerator ScaleXOverTime(float targetX, float targetY, float duration)
    {
        isScaling = true;
        Vector3 originalScale = transform.localScale;
        float elapsedTime = 0f;

        // スケールの線形補間
        while (elapsedTime < duration)
        {
            float newScaleX = Mathf.Lerp(originalScale.x, targetX, elapsedTime / duration);
            float newScaleY = Mathf.Lerp(originalScale.y, targetY, elapsedTime / duration);
            transform.localScale = new Vector3(newScaleX, newScaleY, originalScale.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = new Vector3(targetX, originalScale.y, originalScale.z);
        isScaling = false;
    }

    // ペラエフェクト時のスケールを設定
    void SidePera()
    {
        if (OnSide && !OnFront && OnPera)
        {
            Vector3 currentScale = transform.localScale;
            currentScale.x = targetScaleX;
            currentScale.y = targetScaleFrontY;
            transform.localScale = currentScale;
        }
        if (!OnSide && OnFront && OnPera)
        {
            Vector3 currentScale = transform.localScale;
            currentScale.x = targetScaleFrontX;
            currentScale.y = targetScaleFrontY;
            transform.localScale = currentScale;
        }
        if (!OnPera)
        {
            this.gameObject.transform.localScale = nomalScale;
        }
    }

    // アニメーションの切り替え
    void PlayerAnim()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetBool("Move", true);
            OnSide = false;
            OnFront = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetBool("Left", true);
            OnFront = false;
            OnSide = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetBool("Back", true);
            OnSide = false;
            OnFront = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetBool("Right", true);
            OnFront = false;
            OnSide = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetBool("Move", false);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("Left", false);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            anim.SetBool("Back", false);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("Right", false);
        }
    }

    // ゴール時の処理
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            sceneMana.ChangeClear();
        }
    }
}