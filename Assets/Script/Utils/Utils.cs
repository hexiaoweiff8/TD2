
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 工具类
/// </summary>
public class Utils
{

    /// <summary>
    /// 2转3
    /// </summary>
    /// <param name="vec2"></param>
    /// <returns></returns>
    public static Vector3 V2ToV3(Vector2 vec2)
    {
        return new Vector3(vec2.x, vec2.y);
    }
}