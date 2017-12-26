using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using Util;


/// <summary>
/// 攻击管理器
/// </summary>
public class AttackManager : SingleItem<AttackManager>
{

    /// <summary>
    /// 攻击生成类字典
    /// </summary>
    private Dictionary<int, AttackMaker> attackMakerDic = new Dictionary<int, AttackMaker>();

    /// <summary>
    /// 生成一个单位的攻击类
    /// </summary>
    /// <param name="attacker">攻击单位</param>
    /// <param name="target">被攻击单位</param>
    /// <returns>攻击生成类</returns>
    public AttackMaker CreateAttackMaker([NotNull]DisplayOwner attacker, [NotNull]List<DisplayOwner> target)
    {
        AttackMaker result = null;
        if (!attackMakerDic.ContainsKey(attacker.Id))
        {
            result = new AttackMaker(attacker, target);
            attackMakerDic.Add(attacker.Id, result);
        }
        else
        {
            Debug.LogError("已存在该单位的攻击:" + attacker.Id + ":" + attacker.ClusterData.MapCellObj.name);
        }

        return result;
    }

    /// <summary>
    /// 获取攻击生成类
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public AttackMaker GetAttackMaker(int id)
    {
        AttackMaker result = null;

        if (attackMakerDic.ContainsKey(id))
        {
            result = attackMakerDic[id];
        }

        return result;
    }

    /// <summary>
    /// 删除攻击类
    /// </summary>
    /// <param name="id"></param>
    public void Remove(int id)
    {
        if (attackMakerDic.ContainsKey(id))
        {
            attackMakerDic.Remove(id);
        }
    }

    /// <summary>
    /// 清理
    /// </summary>
    public void Clear()
    {
        foreach (var attackMaker in attackMakerDic)
        {
            attackMaker.Value.Destory();
        }
        attackMakerDic.Clear();
    }
}


/// <summary>
/// 攻击类生成类
/// </summary>
public class AttackMaker
{
    /// <summary>
    /// 攻击计时器
    /// </summary>
    private Timer attackTimer = null;

    /// <summary>
    /// 攻击者
    /// </summary>
    private DisplayOwner attacker;
    
    /// <summary>
    /// 目标
    /// </summary>
    private List<DisplayOwner> targetList;

    /// <summary>
    /// 攻击生成类
    /// </summary>
    public AttackMaker([NotNull]DisplayOwner attacker, [NotNull]List<DisplayOwner> targetList)
    {
        this.attacker = attacker;
        this.targetList = targetList;
    }

    /// <summary>
    /// 开始攻击
    /// </summary>
    public void Begin()
    {
        if (attackTimer != null)
        {
            attackTimer.Start();
        }
        else
        {
            attackTimer = new Timer(attacker.ClusterData.AllData.MemberData.AttackRate);
            attackTimer.OnCompleteCallback(() =>
            {
                Attack(attacker, targetList);
            });
        }
    }


    /// <summary>
    /// 暂停
    /// </summary>
    public void Parse()
    {
        attackTimer.Pause();
    }

    /// <summary>
    /// 停止攻击
    /// </summary>
    public void Stop()
    {
        attackTimer.Kill();
        attackTimer = null;
    }

    /// <summary>
    /// 销毁前清理数据
    /// </summary>
    public void Destory()
    {
        Stop();
        attacker = null;
        targetList = null;
        AttackManager.Single.
    }

    /// <summary>
    /// 攻击
    /// </summary>
    /// <param name="attackerParam">攻击者</param>
    /// <param name="targetListParam">目标</param>
    private void Attack([NotNull]DisplayOwner attackerParam, [NotNull]List<DisplayOwner> targetListParam)
    {
        var myClusterData = attackerParam.ClusterData;
        var myMemberData = myClusterData.AllData.MemberData;

         // 单位转向目标
         myClusterData.RotateToWithoutYAxis(myClusterData.MapCellObj.transform.position);


         // 重置攻击时间间隔
         if (attackTimer.LoopTime != attackerParam.ClusterData.AllData.MemberData.AttackRate)
         {
             attackTimer.LoopTime = attackerParam.ClusterData.AllData.MemberData.AttackRate;
         }

         // 如果能攻击
         if (myMemberData.CouldNormalAttack)
         {
             foreach (var target in targetListParam)
             {
                 // 抛出攻击事件
                 SkillManager.Single.SetTriggerData(new TriggerData()
                 {
                     ReceiveMember = target,
                     ReleaseMember = attackerParam,
                     TypeLevel1 = TriggerLevel1.Fight,
                     TypeLevel2 = TriggerLevel2.Attack
                 });

                 // 发射子弹
                 ShootBullet(attackerParam, target);
             }

             // 攻击动作
             //SwitchAnim(fsm, SoldierAnimConst.GONGJI, WrapMode.Once);
         }
    }


    /// <summary>
    /// 发射子弹
    /// </summary>
    private void ShootBullet([NotNull] DisplayOwner attackerParam, [NotNull] DisplayOwner targetParam)
    {

        var enemyClusterData = targetParam.ClusterData;
        var myClusterData = attackerParam.ClusterData;
        var myMemberData = myClusterData.AllData.MemberData;
        var effect = myClusterData.AllData.EffectData;

        // 如果攻击方的攻击方式不为普通攻击的读取攻击表, 获取对应攻击方式
        IGeneralAttack normalGeneralAttack = null;
        switch (myMemberData.AttackType)
        {
            case Utils.BulletTypeNormal:
                // TODO 攻击带属性
                var theFiveProperties = attackerParam.ClusterData.MapCell.GetTheFiveProperties();
                // TODO 
                normalGeneralAttack = GeneralAttackManager.Single
                    .GetNormalGeneralAttack(myClusterData,
                        enemyClusterData,
                        effect.Bullet,
                        myClusterData.MapCellObj.transform.position,
                        enemyClusterData.MapCellObj,
                        myMemberData.BulletSpeed,
                        TrajectoryAlgorithmType.Line,
                        (obj) =>
                        {
                            Debug.Log("普通攻击");
                        });
                break;
            //case Utils.BulletTypeScope:
            //    // 获取
            //    //Debug.Log("AOE");
            //    var armyAOE = myClusterData.AllData.AOEData;
            //    // 根据不同攻击类型获取不同数据
            //    switch (armyAOE.AOEAim)
            //    {
            //        case Utils.AOEObjScope:
            //            normalGeneralAttack = GeneralAttackManager.Single.GetPointToObjScopeGeneralAttack(myClusterData,
            //                new[] {effect.Bullet, effect.RangeEffect},
            //                myClusterData.transform.position,
            //                enemyClusterData.gameObject,
            //                armyAOE.AOERadius,
            //                myMemberData.BulletSpeed,
            //                1, //effect.EffectTime,
            //                (TrajectoryAlgorithmType)
            //                    Enum.Parse(typeof (TrajectoryAlgorithmType), effect.TrajectoryEffect),
            //                () =>
            //                {
            //                    //Debug.Log("AOE Attack1");
            //                });
            //            break;
            //        case Utils.AOEPointScope:
            //            normalGeneralAttack =
            //                GeneralAttackManager.Single.GetPointToPositionScopeGeneralAttack(myClusterData,
            //                    myClusterData.transform.position,
            //                    enemyClusterData.transform.position,
            //                    armyAOE.AOERadius,
            //                    myMemberData.BulletSpeed,
            //                    (TrajectoryAlgorithmType)
            //                        Enum.Parse(typeof (TrajectoryAlgorithmType), effect.TrajectoryEffect),
            //                    () =>
            //                    {
            //                        //Debug.Log("AOE Attack2");
            //                    });
            //            break;
            //        case Utils.AOEScope:
            //            normalGeneralAttack = GeneralAttackManager.Single.GetPositionScopeGeneralAttack(myClusterData,
            //                effect.RangeEffect,
            //                myClusterData.transform.position,
            //                new CircleGraphics(new Vector2(myClusterData.X, myClusterData.Y), armyAOE.AOERadius),
            //                1, //effect.EffectTime,
            //                () =>
            //                {
            //                    //Debug.Log("AOE Attack3");
            //                });
            //            break;
            //        case Utils.AOEForwardScope:
            //            normalGeneralAttack =
            //                GeneralAttackManager.Single.GetPositionRectScopeGeneralAttack(myClusterData,
            //                    effect.RangeEffect,
            //                    myClusterData.transform.position,
            //                    armyAOE.AOEWidth,
            //                    armyAOE.AOEHeight,
            //                    Vector2.Angle(Vector2.up,
            //                        new Vector2(myClusterData.transform.forward.x, myClusterData.transform.forward.z)),
            //                    1, //effect.EffectTime,
            //                    () =>
            //                    {
            //                        //Debug.Log("AOE Attack4");
            //                        // 播放目标的受击特效
            //                    });
            //            break;
            //    }
            //    break;
        }

        if (normalGeneralAttack != null)
        {
            normalGeneralAttack.Begin();
        }
    }
}