using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// FightUnit
/// </summary>
public class FightUnit : MapCellBase
{
    /// <summary>
    /// 初始化战斗单位
    /// </summary>
    /// <param name="obj">游戏物体</param>
    /// <param name="dataId">数据Id</param>
    /// <param name="drawLayer">绘制层级</param>
    public FightUnit(GameObject obj, int dataId, int drawLayer)
        : base(obj, dataId, drawLayer)
    {
        MapCellType = UnitType.FightUnit;
    }
}