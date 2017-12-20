using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


/// <summary>
/// FightUnit
/// </summary>
public class FightUnit : MemberCellBase
{
    /// <summary>
    /// 初始化战斗单位
    /// </summary>
    /// <param name="obj">游戏物体</param>
    /// <param name="dataId">数据Id</param>
    /// <param name="drawLayer">绘制层级</param>
    public FightUnit(GameObject obj, int dataId, int drawLayer)
        : base(obj, dataId, drawLayer)
    {
        MapCellType = UnitType.FightUnit;
    }


    public override void Draw(Vector3 leftdown, int unitWidth)
    {
        // 检测缩放, 不进行位置控制
        CheckScale(unitWidth);
        //base.Draw(leftdown, unitWidth);
    }

    public override List<DisplayOwner> CheckAttack()
    {
        throw new NotImplementedException();
    }

    public override List<DisplayOwner> CheckSkill()
    {
        throw new NotImplementedException();
    }

    public override void Attack(List<DisplayOwner> targetList)
    {
        throw new NotImplementedException();
    }

    public override void StopAttack()
    {
        throw new NotImplementedException();
    }

    public override void Skill(List<DisplayOwner> targetList)
    {
        throw new NotImplementedException();
    }

    public override void StopSkill()
    {
        throw new NotImplementedException();
    }

    public override void Move(List<Vector3> targetList)
    {
        throw new NotImplementedException();
    }

    public override void StopMove()
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 成员cell
/// </summary>
public abstract class MemberCellBase : MapCellBase
{

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="dataId"></param>
    /// <param name="drawLayer"></param>
    protected MemberCellBase(GameObject obj, int dataId, int drawLayer) : base(obj, dataId, drawLayer)
    {

    }


    /// <summary>
    /// 检测攻击
    /// </summary>
    /// <returns></returns>
    public abstract List<DisplayOwner> CheckAttack();

    /// <summary>
    /// 检测技能释放
    /// </summary>
    /// <returns></returns>
    public abstract List<DisplayOwner> CheckSkill();



    /// <summary>
    /// 攻击目标
    /// </summary>
    /// <param name="targetList">目标列表</param>
    public abstract void Attack(List<DisplayOwner> targetList);

    /// <summary>
    /// 停止攻击
    /// </summary>
    public abstract void StopAttack();


    /// <summary>
    /// 释放技能
    /// </summary>
    /// <param name="targetList">目标列表</param>
    public abstract void Skill(List<DisplayOwner> targetList);

    /// <summary>
    /// 停止释放技能
    /// </summary>
    public abstract void StopSkill();


    /// <summary>
    /// 开始移动
    /// </summary>
    /// <param name="targetList">目标列表</param>
    public abstract void Move(List<Vector3> targetList);

    /// <summary>
    /// 停止移动
    /// </summary>
    public abstract void StopMove();


}