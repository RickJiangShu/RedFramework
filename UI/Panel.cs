/*
 * Author:  Rick
 * Create:  2017/11/8 14:30:04
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Panel基类
/// </summary>
public abstract class Panel : MonoBehaviour
{
    public virtual void Hide()
    {
        UIManager.HidePanel(gameObject.name);
    }
}
