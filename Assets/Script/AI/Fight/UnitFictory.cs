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
    /// 障碍单元数据key名称
    /// </summary>
    public const string ObstacleTableName = "ObstacleData";


    /// <summary>
    /// 障碍单元数据key名称
    /// </summary>
    public const string TowerCellTableName = "TowerCellData";


    /// <summary>
    /// 
    /// </summary>
    private Dictionary<long, MapCellBase> mapCellBases = new Dictionary<long, MapCellBase>();


    /// <summary>
    /// 创建单位
    /// </summary>
    /// <param name="unitType">单位类型</param>
    /// <param name="dataId">数据ID</param>
    /// <returns>地图单元类</returns>
    public MapCellBase CreateUnit(UnitType unitType, int dataId)
    {
        // 地图单元与障碍物使用相同单元替换Image的方式进行服用, 来解决创建单位过多问题
        MapCellBase result = null;
        switch (unitType)
        {
                // ---------------------------加载数据进行替换模式----------------------------
            case UnitType.MapCell: // 地图单元
            {
                //var go = GetGameObject(MapCellTableName,
                //    dataId,
                //    MapDrawer.Single.ItemParentList[MapManager.MapBaseLayer]);

                result = new MapCell(null, dataId, MapManager.MapBaseLayer);
                //go.name = result.MapCellId.ToString();
            }
                break;
            case UnitType.Obstacle: // 障碍物
            {
                var go = GetGameObject(MapCellTableName,
                    dataId,
                    MapDrawer.Single.ItemParentList[MapManager.MapObstacleLayer]);

                result = new Obstacle(go, dataId, MapManager.MapObstacleLayer);
                go.name = result.MapCellId.ToString();
            }
                break;

            // ---------------------------------加载预设模式---------------------------------
            case UnitType.FightUnit: // 战斗单位
            {
                var go = GetGameObject(MapCellTableName,
                    dataId,
                    MapDrawer.Single.ItemParentList[MapManager.MapPlayerLayer]);

                result = new FightUnit(go, dataId, MapManager.MapPlayerLayer);
                go.name = result.MapCellId.ToString();
            }
                break;
            case UnitType.NPC: // NPC
            {
                var go = GetGameObject(MapCellTableName,
                    dataId,
                    MapDrawer.Single.ItemParentList[MapManager.MapNpcLayer]);

                // TODO 区分出兵点入兵点
                switch (dataId)
                {
                    case 301:
                        // 出兵点
                        result = new OutMonsterPoint(go, dataId, MapManager.MapObstacleLayer);
                        break;
                    case 302:
                        // 入兵点
                        result = new InMonsterPoint(go, dataId, MapManager.MapObstacleLayer);
                        break;
                    case 401:
                        // 塔基
                        result = new TowerPoint(go, dataId, MapManager.MapObstacleLayer);
                        break;
                    default:
                        result = new Npc(go, dataId, MapManager.MapNpcLayer);
                        break;
                }
                go.name = result.MapCellId.ToString();
            }
                break;

            case UnitType.Tower:
            {
                // 创建塔
                var go = GetGameObject(MapCellTableName,
                    dataId,
                    MapDrawer.Single.ItemParentList[MapManager.MapNpcLayer]);
                result = new Tower(go, dataId, MapManager.MapPlayerLayer);
                // 设置数据
                go.name = result.MapCellId.ToString();
            }
                break;
            //case UnitType.OutPoint: // 出兵点
            //{
            //    var go = GetGameObject(MapCellTableName,
            //        dataId,
            //        MapDrawer.Single.ItemParentList[MapManager.MapNpcLayer]);

            //    result = new OutMonsterPoint(go, dataId, MapManager.MapNpcLayer);
            //    go.name = result.MapCellId.ToString();
            //}
            //break;
            //case UnitType.InPoint: // 入兵点
            //{
            //    var go = GetGameObject(MapCellTableName,
            //        dataId,
            //        MapDrawer.Single.ItemParentList[MapManager.MapNpcLayer]);

            //    result = new InMonsterPoint(go, dataId, MapManager.MapNpcLayer);
            //    go.name = result.MapCellId.ToString();
            //}
                //break;
        }



        return result;
    }


    ///// <summary>
    ///// 获取单位
    ///// </summary>
    ///// <param name="unitType">单位大类型</param>
    ///// <param name="dataId">单位类型Id</param>
    ///// <returns></returns>
    //public MapCellBase GetUnit(UnitType unitType, int dataId)
    //{
    //    MapCellBase result = null;
    //    switch (unitType)
    //    {
    //        case UnitType.MapCell: // 地图单元
    //            {
    //                var go = GetGameObject(MapCellTableName,
    //                    dataId,
    //                    MapDrawer.Single.ItemParentList[MapManager.MapBaseLayer]);

    //                result = new MapCell(go, dataId, MapManager.MapBaseLayer);
    //                go.name = result.MapCellId.ToString();
    //            }
    //            break;
    //        case UnitType.Obstacle: // 障碍物
    //            {
    //                var go = GetGameObject(MapCellTableName,
    //                    dataId,
    //                    MapDrawer.Single.ItemParentList[MapManager.MapObstacleLayer]);
    //                result = new Obstacle(go, dataId, MapManager.MapObstacleLayer);
    //                go.name = result.MapCellId.ToString();
    //            }
    //            break;
    //        case UnitType.FightUnit: // 战斗单位
    //            {
    //                var go = GetGameObject(MapCellTableName,
    //                    dataId,
    //                    MapDrawer.Single.ItemParentList[MapManager.MapPlayerLayer]);

    //                result = new FightUnit(go, dataId, MapManager.MapPlayerLayer);
    //                go.name = result.MapCellId.ToString();
    //            }
    //            break;
    //        case UnitType.NPC: // NPC
    //            {
    //                var go = GetGameObject(MapCellTableName,
    //                    dataId,
    //                    MapDrawer.Single.ItemParentList[MapManager.MapNpcLayer]);

    //                result = new Npc(go, dataId, MapManager.MapNpcLayer);
    //                go.name = result.MapCellId.ToString();
    //            }
    //            break;
    //    }


    //    return result;
    //}


    /// <summary>
    /// 获取游戏物体
    /// </summary>
    /// <param name="tableName">表名称</param>
    /// <param name="id">资源ID</param>
    /// <param name="parent">单位父级</param>
    /// <returns>游戏实体</returns>
    public GameObject GetGameObject(string tableName, int id, Transform parent = null)
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
    MapCell = 1,
    // 障碍物
    Obstacle = 2,
    // NPC
    NPC = 3,
    // 战斗单位
    FightUnit = 4,
    // 出兵点
    OutPoint = 5,
    // 入兵点
    InPoint = 6,
    // 塔基
    TowerPoint = 7,
    // 塔
    Tower = 8,
}