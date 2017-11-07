/*
 * Author:  Rick
 * Create:  2017/11/1 16:37:36
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GMInput
/// </summary>
public class GMInput : MonoBehaviour
{
    public delegate void GMCommandHandler(string commond, string[] args);
    public static event GMCommandHandler onSubmit;

    private string input = "";
    // Use this for initialization
    void Start()
    {

    }

    public void OnGUI()
    {
        GUI.SetNextControlName("GMInput");
        Rect position = new Rect(0, Screen.height - 20, Screen.width, 20);
        input = GUI.TextField(position, input);

        if (Event.current.isKey && Event.current.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == "GMInput")
        {
            Submit();
        }
    }

    public void Submit()
    {
        //处理命令
        string command;
        string[] args = null;
        if (input.IndexOf('=') != -1)
        {
            string[] group = input.Split('=');
            command = group[0];
            args = group[1].Split(',');
        }
        else
        {
            command = input;
        }

        if (onSubmit != null)
            onSubmit(command, args);
        
        //清空输入
        input = "";
    }
}
#endif
