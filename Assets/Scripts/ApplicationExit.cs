using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
