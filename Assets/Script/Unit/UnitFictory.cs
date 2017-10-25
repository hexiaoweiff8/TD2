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
    /// 资源地址
    /// </summary>
    public const string ResourceName = "Resource";

    /// <summary>
    /// 地图单元数据key名称
    /// </summary>
    public const string MapCellTableName = "MapCellData";


    /// <summary>
    /// 
    /// </summary>
    private Dictionary<long, MapCellBase> mapCellBases = new Dictionary<long, MapCellBase>();


    /// <summary>
    /// 创建单位
    /// </summary>
    /// <param name="unitType">单位类型</param>
    /// <param name="resourceId">资源ID</param>
    /// <param name="layer">层级</param>
    /// <returns>地图单元类</returns>
    public MapCellBase GetUnit(UnitType unitType, int resourceId, int layer)
    {
        MapCellBase result = null;
        switch (unitType)
        {
            case UnitType.MapCell: // 地图单元

                var go = GetGameObject(MapCellTableName,
                    resourceId,
                    layer,
                    layer < 0 ? null : MapDrawer.Single.ItemParentList[layer]);

                result = new MapCell(go, layer);
                go.name = result.MapCellId.ToString();

                break;
            case UnitType.Obstacle: // 障碍物

                break;
            case UnitType.FightUnit: // 战斗单位

                break;
            case UnitType.NPC: // NPC

                break;
        }

        return result;
    }


    /// <summary>
    /// 获取游戏物体
    /// </summary>
    /// <param name="tableName">表名称</param>
    /// <param name="id">资源ID</param>
    /// <param name="layer">物体所在层</param>
    /// <param name="parent">单位父级</param>
    /// <returns>游戏实体</returns>
    public GameObject GetGameObject(string tableName, int id, int layer, Transform parent = null)
    {
        GameObject result = null;

        // 读表
        var dataItem = DataPacker.Single[tableName][id.ToString()];
        // 加载资源
        var path = dataItem.GetString(ResourceName);
        // 返回资源
        result = PoolLoader.Single.Load(path, parent: parent);

        return result;
    }

    /// <summary>
    /// 销毁单元
    /// </summary>
    /// <param name="mapCell">地图单元</param>
    public void DestoryMapCell(MapCellBase mapCell)
    {
        GameObject.Destroy(mapCell.GameObj);
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