using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject peraEffect;
    public int playerLife = 3;
    public float moveSpeed = 5f; // プレイヤーの移動速度
    public Vector2 minPosition;  // 移動範囲の最小座標
    public Vector2 maxPosition;  // 移動範囲の最大座標
    public float targetScaleX = 0.5f; // 目標のXスケール
    public float targetScaleFrontX = 3f;
    public float targetScaleFrontY = 4f;
    public float scaleDuration = 2f;  // スケールを変えるのにかける時間
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public bool CameraFollowOn = true;
     private bool isScaling = false;
    private Vector3 nomalScale;
    private Animator anim;
    private bool OnPera = false;
    private bool OnSide = false;
    private bool OnFront = false;
    private bool OnDead = false;
    SceneMana sceneMana;
    SampleSoundManager soundManager;
    void Start()
    {
        CameraFollowOn = true;
        rb = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        nomalScale = this.transform.localScale;
        sceneMana = FindObjectOfType<SceneMana>();
        soundManager = FindObjectOfType<SampleSoundManager>();
    }

    void Update()
    {
        SidePera();
        PlayerAnim();
        // 入力の取得
        float moveX = Input.GetAxis("Horizontal"); // A, Dキーで-1から1の値を取得
        float moveY = Input.GetAxis("Vertical");   // W, Sキーで-1から1の値を取得

        float effectPos;
        effectPos = this.transform.position.y + 1.4f; //ペラペラになる際のエフェクトをプレイヤーの頭上に追尾
        peraEffect.transform.position = new Vector2( this.transform.position.x , effectPos);
        // 入力ベクトルの作成
        moveInput = new Vector2(moveX, moveY).normalized; // ベクトルを正規化してスピードを一定にする

        if (Input.GetKeyDown(KeyCode.R) && !isScaling && !OnPera )
        {
            OnPera = true;
            StartCoroutine(PeraEffect());
            soundManager.PlaySe(SeType.SE2);
            moveSpeed = 2; 
        }
        else if(Input.GetKeyDown(KeyCode.R) && !isScaling && OnPera )
        {
            moveSpeed = 5;
            OnPera = false;
        }

        if(playerLife == 0 && !OnDead)
        {
            sceneMana.ChangeDead();
            OnDead = true;
        }
    }

    void FixedUpdate()
    {
        // プレイヤーの新しい位置を計算
        Vector2 newPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

        // 新しい位置を指定された範囲内に制限
        newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);

        // プレイヤーの移動
        rb.MovePosition(newPosition);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "CameraStop")
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
   private IEnumerator PeraEffect()
    {
        peraEffect.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        peraEffect.SetActive(false);
    }
    private IEnumerator ScaleXOverTime(float targetX,float targetY, float duration)
    {
        isScaling = true;
        Vector3 originalScale = transform.localScale;
        float elapsedTime = 0f;

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
    void SidePera()
    {
        if( OnSide && !OnFront && OnPera)
        {
            Vector3 currentScale = transform.localScale;
            // x軸のスケールをターゲット値に変更
            currentScale.x = targetScaleX;
            currentScale.y = targetScaleFrontY;
            // 新しいスケールを適用
            transform.localScale = currentScale;
        }
        if(!OnSide&& OnFront && OnPera)
        {
            Vector3 currentScale = transform.localScale;
            // x軸とy軸のスケールをターゲット値に変更
            currentScale.x = targetScaleFrontX;
            currentScale.y = targetScaleFrontY;
            // 新しいスケールを適用
            transform.localScale = currentScale;
        }
        if(!OnPera)
        {
            this.gameObject.transform.localScale = nomalScale;
        }
    }
    void PlayerAnim()
    {
        if(Input.GetKeyDown(KeyCode.W))
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            sceneMana.ChangeClear();
        }
    }
}
