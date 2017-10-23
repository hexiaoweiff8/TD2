using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


/// <summary>
/// 地图绘制器
/// </summary>
public class MapDrawer : MapDrawerBase
{
    /// <summary>
    /// 单例
    /// </summary>
    public static MapDrawer Single { get { return single; } }

    /// <summary>
    /// 单例对象
    /// </summary>
    private static MapDrawer single = null;

    // -----------------------------公有属性-----------------------------

    /// <summary>
    /// 单位宽度
    /// </summary>
    public int UnitWidth = 1;

    /// <summary>
    /// 全绘制
    /// </summary>
    public const int DrawAllDrawType = 0;

    /// <summary>
    /// 绘制范围内单位
    /// </summary>
    public const int DrawRectDrawType = 1;

    /// <summary>
    /// 单位父级
    /// </summary>
    public Transform ItemParent;


    // -----------------------------私有属性-----------------------------

    /// <summary>
    /// 地图数据
    /// </summary>
    private MapBase mapData = null;

    /// <summary>
    /// 回收物体位置对应Dic
    /// </summary>
    private Dictionary<long, MapCellBase> hideObjDic = new Dictionary<long, MapCellBase>();

    /// <summary>
    /// 绘制物体对应地图位置Dic
    /// </summary>
    private Dictionary<long, MapCellBase> objDic = new Dictionary<long, MapCellBase>();

    /// <summary>
    /// 是否已启动
    /// </summary>
    private bool isStarted = false;

    /// <summary>
    /// 绘制方式
    /// 0: 全部绘制
    /// 1: 绘制指定范围内部分
    /// </summary>
    private int drawType = 0;

    /// <summary>
    /// 绘制范围
    /// </summary>
    private Rect drawRect;


    void Awake()
    {
        // 设置单例
        single = this;
        // 设置切换不销毁
        DontDestroyOnLoad(this);
    }



    /// <summary>
    /// 帧更新方法
    /// </summary>
    void Update()
    {
        if (mapData != null && isStarted)
        {
            mapData.DrawLine();
        }
        // 更新地图单元相对位置
        if (drawType == DrawAllDrawType)
        {
            Draw();
        }
        else if (drawType == DrawRectDrawType)
        {
            Draw(drawRect);
        }
    }


    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="mapbase">绘制数据</param>
    /// <param name="rect">绘制范文</param>
    /// <param name="type">绘制类型</param>
    public void Init([NotNull]MapBase mapbase, Rect rect = new Rect(), int type = 0)
    {
        mapData = mapbase;
        drawRect = rect;
        drawType = type;
    }


    /// <summary>
    /// 绘制全局
    /// </summary>
    public override void Draw()
    {
        mapData.DrawMap();
    }


    /// <summary>
    /// 按照范围绘制
    /// 绘制物体超过1400会卡
    /// </summary>
    /// <param name="rect">被绘制范围</param>
    public override void Draw(Rect rect)
    {
        if (mapData == null)
        {
            Debug.LogError("地图数据为空");
            return;
        }

        // 获得数据
        var data = mapData.GetMapCellArray();
        // 数据宽度
        var width = mapData.MapWidth;
        // 数据长度
        var height = mapData.MapHeight;

        var i = 0;
        var j = 0;
        // 遍历所有单位
        for (i = 0; i < height; i++)
        {
            for (j = 0; j < width; j++)
            {
                var key = Utils.GetKey(j, i);
                if (rect.Overlaps(mapData.GetItemRect(j, i)))
                {
                    // 刷新范围内单位
                    // 如果物体已存在, 则不重复绘制
                    if (!objDic.ContainsKey(key))
                    {
                        objDic.Add(key, data[i, j]);
                    }
                    if (hideObjDic.ContainsKey(key))
                    {
                        hideObjDic.Remove(key);
                    }
                }
                else
                {
                    // 将范围外单位回收
                    if (objDic.ContainsKey(key))
                    {
                        objDic.Remove(key);
                    }
                    if (!hideObjDic.ContainsKey(key))
                    {
                        hideObjDic.Add(key, data[i, j]);
                    }
                }
            }
        }

        // 将范围内单位,实例化(从对象池中获取)为UI单位


        // 将范围外单位回收
        CycleBack();
    }




    /// <summary>
    /// 开始运行
    /// </summary>
    public override void Begin()
    {
        Debug.Log("开始绘制");
        isStarted = true;

    }

    /// <summary>
    /// 停止运行
    /// </summary>
    public override void Stop()
    {
        Debug.Log("停止绘制");
        isStarted = false;

    }

    /// <summary>
    /// 清理数据
    /// </summary>
    public override void Clear()
    {
        Debug.Log("清理数据");
        mapData = null;
        isStarted = false;
    }




    /// <summary>
    /// 回收实体
    /// </summary>
    private void CycleBack()
    {
        // 将Hide列表中的单位active设置为false
        //hideObjDic
    }
}


/// <summary>
/// 地图绘制器抽象类
/// </summary>
public abstract class MapDrawerBase : MonoBehaviour
{

    /// <summary>
    /// 绘制全局
    /// </summary>
    public abstract void Draw();

    /// <summary> 
    /// 按照范围绘制
    /// </summary>
    /// <param name="rect">被绘制范围</param>
    public abstract void Draw(Rect rect);

    /// <summary>
    /// 开始运行
    /// </summary>
    public abstract void Begin();

    /// <summary>
    /// 停止运行
    /// </summary>
    public abstract void Stop();

    /// <summary>
    /// 清理数据
    /// </summary>
    public abstract void Clear();


}


/// <summary>
/// 地图加载器
/// </summary>
public class MapLoader
{
    /// <summary>
    /// 单例
    /// </summary>
    public static MapLoader Single
    {
        get
        {
            if (single == null)
            {
                single = new MapLoader();
            }
            return single;
        }
    }

    /// <summary>
    /// 单例对象
    /// </summary>
    private static MapLoader single = null;



    /// <summary>
    /// 根据MapId加载地图数据
    /// </summary>
    /// <param name="mapId">map编号</param>
    /// <returns>地图数据类</returns>
    public MapBase LoadMap(int mapId)
    {
        MapBase result = null;

        // 读取地图文件
        // 获得地图宽度高度
        // 遍历内容加载单位
        // 已存在单位加载设置
        //for (var i = 0; i < MapHeight; i++)
        //{
        //    for (var j = 0; j < MapWidth; j++)
        //    {
        //        MapBase.Single.PushMapCell(new MapCell(1), j, i);
        //    }
        //}

        return result;
    }
}