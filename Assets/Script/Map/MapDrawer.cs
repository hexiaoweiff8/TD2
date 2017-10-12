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
    private Dictionary<long, GameObject> cycleBackObjDic = new Dictionary<long, GameObject>();

    /// <summary>
    /// 绘制物体对应地图位置Dic
    /// </summary>
    private Dictionary<long, GameObject> ObjDic = new Dictionary<long, GameObject>();


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
        var width = mapbase.CellWidth;
        // 数据长度
        var height = mapbase.MapHeight;

        var i = 0;
        var j = 0;
        // 遍历所有单位
        for (i = 0; i < height; i++)
        {
            for (j = 0; j < width; j++)
            {
                if (rect.Overlaps(mapbase.GetItemRect(j, i)))
                {
                    // 刷新范围内单位
                    // 如果物体已存在, 则不重复绘制
                }
                else
                {
                    // 将范围外单位回收

                }
            }
        }

        // 将范围内单位,实例化(从对象池中获取)为UI单位

    }

    /// <summary>
    /// 回收实体
    /// </summary>
    private void CycleBack()
    {
        
    }

    /// <summary>
    /// 检测是否在范围内
    /// </summary>
    /// <returns></returns>
    private bool CheckInRect(Rect rect)
    {
        var result = false;



        return result;
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