﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.AI.Neural
{

    /// <summary>
    /// 神经节点
    /// </summary>
    public class NeuronNode
    {
        /// <summary>
        /// 学习率
        /// </summary>
        public static float StudyRate = 0.2f;

        /// <summary>
        /// 目标神经连接列表
        /// </summary>
        public List<NeuronConnection> TargetList = new List<NeuronConnection>();

        /// <summary>
        /// 来源神经网络连接列表
        /// </summary>
        public List<NeuronConnection> FromList = new List<NeuronConnection>();


        /// <summary>
        /// 输入值
        /// </summary>
        public float InputValue;

        /// <summary>
        /// 期望值
        /// </summary>
        public float DesireValue;

        /// <summary>
        /// 误差值
        /// </summary>
        public float Error;

        /// <summary>
        /// 输出值
        /// </summary>
        public float Value;



        /// <summary>
        /// 随机值
        /// </summary>
        private static Random random = new Random();


        /// <summary>
        /// 连接神经元
        /// </summary>
        /// <param name="node">目标神经</param>
        public void ConnectNeuron(NeuronNode node)
        {
            var weight = (float)(random.NextDouble() - random.NextDouble());
            TargetList.Add(new NeuronConnection() { NeuronNode = node, weight = weight });
            node.FromList.Add(new NeuronConnection() { NeuronNode = node, weight = weight });
        }


        /// <summary>
        /// 计算值
        /// </summary>
        public void UpdateValue()
        {
            Value = CalculateValue();
        }

        /// <summary>
        /// 计算误差
        /// </summary>
        public void UpdateError()
        {
            Error = CalculateError();
        }

        /// <summary>
        /// 计算权重
        /// </summary>
        public void UpdateWeight()
        {
            for (var i = 0; i < FromList.Count; i++)
            {
                FromList[i].weight = FromList[i].NeuronNode.Value * Error * StudyRate;
            }
        }



        /// <summary>
        /// 计算值
        /// </summary>
        /// <returns>计算结果</returns>
        private float CalculateValue()
        {
            // 输入层
            if (FromList.Count == 0)
            {
                return InputValue;
            }

            // 输出层

            if (FromList.Count == 0)
            {
                return (DesireValue - Value) * (Value / (1 - Value));
            }

            // 隐层
            var result = 0f;

            // 遍历连接层乘权求和
            for (var i = 0; i < FromList.Count; i++)
            {
                result += FromList[i].weight * FromList[i].NeuronNode.Value;
            }
            // signod
            return (float)(1f / (1f +Math.Pow(Math.E, -result)));
        }


        /// <summary>
        /// 计算误差
        /// </summary>
        private float CalculateError()
        {
            // 输入层
            if (TargetList.Count == 0)
            {
                return 0;
            }

            // 非输入层
            var result = 0f;

            for (var i = 0; i < TargetList.Count; i++)
            {
                result += TargetList[i].weight * TargetList[i].NeuronNode.Error;
            }

            return result * (Value / (1 - Value));
        }






    }


    /// <summary>
    /// 神经连接
    /// </summary>
    public class NeuronConnection
    {
        /// <summary>
        /// 连接目标节点
        /// </summary>
        public NeuronNode NeuronNode;

        /// <summary>
        /// 连接权重
        /// </summary>
        public float weight;
    }
}
