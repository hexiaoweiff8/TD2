using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


/// <summary>
/// 地图单元
/// </summary>
public class MapCell : MapCellBase
{
    /// <summary>
    /// 单位Obj 从该层对应的对象池获取
    /// </summary>
    public override GameObject GameObj
    {
        get { return base.GameObj; }
        set { base.GameObj = value; }
    }


    /// <summary>
    /// 是否在屏幕中
    /// </summary>
    protected bool isInScreen = false;



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

    /// <summary>
    /// 进入屏幕操作
    /// </summary>
    public void EnterScreen()
    {
        isInScreen = true;
    }

    /// <summary>
    /// 出屏幕操作
    /// </summary>
    public void OutScreen()
    {
        isInScreen = false;
    }
}