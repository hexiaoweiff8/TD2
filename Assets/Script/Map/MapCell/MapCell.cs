using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// 地图单元
/// </summary>
public class MapCell : MapCellBase
{
    /// <summary>
    /// 初始化地图单元
    /// </summary>
    /// <param name="obj">游戏物体</param>
    /// <param name="dataId">数据Id</param>
    /// <param name="drawLayer">绘制层级</param>
    public MapCell(GameObject obj, int dataId, int drawLayer)
        : base(obj, dataId, drawLayer)
    {
        MapCellType = UnitType.MapCell;
    }
}


