using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.AI.Neural
{
    public class Example2 : MonoBehaviour
    {
        /// <summary>
        /// 网络类
        /// </summary>
        public NeuralMono NeuralMono;

        /// <summary>
        /// 左侧探针
        /// </summary>
        public GameObject LeftSensor;

        /// <summary>
        /// 右侧探针
        /// </summary>
        public GameObject RightSensor;

        /// <summary>
        /// 训练次数
        /// </summary>
        public int TrainTimes = 10000;


        /// <summary>
        /// 起始位置
        /// </summary>
        private Vector3 startPos;

        /// <summary>
        /// 起始转向
        /// </summary>
        private Quaternion startRotate;


        /// <summary>
        /// 网络输入数据
        /// </summary>
        private float[] inData = new float[2];

        /// <summary>
        /// 网络输出数据
        /// </summary>
        private float[] outData = new float[1];



        void Awake()
        {
            startPos = transform.position;
            startRotate = transform.rotation;
        }


        void Update()
        {
            // 计算当前位置与墙的距离

            // 向左侧探针前方发射射线
            inData[0] = GetDistance(LeftSensor);

            // 向右侧探针前方发射射线
            inData[1] = GetDistance(RightSensor);


            if (inData[0] < 0.00001)
            {
                // 如果碰到左侧墙壁
                // 重置位置
                transform.position = startPos;
                transform.rotation = startRotate;
                // 训练网络
                for (var i = 0; i < TrainTimes; i++)
                {
                    NeuralMono.Train(inData, new[] {1f});
                }
            }
            else if (inData[1] < 0.00001)
            {
                // 如果碰到右侧墙壁
                // 重置位置
                transform.position = startPos;
                transform.rotation = startRotate;
                // 训练网络
                for (var i = 0; i < TrainTimes; i++)
                {
                    NeuralMono.Train(inData, new[] { 0f });
                }
            }


            // 计算网络
            outData = NeuralMono.Calculate(inData);
            print(outData[0]);

            // 判断转向
            if (outData[0] < 0.2f)
            {
                transform.Rotate(0, ((outData[0] / 1) * 0.2f) * -50 * Time.deltaTime, 0);
            }
            else if(outData[0] > 0.7f)
            {
                transform.Rotate(0, ((outData[0] / 1) * 0.2f + 0.8f) * 50 * Time.deltaTime, 0);
            }

            // 位置前进
            transform.Translate(0, 0, 0.2f);


        }


        private float GetDistance(GameObject obj)
        {
            var result = 0f;

            Ray ray = new Ray(obj.transform.position, obj.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                result = hit.distance;
            }

            return result;
        }
    }
}
