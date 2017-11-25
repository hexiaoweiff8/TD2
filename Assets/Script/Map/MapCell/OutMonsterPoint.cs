using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 出兵点单位
/// </summary>
public class OutMonsterPoint : MapCellBase
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="obj">对象</param>
    /// <param name="dataId">数据ID</param>
    /// <param name="drawLayer">绘制层级</param>
    public OutMonsterPoint(GameObject obj, int dataId, int drawLayer) : base(obj, dataId, drawLayer)
    {
        // 读取数据
        // 生成创建事件链
        // 多个事件链
        // 数据格式 : 类型Id, 数量, Id,
    }

    /// <summary>
    ///  创建单位
    /// </summary>
    private void CreateUnit()
    {
        
    }
}