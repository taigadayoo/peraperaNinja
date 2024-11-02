using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMana : MonoBehaviour
{
    // �e�V�[���̃^�C�g���▼��
    [SerializeField] private string sceneTitle;      // �^�C�g���V�[���̖���
    [SerializeField] private string sceneGame;       // �Q�[���V�[���̖���
    [SerializeField] private string sceneGameOver;   // �Q�[���I�[�o�[�V�[���̖���
    [SerializeField] private string sceneGameClear;  // �Q�[���N���A�V�[���̖���

    // �t�F�[�h�Ɋւ���ݒ�
    [SerializeField] private Color fadeColor;        // �t�F�[�h���̐F
    [SerializeField] private float fadeSpeed;        // �t�F�[�h�̑��x

    // �T�E���h�}�l�[�W���[�̃C���X�^���X
    SampleSoundManager soundManager;

    // �V�[���̗񋓌^
    public enum Scene
    {
        title,      // �^�C�g���V�[��
        game,       // �Q�[���V�[��
        result,     // ���ʃV�[��
        GameOver    // �Q�[���I�[�o�[�V�[��
    }

    // ���݂̃V�[�����i�[����ϐ�
    [SerializeField]
    Scene scene;

    private void Start()
    {
        // �T�E���h�}�l�[�W���[�̎擾
        soundManager = FindObjectOfType<SampleSoundManager>();

        // �V�[���ɉ�����BGM���Đ�
        if (scene == Scene.title && soundManager != null)
        {
            soundManager.PlayBgm(BgmType.BGM1);
        }
        if (scene == Scene.GameOver && soundManager != null)
        {
            soundManager.PlayBgm(BgmType.BGM3); // �Q�[���I�[�o�[����BGM
            soundManager.PlaySe(SeType.SE5);    // �Q�[���I�[�o�[SE
        }
        if (scene == Scene.game && soundManager != null)
        {
            soundManager.PlayBgm(BgmType.BGM2); // �Q�[���v���C����BGM
        }
        if (scene == Scene.result && soundManager != null)
        {
            soundManager.PlayBgm(BgmType.BGM4); // ���ʎ���BGM
        }
    }

    private void Update()
    {
        // �V�[���̕ύX���`�F�b�N
        ChangeScene();
    }

    // �V�[����ύX���郁�\�b�h
    public void ChangeScene()
    {
        // �^�C�g���V�[������Q�[���V�[���ֈڍs
        if (scene == Scene.title && Input.GetKeyDown(KeyCode.Space))
        {
            Initiate.Fade(sceneGame, fadeColor, fadeSpeed);
            soundManager.PlaySe(SeType.SE1); // �X�y�[�X�L�[��SE�Đ�
            soundManager.StopBgm();           // BGM��~
        }

        // ���ʃV�[���܂��̓Q�[���I�[�o�[�V�[������^�C�g���V�[���ֈڍs
        if ((scene == Scene.result || scene == Scene.GameOver) && Input.GetKeyDown(KeyCode.Space))
        {
            Initiate.Fade(sceneTitle, fadeColor, fadeSpeed);
        }
    }

    // �Q�[���N���A���̏���
    public void ChangeClear()
    {
        Initiate.Fade(sceneGameClear, fadeColor, fadeSpeed); // �Q�[���N���A�V�[���Ƀt�F�[�h
        soundManager.StopBgm(); // BGM��~
    }

    // �Q�[���I�[�o�[���̏���
    public void ChangeDead()
    {
        Initiate.Fade(sceneGameOver, fadeColor, fadeSpeed); // �Q�[���I�[�o�[�V�[���Ƀt�F�[�h
        soundManager.StopBgm(); // BGM��~
    }
}
