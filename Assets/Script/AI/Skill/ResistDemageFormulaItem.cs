﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class ResistDemageFormulaItem : AbstractFormulaItem
{
    /// <summary>
    /// 伤害吸收量
    /// </summary>
    public float ResistDemage { get; set; }

    /// <summary>
    /// 吸收伤害百分比
    /// 百分比
    /// 1为100%
    /// 0为0%
    /// 0-1区间
    /// </summary>
    public float ResistPercentage { get; set; }

    /// <summary>
    /// 是否吸收过量伤害
    /// 能吸收100伤害
    /// 收到1000伤害
    /// 剩下900伤害是否扣除
    /// </summary>
    public bool IsResistOverflowDemage { get; set; }



    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="array">数据列表</param>
    public ResistDemageFormulaItem(string[] array)
    {
        if (array == null)
        {
            throw new Exception("数据列表为空");
        }

        var argsCount = 4;
        // 解析参数
        if (array.Length < argsCount)
        {
            throw new Exception("参数数量错误.需求参数数量:" + argsCount + " 实际数量:" + array.Length);
        }

        FormulaType = GetDataOrReplace<int>("FormulaType", array, 0);
        ResistDemage = GetDataOrReplace<float>("ResistDemage", array, 1);
        ResistPercentage = GetDataOrReplace<float>("ResistPercentage", array, 2);
        IsResistOverflowDemage = GetDataOrReplace<bool>("IsResistOverflowDemage", array, 3);
    }



    /// <summary>
    /// 生成行为节点
    /// </summary>
    /// <param name="paramsPacker"></param>
    /// <returns></returns>
    public override IFormula GetFormula(FormulaParamsPacker paramsPacker)
    {
        IFormula result = null;

        // 替换数据
        ReplaceData(paramsPacker);
        // 数据本地化
        var myFormulaType = FormulaType;
        var myResistDemage = ResistDemage;
        var myResistPercentage = ResistPercentage;
        var myIsResistOverflowDemage = IsResistOverflowDemage;
        var mySkill = paramsPacker.Skill;
        var myTrigger = paramsPacker.TriggerData;

        result = new Formula((callback, scope) =>
        {
            callback();
        },
            myFormulaType);

        Debug.Log("构造伤害吸收行为");
        if (!mySkill.DataScope.HasData<float>("ResistDemage"))
        {
            // 缓存数据
            mySkill.DataScope.SetData("ResistDemage", myResistDemage);
            mySkill.DataScope.SetData("AllResistDemage", myResistDemage);
        }
        Debug.Log("当前生命值变动量:" + myTrigger.HealthChangeValue);

        // 无伤害可吸收
        if (myTrigger.HealthChangeValue <= 0)
        {
            return result;
        }
        var nowCouldResistDemage = mySkill.DataScope.GetData<float>("ResistDemage");
        // 伤害吸收结束
        if (nowCouldResistDemage <= 0)
        {
            return result;
        }
        var needResistDemage = myTrigger.HealthChangeValue*myResistPercentage;
        var absNeedRessitDemage = Math.Abs(needResistDemage);
        if (absNeedRessitDemage > nowCouldResistDemage)
        {
            if (myIsResistOverflowDemage)
            {
                myTrigger.HealthChangeValue = 0;
            }
            else
            {
                myTrigger.HealthChangeValue -= nowCouldResistDemage;
            }
            // 清空伤害吸收
            mySkill.DataScope.SetData("ResistDemage", 0);
        }
        else
        {
            Debug.Log("吸收伤害");
            myTrigger.HealthChangeValue -= needResistDemage;
            nowCouldResistDemage -= absNeedRessitDemage;

            Debug.Log("剩余伤害:" + myTrigger.HealthChangeValue);
            // 设置剩余伤害量
            mySkill.DataScope.SetData("ResistDemage", nowCouldResistDemage);
        }

        // 判断是否到达伤害吸收上限
        if (nowCouldResistDemage == 0)
        {
            // 执行子级技能
            if (SubFormulaItem != null)
            {
                var packer = new FormulaParamsPacker();
                FormulaParamsPackerFactroy.Single.CopyPackerData(paramsPacker, packer);
                var subSkill = new SkillInfo(packer.SkillNum);
                subSkill.DataList = packer.DataList;
                subSkill.AddActionFormulaItem(SubFormulaItem);
                SkillManager.Single.DoSkillInfo(subSkill, packer, true);
            }
        }
        // 伤害被吸收
        myTrigger.IsAbsorption = true;


        return result;
    }
}