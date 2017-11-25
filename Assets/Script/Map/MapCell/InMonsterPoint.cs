using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// 入兵点单位
/// </summary>
public class InMonsterPoint : MapCellBase
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="obj">对象</param>
    /// <param name="dataId">数据ID</param>
    /// <param name="drawLayer">绘制层级</param>
    public InMonsterPoint(GameObject obj, int dataId, int drawLayer) : base(obj, dataId, drawLayer)
    {

    }


    /// <summary>
    /// 回收单位
    /// </summary>
    private void RecycleUnit()
    {
        
    }
}