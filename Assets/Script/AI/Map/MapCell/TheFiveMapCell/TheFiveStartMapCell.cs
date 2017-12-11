﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// 五行起始点
/// </summary>
public class TheFiveStartMapCell : TheFiveCellBase
{

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="dataId"></param>
    /// <param name="drawLayer"></param>
    public TheFiveStartMapCell(GameObject obj, int dataId, int drawLayer) : base(obj, dataId, drawLayer)
    {

        this.Action = (mapCell) =>
        {
            // 获取下一节点, 向其传导属性
            // TODO 节点链 防止循环

        };
    }





    /// <summary>
    /// 执行
    /// </summary>
    public void DoAction()
    {
        if (Action != null)
        {
            Action(this);
        }
    }

}