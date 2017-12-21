using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


/// <summary>
/// FightUnit
/// </summary>
public class FightUnit : MapCellBase, IMember
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

    public List<DisplayOwner> CheckAttack()
    {
        throw new NotImplementedException();
    }

    public List<DisplayOwner> CheckSkill()
    {
        throw new NotImplementedException();
    }

    public void Attack(List<DisplayOwner> targetList)
    {
        throw new NotImplementedException();
    }

    public void StopAttack()
    {
        throw new NotImplementedException();
    }

    public void Skill(List<DisplayOwner> targetList)
    {
        throw new NotImplementedException();
    }

    public void StopSkill()
    {
        throw new NotImplementedException();
    }

    public void Move(List<Vector3> targetList)
    {
        throw new NotImplementedException();
    }

    public void StopMove()
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 成员接口
/// </summary>
public interface IMember 
{



    /// <summary>
    /// 检测攻击
    /// </summary>
    /// <returns></returns>
    List<DisplayOwner> CheckAttack();

    /// <summary>
    /// 检测技能释放
    /// </summary>
    /// <returns></returns>
    List<DisplayOwner> CheckSkill();



    /// <summary>
    /// 攻击目标
    /// </summary>
    /// <param name="targetList">目标列表</param>
    void Attack(List<DisplayOwner> targetList);

    /// <summary>
    /// 停止攻击
    /// </summary>
    void StopAttack();


    /// <summary>
    /// 释放技能
    /// </summary>
    /// <param name="targetList">目标列表</param>
    void Skill(List<DisplayOwner> targetList);

    /// <summary>
    /// 停止释放技能
    /// </summary>
    void StopSkill();


    /// <summary>
    /// 开始移动
    /// </summary>
    /// <param name="targetList">目标列表</param>
    void Move(List<Vector3> targetList);

    /// <summary>
    /// 停止移动
    /// </summary>
    void StopMove();


}