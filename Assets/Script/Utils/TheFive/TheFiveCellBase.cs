﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

/// <summary>
/// 五行单元基类
/// </summary>
public class TheFiveCellBase
{
    /// <summary>
    /// 当前cell属性
    /// </summary>
    public TheFiveType TheFiveType = TheFiveType.None;


    // TODO 四个方向的单位与连通性 放到外部管理

    /// <summary>
    /// 五行属性
    /// </summary>
    public TheFiveProperties properties = new TheFiveProperties();

    /// <summary>
    /// 输出数量
    /// </summary>
    public int ExoprtCount { get; set; }


    /// <summary>
    /// 向外传输
    /// </summary>
    /// <returns>输出自己</returns>
    public TheFiveProperties Export()
    {
        if (ExoprtCount > 0)
        {
            // 分裂
            return properties/ExoprtCount;
        }
        else
        {
            return properties;
        }
    }

    /// <summary>
    /// 吸收来自其他单元的属性
    /// </summary>
    /// <param name="fromCell">来源及cell</param>
    public void Absorb([NotNull]TheFiveCellBase fromCell)
    {
        properties.Plus(fromCell.properties);
    }
}

/// <summary>
/// 五行属性
/// </summary>
public class TheFiveProperties
{


    /// <summary>
    /// 无属性值
    /// </summary>
    public float NoneValue { get; private set; }

    /// <summary>
    /// 火属性值
    /// </summary>
    public float FireValue { get; private set; }

    /// <summary>
    /// 水属性值
    /// </summary>
    public float WaterValue { get; private set; }

    /// <summary>
    /// 金属性值
    /// </summary>
    public float MetalValue { get; private set; }

    /// <summary>
    /// 木属性值
    /// </summary>
    public float WoodValue { get; private set; }

    /// <summary>
    /// 土属性值
    /// </summary>
    public float EarthValue { get; private set; }

    /// <summary>
    /// 链接次数
    /// </summary>
    public int LinkCount { get; private set; }


    /// <summary>
    /// 吸收来自其他单元的属性
    /// </summary>
    /// <param name="fromCell">来源及cell</param>
    public void Plus([NotNull]TheFiveProperties fromCell)
    {
        // 获取目标值到当前cell中
        // 混合本地值
        NoneValue += fromCell.NoneValue;
        FireValue += fromCell.FireValue;
        WaterValue += fromCell.WaterValue;
        MetalValue += fromCell.MetalValue;
        WoodValue += fromCell.WoodValue;
        EarthValue += fromCell.EarthValue;

        // 如果五属性都有则将其转化为无属性
        var min = Utils.MinValue(FireValue, WaterValue, MetalValue, WoodValue, EarthValue);
        if (min > 0)
        {
            NoneValue += min;
            FireValue -= min;
            WaterValue -= min;
            MetalValue -= min;
            WoodValue -= min;
            EarthValue -= min;
        }
        LinkCount += fromCell.LinkCount;
    }



    /// <summary>
    /// 除操作
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="count">分裂数量</param>
    /// <returns></returns>
    public static TheFiveProperties operator /([NotNull]TheFiveProperties cell, int count)
    {
        return new TheFiveProperties()
        {
            NoneValue = cell.NoneValue / count,
            FireValue = cell.FireValue / count,
            WaterValue = cell.WaterValue / count,
            MetalValue = cell.MetalValue / count,
            WoodValue = cell.WoodValue / count,
            EarthValue = cell.EarthValue / count,
        };
    }
}

/// <summary>
/// 五行类型
/// </summary>
public enum TheFiveType
{
    None,   // 无属性
    Fire,   // 火
    Water,  // 水
    Metal,  // 金
    Wood,   // 木
    Earth   // 土
}