using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 地图管理器
/// </summary>
public class MapManager : SingleItem<MapManager>
{


    /// <summary>
    /// 地图文件地址
    /// </summary>
    public const string MapDataFilePath = @"MapData\mapdata";

    /// <summary>
    /// 地图文件数据字典
    /// (地图文件名, 地图数据)
    /// </summary>
    private Dictionary<string, string> mapDataDic = null;

    /// <summary>
    /// 地图文件是否已加载
    /// </summary>
    private bool isLoaded = false;

    // -----------------文件加载管理--------------------


    public void BeginMap(int mapId, int level, Vector3 mapCenter, int unitWidth)
    {
        if (MapDrawer.Single == null)
        {
            throw new Exception("地图绘制器为空.");
        }
        // 验证数据
        if (mapId < 0)
        {
            throw new Exception("地图Id错误:" + mapId);
        }
        if (level < 0)
        {
            throw new Exception("地图level错误:" + level);
        }
        if (unitWidth < 0)
        {
            throw new Exception("地图unitWidth错误:" + unitWidth);
        }

        var mapBase = GetMapInfo(mapId, level, mapCenter, unitWidth);

        if (mapBase == null)
        {
            throw new Exception("地图数据为空");
        }

        // 启动绘制
        MapDrawer.Single.Clear();
        MapDrawer.Single.Init(mapBase);
        MapDrawer.Single.Begin();
    }


    /// <summary>
    /// 转换地图数据为地图单位
    /// </summary>
    /// <param name="mapData">地图数据</param>
    /// <returns></returns>
    public MapCellBase[,] GetCells(int[][] mapData)
    {
        var height = mapData.Length;
        var width = mapData[0].Length;

        var mapCellDataArray = new MapCellBase[height, width];
        // 遍历内容
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                // 加载模型
                var mapCell = UnitFictory.Single.GetUnit(UnitType.MapCell, mapData[i][j]);
                // 根据数据加载
                mapCellDataArray[i, j] = mapCell; 
            }
        }

        return mapCellDataArray;
    }


    // ----------------------------私有方法---------------------------------

    /// <summary>
    /// 按照ID加载文件, 并且将加载文件缓存
    /// </summary>
    /// <param name="mapId">被加载地图DI</param>
    /// <param name="level">被加载层级</param>
    /// <param name="mapCenter">地图中心点</param>
    /// <param name="unitWidth">单位宽度</param>
    /// <returns>被加载地图内容, 如果不存在返回null</returns>
    private MapBase GetMapInfo(int mapId, int level, Vector3 mapCenter, int unitWidth)
    {
        MapBase result = null;

        if (!isLoaded)
        {
            // 加载文件
            mapDataDic = Utils.DepartFileData(Utils.LoadFileRotate(MapDataFilePath));


            if (mapDataDic == null)
            {
                Debug.LogError("加载失败");
            }
            else
            {
                isLoaded = true;
            }
        }
        if (mapId > 0)
        {
            // 从缓存中查找, 如果缓存中不存在, 则从文件中加载
            var key = Utils.GetMapFileNameById(mapId, level);
            if (mapDataDic != null && mapDataDic.ContainsKey(key))
            {
                var mapData = DeCodeInfo(mapDataDic[key]);
                // 转换数据
                result = new MapBase(GetCells(mapData), mapCenter, unitWidth);
            }
            else
            {
                Debug.LogError("地图不存在 ID:" + mapId);
            }
        }

        return result;
    }

    /// <summary>
    /// 解码地图数据
    /// </summary>
    /// <param name="mapInfoJson">地图数据json</param>
    /// <returns>地图数据数组</returns>
    private int[][] DeCodeInfo(string mapInfoJson)
    {
        if (string.IsNullOrEmpty(mapInfoJson))
        {
            return null;
        }

        // 读出数据
        var mapLines = mapInfoJson.Split('\n');

        int[][] mapInfo = new int[mapLines.Length][];
        for (var row = 0; row < mapLines.Length; row++)
        {
            var line = mapLines[row];
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            var cells = line.Split(',');
            mapInfo[row] = new int[cells.Length];
            for (int col = 0; col < cells.Length; col++)
            {
                if (string.IsNullOrEmpty(cells[col].Trim()))
                {
                    continue;
                }
                mapInfo[row][col] = int.Parse(cells[col]);
            }
        }

        return mapInfo;
    }
}