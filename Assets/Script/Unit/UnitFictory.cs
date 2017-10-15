using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// 单位工厂
/// 创建各种单位
/// </summary>
public class UnitFictory : SingleItem<UnitFictory>
{


    /// <summary>
    /// 创建单位
    /// </summary>
    /// <param name="unitType">单位类型</param>
    /// <param name="id">资源id</param>
    /// <param name="resourceId">资源ID</param>
    /// <returns>地图单元类</returns>
    public MapCellBase GetUnit(UnitType unitType, int resourceId)
    {
        MapCellBase result = null;
        switch (unitType)
        {
            case UnitType.MapCell:  // 地图单元
                result = new MapCell(GetGameObject(resourceId));
                break;
            case UnitType.Obstacle: // 障碍物

                break;
            case UnitType.FightUnit:// 战斗单位

                break;
            case UnitType.NPC:      // NPC

                break;
        }
        return result;
    }


    /// <summary>
    /// 获取游戏物体
    /// </summary>
    /// <param name="id">资源ID</param>
    /// <returns></returns>
    public GameObject GetGameObject(int id)
    {
        GameObject result = null;

        // 读表
        // 获得资源数据
        // 加载资源
        // 返回资源

        return result;
    }

}

/// <summary>
/// 单位类型
/// </summary>
public enum UnitType
{
    // 地图单元
    MapCell,
    // 障碍物
    Obstacle,
    // 战斗单位
    FightUnit,
    // NPC
    NPC,
}