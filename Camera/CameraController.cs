/*
 * Author:  Rick
 * Create:  2017/11/7 14:58:50
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CameraController
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// 耳朵
    /// </summary>
    private static AudioListener audioListener;

    /// <summary>
    /// 
    /// </summary>
    private static Transform controller;

    /// <summary>
    /// 摄像机偏移的位置
    /// </summary>
    private static Vector3 cameraOffset;

    /// <summary>
    /// 摄像机
    /// </summary>
    private static Camera camera;

    /// <summary>
    /// 跟随的目标
    /// </summary>
    private static Transform followTarget;
    private static float followSpeed;

    /// <summary>
    /// 移动的目标
    /// </summary>
    private static Vector3? moveTarget;
    private static Vector3 moveOrigin;
    private static float moveDuration;
    private static float moveTime;

    /// <summary>
    /// 震动时间
    /// </summary>
    private static float shakeTime = 0f;
    private static float shakeAmout = 0f;
    private static float shakeDecrease = 0f;

    // Use this for initialization
    void Awake()
    {
        controller = transform;

        CameraSettings settings = RedFramework.settings.camera;
        cameraOffset = settings.offset;

        //GameObject
        GameObject cameraGO = new GameObject("Main Camera");
        cameraGO.tag = "MainCamera";
        cameraGO.transform.SetParent(transform, false);
        cameraGO.transform.localPosition = cameraOffset;
        cameraGO.transform.localRotation = Quaternion.Euler(settings.euler);
        camera = cameraGO.AddComponent<Camera>();
        camera.clearFlags = settings.clearFlags;
        camera.backgroundColor = settings.backgroundColor;
        camera.cullingMask = settings.cullingMask;
        audioListener = cameraGO.AddComponent<AudioListener>();
    }


    void Update()
    {
        if (moveTarget != null)
        {
            moveTime += Time.deltaTime;
            float t = moveTime / moveDuration;

            if (t < 1.0f)
            {
                Vector3 offset = (moveTarget.Value - moveOrigin) * t;
                Vector3 position = moveOrigin + offset;
                transform.position = position;
            }
            else
            {
                transform.position = moveTarget.Value;
                moveTarget = null;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(followTarget != null)
        {
            if (followSpeed == 0f)
            {
                transform.position = followTarget.position;
            }
            else
            {
                float distance = Vector3.Distance(transform.position, followTarget.position);
                float step = followSpeed * Time.deltaTime;
                if (distance > step)
                {
                    Vector3 direction = (followTarget.position - transform.position).normalized;
                    Vector3 position = transform.position + direction * step;
                    transform.position = position;
                }
                else
                {
                    transform.position = followTarget.position;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (shakeTime > 0)
        {
            camera.transform.localPosition = cameraOffset + Random.insideUnitSphere * shakeAmout;
            shakeTime -= Time.fixedDeltaTime * shakeDecrease;

            if (shakeTime <= 0)
            {
                shakeTime = 0f;
                camera.transform.localPosition = cameraOffset;
            }
        }
    }

    public static void Follow(Transform target)
    {
        Follow(target, 0f);
    }
    /// <summary>
    /// 跟随目标
    /// </summary>
    public static void Follow(Transform target, float speed)
    {
#if UNITY_EDITOR
        if (moveTarget != null)
        {
            Debug.LogError("CameraController：在移动状态下跟随目标！");
        }
#endif

        followTarget = target;
        followSpeed = speed;
    }

    /// <summary>
    /// 移动到目标点
    /// </summary>
    public static void MoveTo(Vector3 target, float time)
    {
#if UNITY_EDITOR
        if (followTarget != null)
        {
            Debug.LogError("CameraController：在跟随状态下移动相机！");
        }
#endif

        moveTarget = target;
        moveOrigin = controller.position;
        moveDuration = time;
        moveTime = 0f;
    }

    /// <summary>
    /// 震动
    /// </summary>
    public static void Shake(float time, float amount, float decrease)
    {
        shakeTime = time;
        shakeAmout = amount;
        shakeDecrease = decrease;
    }

    /// <summary>
    /// 停止跟随
    /// </summary>
    public static void StopFollow()
    {
        followTarget = null;
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public static void StopMove()
    {
        moveTarget = null;
    }

    /// <summary>
    /// 停止震动
    /// </summary>
    public static void StopShake()
    {
        shakeTime = 0f;
    }

    /// <summary>
    /// 设置位置
    /// </summary>
    public static void SetPosition(Vector3 position)
    {
        controller.position = position;
    }
}
