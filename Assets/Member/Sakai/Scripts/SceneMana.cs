using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMana : MonoBehaviour
{
    // 各シーンのタイトルや名称
    [SerializeField] private string sceneTitle;      // タイトルシーンの名称
    [SerializeField] private string sceneGame;       // ゲームシーンの名称
    [SerializeField] private string sceneGameOver;   // ゲームオーバーシーンの名称
    [SerializeField] private string sceneGameClear;  // ゲームクリアシーンの名称

    // フェードに関する設定
    [SerializeField] private Color fadeColor;        // フェード時の色
    [SerializeField] private float fadeSpeed;        // フェードの速度

    // サウンドマネージャーのインスタンス
    SampleSoundManager soundManager;

    // シーンの列挙型
    public enum Scene
    {
        title,      // タイトルシーン
        game,       // ゲームシーン
        result,     // 結果シーン
        GameOver    // ゲームオーバーシーン
    }

    // 現在のシーンを格納する変数
    [SerializeField]
    Scene scene;

    private void Start()
    {
        // サウンドマネージャーの取得
        soundManager = FindObjectOfType<SampleSoundManager>();

        // シーンに応じたBGMを再生
        if (scene == Scene.title && soundManager != null)
        {
            soundManager.PlayBgm(BgmType.BGM1);
        }
        if (scene == Scene.GameOver && soundManager != null)
        {
            soundManager.PlayBgm(BgmType.BGM3); // ゲームオーバー時のBGM
            soundManager.PlaySe(SeType.SE5);    // ゲームオーバーSE
        }
        if (scene == Scene.game && soundManager != null)
        {
            soundManager.PlayBgm(BgmType.BGM2); // ゲームプレイ時のBGM
        }
        if (scene == Scene.result && soundManager != null)
        {
            soundManager.PlayBgm(BgmType.BGM4); // 結果時のBGM
        }
    }

    private void Update()
    {
        // シーンの変更をチェック
        ChangeScene();
    }

    // シーンを変更するメソッド
    public void ChangeScene()
    {
        // タイトルシーンからゲームシーンへ移行
        if (scene == Scene.title && Input.GetKeyDown(KeyCode.Space))
        {
            Initiate.Fade(sceneGame, fadeColor, fadeSpeed);
            soundManager.PlaySe(SeType.SE1); // スペースキーでSE再生
            soundManager.StopBgm();           // BGM停止
        }

        // 結果シーンまたはゲームオーバーシーンからタイトルシーンへ移行
        if ((scene == Scene.result || scene == Scene.GameOver) && Input.GetKeyDown(KeyCode.Space))
        {
            Initiate.Fade(sceneTitle, fadeColor, fadeSpeed);
        }
    }

    // ゲームクリア時の処理
    public void ChangeClear()
    {
        Initiate.Fade(sceneGameClear, fadeColor, fadeSpeed); // ゲームクリアシーンにフェード
        soundManager.StopBgm(); // BGM停止
    }

    // ゲームオーバー時の処理
    public void ChangeDead()
    {
        Initiate.Fade(sceneGameOver, fadeColor, fadeSpeed); // ゲームオーバーシーンにフェード
        soundManager.StopBgm(); // BGM停止
    }
}
