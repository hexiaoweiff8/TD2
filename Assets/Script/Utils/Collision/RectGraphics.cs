﻿using System;
using UnityEngine;


/// <summary>
/// 矩形图形
/// </summary>
public class RectGraphics : CollisionGraphics
{

    /// <summary>
    /// 举行宽度
    /// </summary>
    public float Width { get; set; }

    /// <summary>
    /// 举行高度
    /// </summary>
    public float Height { get; set; }

    /// <summary>
    /// 旋转角度
    /// </summary>
    public float Rotation
    {
        get { return rotation; }
        set
        {
            rotation = value;
            //if (rotation > 360 || rotation < 360)
            //{
            //    rotation %= 360;
            //}

            // 重置相对水平轴与相对垂直轴
            HorizonalAxis = Utils.GetHorizonalTestLine(rotation);
            VerticalAxis = Utils.GetVerticalTestLine(rotation);

            // 求对角线
            var angle = rotation * Utils.AngleToPi;
            var cos = (float)Math.Cos(angle);
            var sin = (float)Math.Sin(angle);
            Diagonal1 = new Vector2(Width * cos - Height * sin, Width * sin + Height * cos);
            Diagonal2 = new Vector2(-Width * cos - Height * sin, -Width * sin + Height * cos);
            //Debug.DrawLine(Vector3.zero, new Vector3(Diagonal1.x, 0, Diagonal1.y));
            //Debug.DrawLine(Vector3.zero, new Vector3(Diagonal2.x, 0, Diagonal2.y));

        }
    }


    /// <summary>
    /// 相对水平轴
    /// </summary>
    public Vector2 HorizonalAxis { get; private set; }

    /// <summary>
    /// 相对垂直轴
    /// </summary>
    public Vector2 VerticalAxis { get; private set; }

    /// <summary>
    /// 对角线2
    /// </summary>
    public Vector2 Diagonal1 { get; private set; }

    /// <summary>
    /// 对角线1
    /// </summary>
    public Vector2 Diagonal2 { get; private set; }


    /// <summary>
    /// 旋转角度
    /// </summary>
    private float rotation = 0f;


    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="width">宽度</param>
    /// <param name="height">高度</param>
    /// <param name="rotation">旋转角度</param>
    public RectGraphics(Vector2 position, float width, float height, float rotation)
    {
        Postion = position;
        Width = width;
        Height = height;
        graphicType = GraphicType.Rect;
        Rotation = rotation;
    }

    /// <summary>
    /// 获取正方向外接矩形
    /// </summary>
    /// <returns>外接矩形</returns>
    public override RectGraphics GetExternalRect()
    {
        return this;
    }

}

