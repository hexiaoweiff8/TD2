using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.AI.Neural
{
    /// <summary>
    /// 测试类
    /// </summary>
    public class Example : MonoBehaviour
    {
        /// <summary>
        /// 主相机
        /// </summary>
        public Camera MainCamere;

        /// <summary>
        /// 主地面
        /// </summary>
        public GameObject MainPlane;

        /// <summary>
        /// 球
        /// </summary>
        public GameObject Ball;

        /// <summary>
        /// 地图宽
        /// </summary>
        public int Width = 10;

        /// <summary>
        /// 地图长
        /// </summary>
        public int Height = 10;

        /// <summary>
        /// 训练次数
        /// </summary>
        public int TrainTime = 10000;

        /// <summary>
        /// 平台单位长度
        /// </summary>
        public const int PlaneHalfUnit = 5;


        /// <summary>
        /// 位置映射数组
        /// </summary>
        private int[] positionMappingArray;

        /// <summary>
        /// 最后球所在位置
        /// </summary>
        private int lastPos = -1;

        /// <summary>
        /// 是否在平台上
        /// </summary>
        private bool onTheTable = false;


        void Awake()
        {
            positionMappingArray = new int[Width];
        }




        void Update()
        {
            // 起始位置
            var positon = Ball.transform.position;

            //Ray ray = new Ray();
            //var scale = MainPlane.transform.localScale;

            onTheTable = false;

            // 检测球体位置
            // 射线更新
            //for (var i = 0; i < Height; i++)
            //{
            for (var j = 0; j < Width; j++)
            {
                //// 发射射线
                //ray.direction = new Vector3();
                //ray.origin = new Vector3();
                if (j - PlaneHalfUnit < positon.x && j - PlaneHalfUnit + 1 > positon.x)
                {
                    positionMappingArray[j] = 1;
                    lastPos = j;
                    onTheTable = true;
                }
                else
                {
                    positionMappingArray[j] = 0;
                }
            }
            //}

            // 如果球没有在范围内则判断球掉下去了, 从上次掉下去位置判断从哪里掉下去的, 训练网络
            if (!onTheTable)
            {
                // 重置场景, 训练网络

            }
            else
            {
                // 输入数据


                // 判断输出


                // 调整平台
            }
        }
    }
}
