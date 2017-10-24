
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 工具类
/// </summary>
public class Utils
{



    /// <summary>
    /// 角度转π
    /// </summary>
    public const float AngleToPi = 0.0174532925199433f;

    /// <summary>
    /// π转角度
    /// </summary>
    public const float PiToAngle = 57.2957795130823f;

    /// <summary>
    /// 地图文件名称Head
    /// </summary>
    private const string MapNameHead = "MapInfo";

    /// <summary>
    /// 地图文件名称Level部分
    /// </summary>
    private const string MapNameLevel = "_Level";




    /// <summary>
    /// 2转3
    /// </summary>
    /// <param name="vec2"></param>
    /// <returns></returns>
    public static Vector3 V2ToV3(Vector2 vec2)
    {
        return new Vector3(vec2.x, vec2.y);
    }

    /// <summary>
    /// 获取node的key值
    /// </summary>
    /// <param name="x">位置x</param>
    /// <param name="y">位置y</param>
    /// <returns>key值</returns>
    public static long GetKey(long x, long y)
    {
        var result = (x << 32) + y;
        return result;
    }

    /// <summary>
    /// 生成地图文件名
    /// </summary>
    /// <param name="mapId">地图ID</param>
    /// <param name="mapLevel">地图层级</param>
    /// <returns>地图文件名</returns>
    public static string GetMapFileNameById(int mapId, int mapLevel)
    {
        return GetMapFileNameById(string.Format("{0:0000}", mapId), mapLevel);
    }

    /// <summary>
    /// 生成地图文件名
    /// </summary>
    /// <param name="mapId">地图ID</param>
    /// <param name="mapLevel">地图层级</param>
    /// <returns>地图文件名</returns>
    public static string GetMapFileNameById(string mapId, int mapLevel)
    {
        return MapNameHead + mapId + MapNameLevel + mapLevel;
    }


    /// <summary>
    /// 分解文件内容
    /// </summary>
    /// <param name="data">被分解数据</param>
    /// <returns>分解后的文件对照表(文件名, 文件内容)</returns>
    public static Dictionary<string, string> DepartFileData(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return null;
        }
        var result = new Dictionary<string, string>();

        // 解析数据
        var filesDataArray = data.Split(new[] { "%---%" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var fileData in filesDataArray)
        {
            var dataDepart = fileData.Split(new[] { "%--%" }, StringSplitOptions.RemoveEmptyEntries);
            if (dataDepart.Length == 2)
            {
                var fileName = dataDepart[0];
                var fileInfo = dataDepart[1];
                result.Add(fileName, fileInfo);
            }
        }

        return result;
    }



    /// <summary>
    /// 从两个目录中加载文件, 如果persistentDataPath中存在文件则加载, 否则从streamingAssetsPath中加载
    /// </summary>
    /// <param name="path">文件路径(在目录中的结构)</param>
    /// <returns>文件内容</returns>
    public static string LoadFileRotate(string path)
    {
        string result = null;
        FileInfo fi = new FileInfo(Path.Combine(Application.persistentDataPath, path));
        if (fi.Exists)
        {
            result = LoadFileInfo(fi);
        }
        else
        {
            fi = new FileInfo(Path.Combine(Application.streamingAssetsPath, path));
            // 继续加载
            if (fi.Exists)
            {
                result = LoadFileInfo(fi);
            }
        }

        return result;
    }



    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="fi">文件信息类</param>
    /// <returns>文件内容, 如果文件不存在则返回null</returns>
    public static string LoadFileInfo(FileInfo fi)
    {
        string result = null;
        if (fi != null)
        {
            StreamReader sr;
            if (fi.Exists)
            {
                sr = new StreamReader(fi.OpenRead());
                result = sr.ReadToEnd();
                sr.Close();
            }
        }

        return result;
    }


    // ---------------------------图形-------------------------------

    /// <summary>
    /// 获取矩形水平检测线
    /// </summary>
    /// <param name="rotation">旋转角度-360-360°</param>
    /// <returns>水平检测线标量</returns>
    public static Vector2 GetHorizonalTestLine(float rotation)
    {
        var angle = rotation * AngleToPi;
        return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
    }

    /// <summary>
    /// 获取矩形垂直检测线
    /// </summary>
    /// <param name="rotation">旋转角度-360-360°</param>
    /// <param name="radius">检测线长度</param>
    /// <returns>垂直检测线标量</returns>
    public static Vector2 GetVerticalTestLine(float rotation, float radius = 1f)
    {
        var angle = rotation * AngleToPi;
        return new Vector2(-(float)Math.Sin(angle) * radius, (float)Math.Cos(angle) * radius);
    }

    /// <summary>
    /// 绘制矩形
    /// </summary>
    /// <param name="position">举行中心位置</param>
    /// <param name="width">举行宽度</param>
    /// <param name="height">举行高度</param>
    /// <param name="rotation">基于矩形中心旋转角度</param>
    /// <param name="color">绘制颜色</param>
    public static void DrawRect(Vector3 position, float width, float height, float rotation, Color color)
    {
        var angle = rotation * AngleToPi;
        var halfWidth = width * 0.5f;
        var halfHeight = height * 0.5f;

        var sin = (float)(Math.Sin(angle));
        var cos = (float)(Math.Cos(angle));
        var left = (-halfWidth);
        var right = (halfWidth);
        var top = (halfHeight);
        var bottom = (-halfHeight);
        var point1 = new Vector3(left * cos - bottom * sin, left * sin + bottom * cos) + position;
        var point2 = new Vector3(left * cos - top * sin, left * sin + top * cos) + position;
        var point3 = new Vector3(right * cos - top * sin, right * sin + top * cos) + position;
        var point4 = new Vector3(right * cos - bottom * sin, right * sin + bottom * cos) + position;
        Debug.DrawLine(point1, point2, color);
        Debug.DrawLine(point2, point3, color);
        Debug.DrawLine(point3, point4, color);
        Debug.DrawLine(point4, point1, color);
    }
}