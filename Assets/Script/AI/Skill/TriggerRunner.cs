﻿//using System;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 事件处理器
///// </summary>
//public class TriggerRunner : MonoBehaviour
//{
//    /// <summary>
//    /// 单位数据
//    /// </summary>
//    public DisplayOwner Display;

//    /// <summary>
//    /// 临时事件列表
//    /// </summary>
//    public List<TriggerData> tmpList = new List<TriggerData>();

//    /// <summary>
//    /// 结算伤害/治疗
//    /// </summary>
//    private Action<TriggerLevel1, TriggerLevel2, TriggerData, AllData> settlementDamageOrCure;


//    private void Awake()
//    {
//        // 初始化事件
//        settlementDamageOrCure = (type1, type2, trigger, alldata) =>
//        {
//            //Debug.Log("伤害结算");
//            var isChange = false;
//            // 治疗结算
//            if (type1 == TriggerLevel1.Fight && type2 == TriggerLevel2.BeCure)
//            {
//                alldata.MemberData.CurrentHP += trigger.HealthChangeValue;
//                isChange = true;
//                // 统计受到的治疗量
//                FightDataStatistical.Single.AddHealthChange("" + alldata.MemberData.ObjID.ID, trigger.HealthChangeValue,
//                    alldata.MemberData.Camp, DemageOrCure.Cure, trigger.DemageType);

//                // 统计产生的治疗量
//                FightDataStatistical.Single.AddHealthChange(
//                    "" + trigger.ReleaseMember.ClusterData.AllData.MemberData.ObjID.ID, trigger.HealthChangeValue,
//                    trigger.ReleaseMember.ClusterData.AllData.MemberData.Camp, DemageOrCure.Cure, trigger.DemageType,
//                    AttackOrBeAttach.Attack);

//                // 抛出治疗事件给外层
//                FightManager.Single.DoHealthChangeAction(trigger.ReleaseMember.GameObj, alldata.MemberData.TotalHp,
//                    alldata.MemberData.CurrentHP, trigger.HealthChangeValue, FightManager.HurtType.Cure,
//                    alldata.MemberData.ObjID.ObjType);

//            }
//            // 伤害结算
//            if (type1 == TriggerLevel1.Fight && type2 == TriggerLevel2.BeAttack)
//            {
//                //Debug.Log("结算生命值:" + trigger.HealthChangeValue);
//                alldata.MemberData.SetCurrentHP(alldata.MemberData.CurrentHP - trigger.HealthChangeValue);

//                // 统计受到的伤害量
//                FightDataStatistical.Single.AddHealthChange("" + alldata.MemberData.ObjID.ID, trigger.HealthChangeValue,
//                    alldata.MemberData.Camp, DemageOrCure.Demage, trigger.DemageType);

//                // 统计产生的伤害量
//                FightDataStatistical.Single.AddHealthChange(
//                    "" + trigger.ReleaseMember.ClusterData.AllData.MemberData.ObjID.ID, trigger.HealthChangeValue,
//                    trigger.ReleaseMember.ClusterData.AllData.MemberData.Camp, DemageOrCure.Demage, trigger.DemageType,
//                    AttackOrBeAttach.Attack);

//                // 转换伤害类型
//                var hurtType = FightManager.GetDemageHurtType(trigger);
//                // 抛出伤害事件给外层
//                FightManager.Single.DoHealthChangeAction(trigger.ReleaseMember.GameObj, alldata.MemberData.TotalHp,
//                    alldata.MemberData.CurrentHP, trigger.HealthChangeValue, hurtType,
//                    alldata.MemberData.ObjID.ObjType);
//                if (alldata.MemberData.CurrentHP < Utils.ApproachZero)
//                {
//                    alldata.MemberData.SetCurrentHP(0);
//                    // 并判断该伤害是否致死, 如果不致死则生命值设置为1
//                    if (trigger.IsNotLethal)
//                    {
//                        alldata.MemberData.SetCurrentHP(1);
//                    }
//                    else
//                    {
//                        // 抛出致死攻击事件
//                        tmpList.Add(new TriggerData()
//                        {
//                            HealthChangeValue = trigger.HealthChangeValue,
//                            ReceiveMember = trigger.ReceiveMember,
//                            ReleaseMember = trigger.ReleaseMember,
//                            TypeLevel1 = TriggerLevel1.Fight,
//                            TypeLevel2 = TriggerLevel2.LethalHit
//                        });

//                        // 统计杀敌数量
//                        FightDataStatistical.Single.AddKillCount("" + alldata.MemberData.ObjID.ID, 1, alldata.MemberData.Camp);
//                    }
//                }


//                // 是否有吸收伤害
//                if (trigger.IsAbsorption)
//                {
//                    // 检测是否有伤害吸收的buff/skill
//                    // 触发吸收伤害事件
//                    tmpList.Add(new TriggerData()
//                    {
//                        HealthChangeValue = trigger.HealthChangeValue,
//                        ReceiveMember = trigger.ReceiveMember,
//                        ReleaseMember = trigger.ReleaseMember,
//                        TypeLevel1 = TriggerLevel1.Fight,
//                        TypeLevel2 = TriggerLevel2.Absorption
//                    });
//                }
//                // 抛出伤害事件
//                if (trigger.HealthChangeValue > 0)
//                {
//                    tmpList.Add(new TriggerData()
//                    {
//                        HealthChangeValue = trigger.HealthChangeValue,
//                        ReceiveMember = trigger.ReceiveMember,
//                        ReleaseMember = trigger.ReleaseMember,
//                        TypeLevel1 = TriggerLevel1.Fight,
//                        TypeLevel2 = TriggerLevel2.BeHurt
//                    });
//                    // 伤害类型
//                    var attackType = TriggerLevel2.BeNormalAttackHurt;
//                    switch (trigger.DemageType)
//                    {
//                            // 普通伤害
//                        case DemageType.NormalAttackDemage:
//                            attackType = TriggerLevel2.BeNormalAttackHurt;
//                            break;
//                            // 技能伤害
//                        case DemageType.SkillAttackDemage:
//                            attackType = TriggerLevel2.BeSkillHurt;
//                            break;
//                    }
//                    tmpList.Add(new TriggerData()
//                    {
//                        HealthChangeValue = trigger.HealthChangeValue,
//                        ReceiveMember = trigger.ReceiveMember,
//                        ReleaseMember = trigger.ReleaseMember,
//                        TypeLevel1 = TriggerLevel1.Fight,
//                        TypeLevel2 = attackType
//                    });
//                }
//                isChange = true;
//            }

//            // 如果血量有变动则抛出血量变动事件
//            if (isChange)
//            {
//                tmpList.Add(new TriggerData()
//                {
//                    ReceiveMember = trigger.ReceiveMember,
//                    ReleaseMember = trigger.ReleaseMember,
//                    TypeLevel1 = TriggerLevel1.Fight,
//                    TypeLevel2 = TriggerLevel2.HealthChange
//                });
//            }
//        };
//    }


//    private void Update()
//    {
//        // 检查光环
//        CheckRemain(Display);
//        // 处理事件
//        CheckTrigger(Display);
//    }


//    /// <summary>
//    /// 检测当前单位的触发事件
//    /// </summary>
//    private void CheckTrigger(DisplayOwner display)
//    {
//        var allData = display.ClusterData.AllData;
//        if (allData.MemberData != null)
//        {
//            // 触发当前单位的所有事件
//            SkillManager.Single.SetEachAction(allData.MemberData.ObjID, (type1, type2, trigger) =>
//            {
//                if (allData.SkillInfoList != null)
//                {

//                    //Debug.Log("触发技能");
//                    // 触发skill类
//                    SkillManager.Single.CheckAndDoSkillInfo(allData.SkillInfoList, trigger);
//                }
//                if (allData.BuffInfoList != null)
//                {
//                    //Debug.Log("触发buff");
//                    // 技能触发完毕开始触发buff类
//                    BuffManager.Single.CheckAndDoBuffInfo(allData.BuffInfoList, trigger);
//                }
//                // 计算伤害
//                settlementDamageOrCure(type1, type2, trigger, allData);
//            }, true);
//            PushTriggerData();
//        }
//    }

//    /// <summary>
//    /// 检查范围技能
//    /// </summary>
//    /// <param name="display">被检查单位</param>
//    private void CheckRemain(DisplayOwner display)
//    {
//        // 执行范围技能
//        var allData = display.ClusterData.AllData;
//        if (allData.MemberData == null || allData.RemainInfoList.Count == 0) return;
//        //var memberPos = new Vector2(display.ClusterData.X, display.ClusterData.Y);
//        // 检查范围技能
//        for (var i = 0; i < allData.RemainInfoList.Count; i++)
//        {
//            var remain = allData.RemainInfoList[i];
//            if (remain.CheckRange())
//            {
//                // 如果范围技能已结束, 则删除
//                allData.RemainInfoList.RemoveAt(i);
//                i--;
//                RemainManager.Single.DelRemainInstance(remain.AddtionId);
//            }
//        }
//    }

//    /// <summary>
//    /// 将缓存的事件压入事件列表中
//    /// </summary>
//    private void PushTriggerData()
//    {
//        // 将事件压入
//        foreach (var item in tmpList)
//        {
//            SkillManager.Single.SetTriggerData(item);
//        }
//        tmpList.Clear();
//    }


//    private void OnDestroy()
//    {
//        // TODO 释放
//        // TODO 死亡时将未释放技能进行释放
//    }
//}

///// <summary>
///// 技能释放包装类
///// </summary>
//public class SkillReleasePacker
//{

//    /// <summary>
//    /// 技能释放者
//    /// </summary>
//    public DisplayOwner SkillReleaseMember { get; set; }

//    /// <summary>
//    /// 技能接受者
//    /// </summary>
//    public DisplayOwner SkillReceiveMember { get; set; }

//    /// <summary>
//    /// 被释放技能
//    /// </summary>
//    public SkillBase Skill { get; set; }

//}