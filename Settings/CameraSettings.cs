/*
 * Author:  Rick
 * Create:  2017/11/7 14:59:15
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
using UnityEngine;

/// <summary>
/// CameraSettings
/// </summary>
[System.Serializable]
public class CameraSettings
{
    public Vector3 offset;
    public Vector3 euler;
    public CameraClearFlags clearFlags = CameraClearFlags.Skybox;
    public Color backgroundColor;
    public int cullingMask;
}
