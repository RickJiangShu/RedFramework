/*
 * Author:  Rick
 * Create:  2017/10/21 17:15:54
 * Email:   rickjiangshu@gmail.com
 * Follow:  https://github.com/RickJiangShu
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIManager
/// </summary>
public class UIManager : MonoBehaviour
{
    #region 常量
    /// <summary>
    /// 名字
    /// </summary>
    public const string NAME = "Canvas(UIManager)";
    
    /// <summary>
    /// GameObject的Layer
    /// </summary>
    private const int UNITY_UI_LAYER = 5;

    /// <summary>
    /// 层级之间的距离（米）
    /// </summary>
    private const float LAYER_DISTANCE = 5f;
    #endregion

    #region 自身组件
    private static RectTransform root;
    private static Camera camera;
    private static Canvas canvas;
    private static CanvasScaler scaler;
    private static GraphicRaycaster raycaster;
    #endregion

    /// <summary>
    /// 整个UI是否可交互
    /// </summary>
    public static bool interactable
    {
        get { return raycaster.enabled; }
        set { raycaster.enabled = value; }
    }

    /// <summary>
    /// 层级
    /// </summary>
    private static RectTransform[] layers = new RectTransform[8];
    
    void Awake()
    {
        root = gameObject.AddComponent<RectTransform>();

        //设置层级
        gameObject.layer = UNITY_UI_LAYER;

        //Canvas
        canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;

        scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.matchWidthOrHeight = 1f;

        raycaster = gameObject.AddComponent<GraphicRaycaster>();

        //Camera
        camera = new GameObject("UICamera").AddComponent<Camera>();
        camera.gameObject.layer = UNITY_UI_LAYER;
        camera.transform.SetParent(root, false);
        camera.clearFlags = CameraClearFlags.Depth;
        camera.cullingMask = 1 << UNITY_UI_LAYER;
        camera.orthographic = true;
        camera.nearClipPlane = 0f;
        camera.farClipPlane = LAYER_DISTANCE * layers.Length + 0.3f;
        camera.transform.localPosition = new Vector3(0f, 0f, -LAYER_DISTANCE * layers.Length * 100f);
        canvas.worldCamera = camera;

        //Instantiate Containers
        for (int i = 0, l = layers.Length; i < l; i++)
        {
            GameObject container = new GameObject(name, typeof(RectTransform));
            container.layer = UNITY_UI_LAYER;
            container.transform.SetParent(root, false);

            container.name = "Layer" + i;

            RectTransform layer = (RectTransform)container.transform;
            layer.offsetMin = Vector2.zero;
            layer.offsetMax = Vector2.zero;
            layer.anchorMin = Vector2.zero;
            layer.anchorMax = Vector2.one;
            layer.localPosition = new Vector3(0f, 0f, -LAYER_DISTANCE * i * 100f);
            layers[i] = layer;
        }
    }

    public void Set(UISettings settings)
    {
        scaler.referenceResolution = settings.resolution;
        camera.orthographicSize = scaler.referenceResolution.y * 0.005f;

        root.position = settings.offset;
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void HUD(int layer, string content, Vector3 target)
    {
        HUD(layer, content, target, HUDSettings.GetDefault());
    }

    /// <summary>
    /// 显示HUDText
    /// </summary>
    public static void HUD(int layer, string content, Vector3 target, HUDSettings settings)
    {
        GameObject go = Warehouser.Pull(HUDText.NAME);
        HUDText hudText;
        if (go == null)
        {
            go = new GameObject("HUDText", typeof(RectTransform));
            go.layer = UNITY_UI_LAYER;
            hudText = go.AddComponent<HUDText>();
        }
        else
        {
            hudText = go.GetComponent<HUDText>();
        }

        //设置位置
        go.transform.localPosition = MainCamera2Canvas(target) + settings.enterOffset;

        //添加到UI层
        AddChild(hudText.gameObject, layer);

        //设置并播放
        hudText.Set(content, settings);
        hudText.Play(settings);
    }

    /// <summary>
    /// 添加到层级
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    public static void AddChild(GameObject go, int layer, bool worldPositionStays = false)
    {
        go.transform.SetParent(layers[layer], worldPositionStays);
    }

    /// <summary>
    /// 删除某一层所有的子对象
    /// </summary>
    /// <param name="layer"></param>
    public static void RemoveChildren(int layer)
    {
        foreach (Transform child in layers[layer])
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// 获取某一层下的某个对象
    /// </summary>
    /// <returns></returns>
    public static Transform GetChild(int layer, int index)
    {
        return layers[layer].GetChild(index);
    }

#region 坐标转换
    /// <summary>
    /// 从主摄像机中的坐标转换到画布坐标
    /// </summary>
    /// <returns></returns>
    public static Vector2 MainCamera2Canvas(Vector3 worldPosition)
    {
        Vector2 viewport = Camera.main.WorldToViewportPoint(worldPosition);
        viewport = (viewport - root.pivot) * 2;
        float w = root.rect.width * 0.5f;
        float h = root.rect.height * 0.5f;
        return new Vector2(viewport.x * w, viewport.y * h);
    }

    /// <summary>
    /// 从画布坐标转换到主摄像机中的坐标
    /// </summary>
    /// <returns></returns>
    public static Vector3 Canvas2MainCamera(Vector2 canvasPosition)
    {
        float w = root.rect.width * 0.5f;
        float h = root.rect.height * 0.5f;
        Vector3 viewport = new Vector3((canvasPosition.x / w + 1f) / 2, (canvasPosition.y / h + 1f) / 2, 0f);
        return Camera.main.ViewportToWorldPoint(viewport);
    }

    /// <summary>
    /// 世界坐标转换为相对于Canvas坐标
    /// </summary>
    /// <returns></returns>
    public static Vector2 World2Canvas(Vector2 position)
    {
        return root.InverseTransformPoint(position);
    }

    /// <summary>
    /// 相对于Canavs坐标转换为世界坐标
    /// </summary>
    /// <param name="localPosition"></param>
    /// <returns></returns>
    public static Vector2 Canvas2World(Vector2 localPosition)
    {
        return root.TransformPoint(localPosition);
    }
#endregion
}
