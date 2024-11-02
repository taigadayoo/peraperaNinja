using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �y���G�t�F�N�g�̃Q�[���I�u�W�F�N�g
    [SerializeField]
    GameObject peraEffect;

    // �v���C���[�̃��C�t�|�C���g
    public int playerLife = 3;

    // �v���C���[�̈ړ����x
    public float moveSpeed = 5f;

    // �v���C���[�̈ړ��͈͂𐧌����邽�߂̍ŏ����W�ƍő���W
    public Vector2 minPosition;
    public Vector2 maxPosition;

    // �X�P�[���ω��̖ڕW�l
    public float targetScaleX = 0.5f;
    public float targetScaleFrontX = 3f;
    public float targetScaleFrontY = 4f;
    public float scaleDuration = 2f;

    // �v���C���[�̈ړ��Ɋւ���ϐ�
    private Rigidbody2D rb;
    private Vector2 moveInput;

    // �J�������v���C���[��Ǐ]���邩�ǂ���
    public bool CameraFollowOn = true;

    // �X�P�[�����O���������ǂ���
    private bool isScaling = false;

    // �����X�P�[����ێ�
    private Vector3 nomalScale;

    // �v���C���[�̃A�j���[�V�����R���g���[��
    private Animator anim;

    // ��Ԃ�\���t���O
    private bool OnPera = false;
    private bool OnSide = false;
    private bool OnFront = false;
    private bool OnDead = false;

    // �V�[���Ǘ�����уT�E���h�Ǘ��I�u�W�F�N�g
    SceneMana sceneMana;
    SampleSoundManager soundManager;

    // ����������
    void Start()
    {
        CameraFollowOn = true;
        rb = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        nomalScale = this.transform.localScale;
        sceneMana = FindObjectOfType<SceneMana>();
        soundManager = FindObjectOfType<SampleSoundManager>();
    }

    // �t���[�����̍X�V����
    void Update()
    {
        SidePera();  // �y���G�t�F�N�g�̏�Ԃ��X�V
        PlayerAnim(); // �A�j���[�V�����̏�Ԃ��X�V

        // �ړ����͂̎擾
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // �G�t�F�N�g�ʒu���v���C���[�̓���ɐݒ�
        float effectPos = this.transform.position.y + 1.4f;
        peraEffect.transform.position = new Vector2(this.transform.position.x, effectPos);

        // �ړ��x�N�g���𐳋K�����đ��x������
        moveInput = new Vector2(moveX, moveY).normalized;

        // �uR�v�L�[�Ńy����Ԃ̐؂�ւ�
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

        // ���C�t��0�Ŏ��S���������s
        if (playerLife == 0 && !OnDead)
        {
            sceneMana.ChangeDead();
            OnDead = true;
        }
    }

    // �����X�V����
    void FixedUpdate()
    {
        // �v���C���[�̐V�����ʒu���v�Z
        Vector2 newPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

        // �ړ��͈͂𐧌�
        newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);

        // �v���C���[�̈ʒu���X�V
        rb.MovePosition(newPosition);
    }

    // ����̃^�O�̃G���A���ɂ��邩�ǂ����̔���
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

    // �y���G�t�F�N�g�̈ꎞ�I�ȕ\��
    private IEnumerator PeraEffect()
    {
        peraEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        peraEffect.SetActive(false);
    }

    // �X�P�[�������X�ɕύX����R���[�`��
    private IEnumerator ScaleXOverTime(float targetX, float targetY, float duration)
    {
        isScaling = true;
        Vector3 originalScale = transform.localScale;
        float elapsedTime = 0f;

        // �X�P�[���̐��`���
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

    // �y���G�t�F�N�g���̃X�P�[����ݒ�
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

    // �A�j���[�V�����̐؂�ւ�
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

    // �S�[�����̏���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            sceneMana.ChangeClear();
        }
    }
}