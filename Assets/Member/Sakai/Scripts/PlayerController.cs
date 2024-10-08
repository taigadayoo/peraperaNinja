using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject peraEffect;
    public int playerLife = 3;
    public float moveSpeed = 5f; // �v���C���[�̈ړ����x
    public Vector2 minPosition;  // �ړ��͈͂̍ŏ����W
    public Vector2 maxPosition;  // �ړ��͈͂̍ő���W
    public float targetScaleX = 0.5f; // �ڕW��X�X�P�[��
    public float targetScaleFrontX = 3f;
    public float targetScaleFrontY = 4f;
    public float scaleDuration = 2f;  // �X�P�[����ς���̂ɂ����鎞��
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
        // ���͂̎擾
        float moveX = Input.GetAxis("Horizontal"); // A, D�L�[��-1����1�̒l���擾
        float moveY = Input.GetAxis("Vertical");   // W, S�L�[��-1����1�̒l���擾

        float effectPos;
        effectPos = this.transform.position.y + 1.4f; //�y���y���ɂȂ�ۂ̃G�t�F�N�g���v���C���[�̓���ɒǔ�
        peraEffect.transform.position = new Vector2( this.transform.position.x , effectPos);
        // ���̓x�N�g���̍쐬
        moveInput = new Vector2(moveX, moveY).normalized; // �x�N�g���𐳋K�����ăX�s�[�h�����ɂ���

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
        // �v���C���[�̐V�����ʒu���v�Z
        Vector2 newPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

        // �V�����ʒu���w�肳�ꂽ�͈͓��ɐ���
        newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);

        // �v���C���[�̈ړ�
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
            // x���̃X�P�[�����^�[�Q�b�g�l�ɕύX
            currentScale.x = targetScaleX;
            currentScale.y = targetScaleFrontY;
            // �V�����X�P�[����K�p
            transform.localScale = currentScale;
        }
        if(!OnSide&& OnFront && OnPera)
        {
            Vector3 currentScale = transform.localScale;
            // x����y���̃X�P�[�����^�[�Q�b�g�l�ɕύX
            currentScale.x = targetScaleFrontX;
            currentScale.y = targetScaleFrontY;
            // �V�����X�P�[����K�p
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
