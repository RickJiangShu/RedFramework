/*
 * Author:  Rick
 * Create:  2017/11/7 15:12:49
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RedFrameSettings
/// </summary>
[System.Serializable]
public class RedFrameworkSettings : ScriptableObject
{
    /// <summary>
    /// UI设置
    /// </summary>
    public UISettings UI;

    /// <summary>
    /// 相机设置
    /// </summary>
    public CameraSettings camera;


}
