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
    private const float _layerDinstanceInterval = 5f;

    #endregion

    #region 自身组件
    private static RectTransform root;
    private static Camera camera;
    private static Canvas canvas;
    private static CanvasScaler scaler;
    private static GraphicRaycaster raycaster;

    private static GameObject _panelMask;
    #endregion

    /// <summary>
    /// 整个UI是否可交互
    /// </summary>
    public static bool interactable
    {
        get { return raycaster.enabled; }
        set { raycaster.enabled = value; }
    }


    private static int[] _layerOrders = new int[8] { 0, 8, 16, 24, 32, 40, 48, 56 };

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
        camera.farClipPlane = _layerDinstanceInterval * layers.Length + 0.3f;
        camera.transform.localPosition = new Vector3(0f, 0f, -_layerDinstanceInterval * layers.Length * 100f);
        canvas.worldCamera = camera;

        //Instantiate Containers
        for (int i = 0, l = layers.Length; i < l; i++)
        {
            GameObject container = new GameObject("Layer" + i, typeof(RectTransform));
            container.layer = UNITY_UI_LAYER;
            container.transform.SetParent(root, false);

            RectTransform layer = (RectTransform)container.transform;
            layer.offsetMin = Vector2.zero;
            layer.offsetMax = Vector2.zero;
            layer.anchorMin = Vector2.zero;
            layer.anchorMax = Vector2.one;
            layer.localPosition = new Vector3(0f, 0f, -_layerDinstanceInterval * i * 100f);
            layers[i] = layer;

            if (i == 0)
                continue;

            //设置排序Canvas（主要针对粒子）
            Canvas sortedCanvas = layers[i].gameObject.AddComponent<Canvas>();
            sortedCanvas.overrideSorting = true;
            sortedCanvas.sortingOrder = _layerOrders[i];
            sortedCanvas.gameObject.AddComponent<GraphicRaycaster>();
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

    /// <summary>
    /// 显示HUDText
    /// </summary>
    public static void HUD(string content, int fontSize, Color color, Vector3 target, HUDSettings settings)
    {
        GameObject go = Warehouser.GetObject(HUDText.NAME, typeof(RectTransform), typeof(HUDText));
        HUDText hudText = go.GetComponent<HUDText>();

        //设置位置
        go.transform.localPosition = MainCamera2Canvas(target) + new Vector2(0f, 100f);

        //添加到UI层
        AddChild(hudText.gameObject, UILayer.BelowMainUI);

        //设置并播放
        hudText.Set(content, fontSize, color, target, settings);
        hudText.Play(settings);
    }

    public static void AddPanel(GameObject go, bool setChildrenOrders = false)
    {
        //遮罩处理

        AddChild(go,UILayer.Panel,false,setChildrenOrders);
    }

    /// <summary>
    /// 添加到层级
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    public static void AddChild(GameObject go, UILayer layer, 
        bool worldPositionStays = false, 
        bool setChildrenOrders = false)
    {
        if (setChildrenOrders)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].sortingOrder = _layerOrders[(int)layer];
            }
        }

        go.transform.SetParent(layers[(int)layer], worldPositionStays);
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
    /// 屏幕坐标转换到相对于Canvas坐标
    /// </summary>
    /// <returns></returns>
    public static Vector2 Screen2Canvas(Vector2 position)
    {
        Vector2 world = camera.ScreenToWorldPoint(position);
        return World2Canvas(world);
    }

    public static Vector2 Canvas2Screen(Vector2 position)
    {
        return Vector2.zero;
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

public enum UILayer
{
    Limbo = 0,
    BelowMainUI = 1,
    MainUI = 2,
    AboveMainUI = 3,
    BelowPanel = 4,
    Panel = 5,
    AbovePanel = 6,
    Sky = 7
}
