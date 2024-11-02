using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class OpenSampleScene : MonoBehaviour
{
    public void ButtonClick()
    { 
            
    }

    // 方法将在按钮点击时被调用
    public void LoadSampleScene()
    {
        Debug.Log("LoadSampleScene 方法被调用，正在加载 SampleScene...");
        // 加载名为 "SampleScene" 的场景
        SceneManager.LoadScene("SampleScene");
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
