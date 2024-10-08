using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMana : MonoBehaviour
{
    [SerializeField] private string sceneTitle;
    [SerializeField] private string sceneGame;
    [SerializeField] private string sceneGameOver;
    [SerializeField] private string sceneGameClear;
    [SerializeField] private Color fadeColor;
    [SerializeField] private float fadeSpeed;

    SampleSoundManager soundManager;
    public enum Scene
    {
        title,
        game,
        result,
        GameOver
    }
    [SerializeField]
    Scene scene;
    private void Start()
    {
        soundManager = FindObjectOfType<SampleSoundManager>();
        if(scene == Scene.title && soundManager != null)
        {
            soundManager.PlayBgm(BgmType.BGM1);
        }
        if (scene == Scene.GameOver && soundManager != null)
        {
            soundManager.PlayBgm(BgmType.BGM3);
            soundManager.PlaySe(SeType.SE5);
        }
        if (scene == Scene.game && soundManager != null)
        {
            soundManager.PlayBgm(BgmType.BGM2);
        }
        if (scene == Scene.result && soundManager != null)
        {
            soundManager.PlayBgm(BgmType.BGM4);
        }
    }
    private void Update()
    {
        ChangeScene();
    }
    public void ChangeScene()
    {
        if (scene == Scene.title && Input.GetKeyDown(KeyCode.Space))
        {
            Initiate.Fade(sceneGame, fadeColor, fadeSpeed);
            soundManager.PlaySe(SeType.SE1);
            soundManager.StopBgm();
        }
        if (scene == Scene.result && Input.GetKeyDown(KeyCode.Space) || scene == Scene.GameOver && Input.GetKeyDown(KeyCode.Space))
        {
            Initiate.Fade(sceneTitle, fadeColor, fadeSpeed);
        }
    }
    public void ChangeClear()
    {
        Initiate.Fade(sceneGameClear, fadeColor, fadeSpeed);
        soundManager.StopBgm();
    }
    public void ChangeDead()
    {
        Initiate.Fade(sceneGameOver, fadeColor, fadeSpeed);
        soundManager.StopBgm();
    }
}
