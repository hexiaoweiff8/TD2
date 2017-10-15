using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// 地图绘制器
/// </summary>
public class MapDrawer : MapDrawerBase
{

    /// <summary>
    /// 回收物体位置对应Dic
    /// </summary>
    private Dictionary<long, MapCellBase> hideObjDic = new Dictionary<long, MapCellBase>();

    /// <summary>
    /// 绘制物体对应地图位置Dic
    /// </summary>
    private Dictionary<long, MapCellBase> objDic = new Dictionary<long, MapCellBase>();


    /// <summary>
    /// 绘制全局
    /// </summary>
    public override void Draw()
    {
        // 获得数据

        // 读取数据, 将数据实例化(对象池)为UI单位

    }


    /// <summary>
    /// 按照范围绘制
    /// 绘制物体超过1400会卡
    /// </summary>
    /// <param name="mapbase">地图基础数据</param>
    /// <param name="rect">被绘制范围</param>
    public override void Draw(MapBase mapbase, Rect rect)
    {
        if (mapbase == null)
        {
            Debug.LogError("地图数据为空");
            return;
        }

        // 获得数据
        var data = mapbase.GetMapCellArray();
        // 数据宽度
        var width = mapbase.UnitWidth;
        // 数据长度
        var height = mapbase.MapHeight;

        var i = 0;
        var j = 0;
        // 遍历所有单位
        for (i = 0; i < height; i++)
        {
            for (j = 0; j < width; j++)
            {
                var key = Utils.GetKey(j, i);
                if (rect.Overlaps(mapbase.GetItemRect(j, i)))
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


    }

    /// <summary>
    /// 回收实体
    /// </summary>
    private void CycleBack()
    {
        
    }
}


/// <summary>
/// 地图绘制器抽象类
/// </summary>
public abstract class MapDrawerBase
{

    /// <summary>
    /// 绘制全局
    /// </summary>
    public abstract void Draw();

    /// <summary> 
    /// 按照范围绘制
    /// </summary>
    /// <param name="mapbase">地图基础数据</param>
    /// <param name="rect">被绘制范围</param>
    public abstract void Draw(MapBase mapbase, Rect rect);


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