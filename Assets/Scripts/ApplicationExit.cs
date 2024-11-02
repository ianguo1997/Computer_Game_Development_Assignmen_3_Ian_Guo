using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ApplicationExit : MonoBehaviour
{
    // 这个方法将在按钮点击时被调用
    public void QuitApplication()
    {
        Debug.Log("QuitApplication 方法被调用，正在退出应用程序...");
#if UNITY_EDITOR
        // 在编辑器中停止播放
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 在构建的应用程序中退出
            Application.Quit();
#endif
    }

    // 这个方法将在按钮点击时被调用
    public void LoadStartScene()
    {
        StartCoroutine(LoadAndUnloadScenes());
    }

    private IEnumerator LoadAndUnloadScenes()
    {
        Debug.Log("LoadStartScene 方法被调用，正在加载 StartScene...");

        // 检查 StartScene 是否已加载
        Scene startScene = SceneManager.GetSceneByName("StartScene");
        if (!startScene.isLoaded)
        {
            // 异步加载 StartScene 叠加模式
            AsyncOperation loadOp = SceneManager.LoadSceneAsync("StartScene", LoadSceneMode.Additive);
            loadOp.allowSceneActivation = true;

            while (!loadOp.isDone)
            {
                Debug.Log($"加载 StartScene 进度: {loadOp.progress * 100}%");
                yield return null;
            }

            Debug.Log("StartScene 已加载。");
        }
        else
        {
            Debug.Log("StartScene 已经加载。");
        }

        // 设置 StartScene 为活动场景
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("StartScene"));
        Debug.Log("StartScene 已设置为活动场景。");

        // 检查 SampleScene 是否已加载
        Scene sampleScene = SceneManager.GetSceneByName("SampleScene");
        if (sampleScene.isLoaded)
        {
            // 异步卸载 SampleScene
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync("SampleScene");
            if (unloadOp != null)
            {
                while (!unloadOp.isDone)
                {
                    Debug.Log($"卸载 SampleScene 进度: {unloadOp.progress * 100}%");
                    yield return null;
                }

                Debug.Log("SampleScene 已卸载。");
            }
            else
            {
                Debug.LogError("无法找到 SampleScene，无法卸载。");
            }
        }
        else
        {
            Debug.Log("SampleScene 未加载，无需卸载。");
        }
    }

    public Text highScoreText;
    private int highScore = 0;

    public void UpdateHighScore(int newScore)
    {
        if (newScore > highScore)
        {
            highScore = newScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            UpdateHighScoreUI();
        }
    }

    void UpdateHighScoreUI()
    {
        highScoreText.text = highScore.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 假设你从玩家数据中获取最高分
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
