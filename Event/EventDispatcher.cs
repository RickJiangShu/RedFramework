/*
 * Author:  Rick
 * Create:  2017/11/21 16:47:25
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
using System;
using System.Collections.Generic;

/// <summary>
/// 事件派发器
/// 1. 优化装箱/拆箱
/// 2. 不使用闭包
/// </summary>
public class EventDispatcher
{
    public static EventDispatcher global
    {
        get;
        private set;
    }
    static EventDispatcher()
    {
        global = new EventDispatcher();
    }

    private Dictionary<string, Delegate> _listeners = new Dictionary<string, Delegate>();

    public void AddListener<T1, T2, T3, T4>(string evt, Action<T1, T2, T3, T4> callback)
    {
        AddListener(evt, (Delegate)callback);
    }
    public void AddListener<T1, T2, T3>(string evt, Action<T1, T2, T3> callback)
    {
        AddListener(evt, (Delegate)callback);
    }
    public void AddListener<T1, T2>(string evt, Action<T1, T2> callback)
    {
        AddListener(evt, (Delegate)callback);
    }
    public void AddListener<T>(string evt, Action<T> callback)
    {
        AddListener(evt, (Delegate)callback);
    }
    public void AddListener(string evt, Action callback)
    {
        AddListener(evt, (Delegate)callback);
    }
    public void AddListener(string evt, object callback)
    {
    }

    public void AddListener(string evt, Delegate callback)
    {
        Delegate listener;
        if (_listeners.TryGetValue(evt, out listener))
        {
            _listeners[evt] = Delegate.Combine(listener, callback);
        }
        else
        {
            _listeners[evt] = callback;
        }
    }

    public void RemoveListener<T1, T2, T3, T4>(string evt, Action<T1, T2, T3, T4> callback)
    {
        RemoveListener(evt, (Delegate)callback);
    }
    public void RemoveListener<T1, T2, T3>(string evt, Action<T1, T2, T3> callback)
    {
        RemoveListener(evt, (Delegate)callback);
    }
    public void RemoveListener<T1, T2>(string evt, Action<T1, T2> callback)
    {
        RemoveListener(evt, (Delegate)callback);
    }
    public void RemoveListener<T>(string evt, Action<T> callback)
    {
        RemoveListener(evt, (Delegate)callback);
    }
    public void RemoveListener(string evt, Action callback)
    {
        RemoveListener(evt, callback);
    }
    private void RemoveListener(string evt, Delegate callback)
    {
        Delegate listener;
        if (_listeners.TryGetValue(evt, out listener))
        {
            listener = Delegate.Remove(listener, callback);
            if (listener == null)
            {
                _listeners.Remove(evt);
            }
            else
            {
                _listeners[evt] = listener;
            }
        }
    }

    public void Dispatch<T1, T2, T3, T4>(string evt, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        Delegate[] methods = GetMethods(evt);
        if (methods != null)
        {
            foreach (Delegate m in methods)
            {
                try
                {
                    ((Action<T1, T2, T3, T4>)m)(arg1, arg2, arg3, arg4);
                }
                catch (Exception e) { LogError(e); }
            }
        }
    }

    public void Dispatch<T1, T2, T3>(string evt, T1 arg1, T2 arg2, T3 arg3)
    {
        Delegate[] methods = GetMethods(evt);
        if (methods != null)
        {
            foreach (Delegate m in methods)
            {
                try
                {
                    ((Action<T1, T2, T3>)m)(arg1, arg2, arg3);
                }
                catch (Exception e) { LogError(e); }
            }
        }
    }

    public void Dispatch<T1, T2>(string evt, T1 arg1, T2 arg2)
    {
        Delegate[] methods = GetMethods(evt);
        if (methods != null)
        {
            foreach (Delegate m in methods)
            {
                try
                {
                    ((Action<T1, T2>)m)(arg1, arg2);
                }
                catch (Exception e) { LogError(e); }
            }
        }
    }

    public void Dispatch<T>(string evt, T arg)
    {
        Delegate[] methods = GetMethods(evt);
        if (methods != null)
        {
            foreach (Delegate m in methods)
            {
                try
                {
                    ((Action<T>)m)(arg);
                }
                catch (Exception e) { LogError(e); }
            }
        }
    }

    public void Dispatch(string evt)
    {
        Delegate[] methods = GetMethods(evt);
        if (methods != null)
        {
            foreach (Delegate m in methods)
            {
                try
                {
                    ((Action)m)();
                }
                catch (Exception e) { LogError(e); }
            }
        }
    }

    private Delegate[] GetMethods(string evt)
    {
        Delegate listener;
        if (_listeners.TryGetValue(evt, out listener))
        {
            return listener.GetInvocationList();
        }
        return null;
    }

    private static void LogError(Exception e)
    {
        UnityEngine.Debug.LogError(e);
    }
}
