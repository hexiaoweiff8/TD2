﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;


/// <summary>
/// 能力基类
/// </summary>
public class AbilityBase
{
    /// <summary>
    /// ID
    /// </summary>
    public int Num { get; protected set; }

    /// <summary>
    /// 唯一自增ID
    /// </summary>
    public long AddtionId { get; protected set; }

    /// <summary>
    /// 保存技能等级数据列表
    /// </summary>
    public List<List<string>> DataList = new List<List<string>>();

    /// <summary>
    /// 技能的数据域
    /// </summary>
    public DataScope DataScope = new DataScope();

    /// <summary>
    /// 技能的持有者
    /// </summary>
    public DisplayOwner ReleaseMember { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    public int Level = 1;

    /// <summary>
    /// 被替换列表
    /// </summary>
    public Dictionary<string, string> ReplaceSourceDataDic = new Dictionary<string, string>();

    /// <summary>
    /// 能力共享数据
    /// 在执行流程内
    /// </summary>
    public Dictionary<string, string> ShareData = new Dictionary<string, string>();

    /// <summary>
    /// 技能唯一自增ID
    /// </summary>
    protected static long addtionId = 1024;


    public AbilityBase()
    {
        AddtionId = addtionId++;
    }


    

    /// <summary>
    /// buff触发行为
    /// </summary>
    protected IFormulaItem actionFormulaItem = null;

    /// <summary>
    /// buff 创建行为
    /// </summary>
    protected IFormulaItem attachFormulaItem = null;

    /// <summary>
    /// buff 结束行为
    /// </summary>
    protected IFormulaItem detachFormulaItem = null;



    /// <summary>
    /// 添加触发行为生成器
    /// </summary>
    /// <param name="formulaItem">行为单元生成器</param>
    public void AddActionFormulaItem(IFormulaItem formulaItem)
    {
        //if (formulaItem == null)
        //{
        //    throw new Exception("行为节点为空");
        //}
        if (actionFormulaItem == null)
        {
            actionFormulaItem = formulaItem;
        }
        else
        {
            actionFormulaItem.After(formulaItem);
        }
    }

    /// <summary>
    /// 添加创建行为生成器
    /// </summary>
    /// <param name="formulaItem">行为单元生成器</param>
    public void AddAttachFormulaItem(IFormulaItem formulaItem)
    {
        if (formulaItem == null)
        {
            throw new Exception("行为节点为空");
        }
        if (attachFormulaItem == null)
        {
            attachFormulaItem = formulaItem;
        }
        else
        {
            attachFormulaItem.After(formulaItem);
        }
    }

    /// <summary>
    /// 添加销毁行为生成器
    /// </summary>
    /// <param name="formulaItem">行为单元生成器</param>
    public void AddDetachFormulaItem(IFormulaItem formulaItem)
    {
        if (formulaItem == null)
        {
            throw new Exception("行为节点为空");
        }
        if (detachFormulaItem == null)
        {
            detachFormulaItem = formulaItem;
        }
        else
        {
            detachFormulaItem.After(formulaItem);
        }
    }


    /// <summary>
    /// 获取Action行为链生成器
    /// </summary>
    /// <returns></returns>
    public IFormulaItem GetActionFormulaItem()
    {
        return actionFormulaItem;
    }
    
    /// <summary>
    /// 获取Attach行为链生成器
    /// </summary>
    /// <returns></returns>
    public IFormulaItem GetAttachFormulaItem()
    {
        return attachFormulaItem;
    }

    /// <summary>
    /// 获取Detach行为链生成器
    /// </summary>
    /// <returns></returns>
    public IFormulaItem GetDetachFormulaItem()
    {
        return detachFormulaItem;
    }

    /// <summary>
    /// 获取buff创建时行为
    /// </summary>
    /// <param name="paramsPacker">构造行为数据</param>
    /// <returns>创建时行为链</returns>
    public IFormula GetAttachFormula(FormulaParamsPacker paramsPacker)
    {
        IFormula result = null;
        if (paramsPacker == null)
        {
            throw new Exception("参数封装为空.");
        }
        if (attachFormulaItem == null)
        {
            return result;
        }
        result = GetIFormula(paramsPacker, attachFormulaItem);

        return result;
    }


    /// <summary>
    /// 获取buff销毁行为
    /// </summary>
    /// <param name="paramsPacker">构造行为数据</param>
    /// <returns>销毁时行为链</returns>
    public IFormula GetDetachFormula(FormulaParamsPacker paramsPacker)
    {
        IFormula result = null;
        if (paramsPacker == null)
        {
            throw new Exception("参数封装为空.");
        }
        if (detachFormulaItem == null)
        {
            return result;
        }
        result = GetIFormula(paramsPacker, detachFormulaItem);
        //if (result != null)
        //{
        //    result.After(new Formula((callback) =>
        //    {
        //        Debug.Log("删除buff:" + addtionId);
        //        // 执行结束删除当前buff的具体实现引用
        //        BuffManager.Single.DelBuffInstance(AddtionId);
        //        callback();
        //    }));
        //}
        return result;
    }

    /// <summary>
    /// 获取buff触发行为
    /// </summary>
    /// <param name="paramsPacker">参数封装</param>
    /// <returns>行为链</returns>
    public IFormula GetActionFormula(FormulaParamsPacker paramsPacker)
    {
        if (paramsPacker == null)
        {
            throw new Exception("参数封装为空.");
        }
        IFormula result = null;

        if (actionFormulaItem == null)
        {
            return null;
        }
        result = GetIFormula(paramsPacker, actionFormulaItem);
        return result;
    }


    /// <summary>
    /// 获得数据或替换
    /// </summary>
    /// <param name="name">数据名称</param>
    /// <param name="val">数据</param>
    /// <returns>转换后数据</returns>
    public T GetDataOrReplace<T>(string name, string val)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception("字段名为空.");
        }
        if (string.IsNullOrEmpty(val))
        {
            throw new Exception("数据为空.");
        }
        //if (replaceDic == null)
        //{
        //    throw new Exception("替换列表为空.");
        //}

        var resultType = -1;

        var result = default(T);
        var type = typeof(T);
        var typeName = type.Name;
        if (typeName.Equals("Int32"))
        {
            resultType = 1;
        }
        if (typeName.Equals("Single"))
        {
            resultType = 2;
        }
        if (typeName.Equals("String"))
        {
            resultType = 3;
        }
        else if (result is Enum)
        {
            resultType = 4;
        }
        if (typeName.Equals("Boolean"))
        {
            resultType = 5;
        }
        if (typeName.Equals("FormulaItemValueComputer"))
        {
            resultType = 6;
        }
        var item = val;
        // 数据是否需要替换
        if (item.Contains("{%") || item.Contains("<"))
        {
            ReplaceSourceDataDic.Add(name, resultType + "--,--" + item);
        }
        else
        {
            switch (resultType)
            {
                case 1:
                    // int
                    result = (T)Convert.ChangeType(item, typeof(T));
                    break;
                case 2:
                    // float
                    result = (T)Convert.ChangeType(item, typeof(T));
                    break;
                case 3:
                    // string
                    result = (T)(object)item;
                    break;
                case 4:
                    // enum
                    result = (T)Enum.Parse(typeof(T), item);
                    break;
                case 5:
                    // bool
                    result = (T)Convert.ChangeType(item, typeof(T));
                    break;
                case 6:
                    // 数值计算类, 支持加减乘数公式
                    result = (T)Convert.ChangeType((new AbstractFormulaItem.FormulaItemValueComputer(item)), typeof(T));
                    break;
            }
        }


        return result;
    }

    /// <summary>
    /// 按照等级替换数据
    /// </summary>
    /// <param name="level">技能等级</param>
    public void ReplaceData(int level)
    {
        if (level <= 0)
        {
            throw new Exception("技能等级非法:" + level);
        }
        var type = this.GetType();

        // 遍历替换列表 如果存在数据则替换对应数据
        if (ReplaceSourceDataDic.Count > 0)
        {

            // 技能等级
            var skillLevel = level - 1;
            if (skillLevel < 0) skillLevel = 0;
            // 该等级的数据列表
            var dataRow = DataList[skillLevel];

            foreach (var item in ReplaceSourceDataDic)
            {
                var propertyName = item.Key;
                var value = Regex.Split(item.Value, "--,--");
                var itemType = Convert.ToInt32(value[0]);
                var strInfo = value[value.Length - 1];

                // 替换DataList中数据
                for (var i = 0; i < dataRow.Count; i++)
                {
                    strInfo = strInfo.Replace("{%" + i + "}", dataRow[i]);
                }

                var property = type.GetProperty(propertyName);
                if (property == null)
                {
                    throw new Exception("数据目标属性不存在:" + propertyName);
                }
                switch (itemType)
                {
                    case 1:
                        // int
                        property.SetValue(this, Convert.ToInt32(strInfo), null);
                        break;
                    case 2:
                        // float
                        property.SetValue(this, Convert.ToSingle(strInfo), null);
                        break;
                    case 3:
                        // string
                        property.SetValue(this, strInfo, null);
                        break;
                    case 4:
                        // 枚举
                        property.SetValue(this, Convert.ToInt32(strInfo), null);
                        break;
                    case 5:
                        // 枚举
                        property.SetValue(this, Convert.ToBoolean(strInfo), null);
                        break;
                    case 6:
                        // 计算公式类
                        property.SetValue(this, new AbstractFormulaItem.FormulaItemValueComputer(strInfo), null);
                        break;
                }
            }
        }
    }





    /// <summary>
    /// 获取行为链
    /// </summary>
    /// <param name="paramsPacker">构造数据</param>
    /// <param name="formulaItem">行为链构造器</param>
    /// <returns></returns>
    protected IFormula GetIFormula(FormulaParamsPacker paramsPacker, IFormulaItem formulaItem)
    {
        IFormula result = null;
        // 循环构建行为链构造器
        var tmpItem = formulaItem;
        // 数据列表放入packer中
        paramsPacker.DataList = DataList;
        // 技能ID放入packer中
        paramsPacker.SkillNum = Num;
        while (tmpItem != null)
        {
            if (result != null)
            {
                result = result.After(tmpItem.GetFormula(paramsPacker));
            }
            else
            {
                result = tmpItem.GetFormula(paramsPacker);
            }
            tmpItem = tmpItem.NextFormulaItem;
        }

        // 构造器不为空
        if (result != null)
        {
            // 获取构造器链head
            result = result.GetFirst();
            // 设置行为链链头的数据域
            result.DataScope = DataScope;
        }

        return result;
    }
}

/// <summary>
/// 技能基类
/// </summary>
public class SkillBase : AbilityBase
{

    /// <summary>
    /// 触发条件Level1
    /// </summary>
    public TriggerLevel1 TriggerLevel1 { get; set; }

    /// <summary>
    /// 触发条件Level2
    /// </summary>
    public TriggerLevel2 TriggerLevel2 { get; set; }

    /// <summary>
    /// 技能触发几率
    /// 0:触发概率0%
    /// 1:触发概率100%
    /// </summary>
    public float TriggerProbability {
        get { return triggerProbability; } set { triggerProbability = value; } }

    /// <summary>
    /// 生命区间下限
    /// </summary>
    public float HpScopeMin { get { return hpScopeMin; } set { hpScopeMin = value; } }

    /// <summary>
    /// 生命区间上限
    /// </summary>
    public float HpScopeMax { get { return hpScopeMax; } set { hpScopeMax = value; } }

    /// <summary>
    /// buff的Tick时间(单位 秒)
    /// 每次tick执行一次
    /// </summary>
    public float TickTime { get; set; }

    /// <summary>
    /// 伤害变更类型
    /// 0: 伤害增强
    /// 1: 伤害减免
    /// </summary>
    public int DemageChangeType { get; set; }

    /// <summary>
    /// 伤害附加概率
    /// </summary>
    public float DemageChangeProbability { get; set; }

    /// <summary>
    /// 伤害附加目标类型
    /// </summary>
    public DemageAdditionOrReductionTargetType DemageChangeTargetType { get; set; }

    /// <summary>
    /// 伤害附加
    /// </summary>
    public float DemageChange { get; set; }

    /// <summary>
    /// 修改数据
    /// 使用反射赋值
    /// </summary>
    public MemberData ChangeData { get; set; }

    /// <summary>
    /// 已经修改的数据总量
    /// 在buff/skill Detach时进行删除用
    /// </summary>
    public MemberData ChangedData { get; set; }

    /// <summary>
    /// 数据变更类型字典
    /// (数据变更字段, 数据变更类型0:绝对值(加),1:百分比(1+值 乘))
    /// </summary>
    public Dictionary<string, ChangeDataType> ChangeDataTypeDic = new Dictionary<string, ChangeDataType>();

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 技能触发几率
    /// 0:触发概率0%
    /// 1:触发概率100%
    /// </summary>
    private float triggerProbability = 1;

    /// <summary>
    /// 生命区间下限
    /// </summary>
    private float hpScopeMin = -1;

    /// <summary>
    /// 生命区间上限
    /// </summary>
    private float hpScopeMax = -1;



    public SkillBase() : base()
    {

    }

    /// <summary>
    /// 附加时的增加属性
    /// </summary>
    public static void AdditionAttribute(MemberData memberData, SkillBase skill)
    {
        if (memberData == null
            || skill == null
            || skill.ChangeData == null
            || skill.ChangeDataTypeDic == null)
        {
            return;
        }

        skill.ChangedData = new MemberData();

        // 获取可被变更的数据列表
        var propertyList = typeof(MemberData).GetProperties().Where((property) =>
        {
            if (property.GetCustomAttributes(typeof(SkillAddition), false).Any())
            {
                return true;
            }
            return false;
        });

        foreach (var property in propertyList)
        {
            AdditionProperty(property, memberData, skill.ChangeData, skill.ChangedData, skill.ChangeDataTypeDic);
        }

        var fieldList = typeof(MemberData).GetFields().Where((property) =>
        {
            if (property.GetCustomAttributes(typeof(SkillAddition), false).Any())
            {
                return true;
            }
            return false;
        });
        foreach (var field in fieldList)
        {
            AdditionField(field, memberData, skill.ChangeData, skill.ChangedData, skill.ChangeDataTypeDic);
        }
    }

    /// <summary>
    /// 加属性值
    /// </summary>
    /// <param name="property"></param>
    /// <param name="memberData"></param>
    public static void AdditionProperty(PropertyInfo property, MemberData memberData, MemberData changeData, MemberData changedData, IDictionary<string, ChangeDataType> changeDataTypeDic)
    {


        var propertyName = property.Name;
        ChangeDataType changeDataType = ChangeDataType.Absolute;
        // 读取赋值类型, 如果没有则默认使用绝对值
        if (changeDataTypeDic.ContainsKey(propertyName))
        {
            changeDataType = changeDataTypeDic[propertyName];
        }
        // 修改float类型属性
        if (property.PropertyType == typeof(float))
        {
            var sourceValue = Convert.ToSingle(property.GetValue(memberData, null));
            var plusValue = Convert.ToSingle(property.GetValue(changeData, null)) * (changeDataType == ChangeDataType.Absolute ? 1 : sourceValue);
            property.SetValue(memberData, sourceValue + plusValue, null);
            // 保存变更数据
            property.SetValue(changedData, plusValue, null);
        }
        // 修改bool类型属性
        else if (property.PropertyType == typeof(bool))
        {
            property.SetValue(memberData, Convert.ToBoolean(property.GetValue(changeData, null)), null);
            // 保存变更数据
            property.SetValue(changedData, property.GetValue(memberData, null), null);
        }
        // 修改int,short,long类型属性
        else if (property.PropertyType == typeof(long) || property.PropertyType == typeof(short) || property.PropertyType == typeof(int))
        {
            var sourceValue = Convert.ToInt64(property.GetValue(memberData, null));
            var plusValue = Convert.ToInt64(property.GetValue(changeData, null)) * (changeDataType == ChangeDataType.Absolute ? 1 : sourceValue);
            property.SetValue(memberData, Convert.ChangeType((sourceValue + plusValue), property.PropertyType), null);
            // 保存变更数据
            property.SetValue(changedData, Convert.ChangeType(plusValue, property.PropertyType), null);
        }
    }

    /// <summary>
    /// 加字段值
    /// </summary>
    /// <param name="field"></param>
    /// <param name="memberData"></param>
    /// <param name="skill"></param>
    public static void AdditionField(FieldInfo field, MemberData memberData, MemberData changeData, MemberData changedData, IDictionary<string, ChangeDataType> changeDataTypeDic)
    {
        var propertyName = field.Name;
        ChangeDataType changeDataType = ChangeDataType.Absolute;
        // 读取赋值类型, 如果没有则默认使用绝对值
        if (changeDataTypeDic.ContainsKey(propertyName))
        {
            changeDataType = changeDataTypeDic[propertyName];
        }
        // 修改float类型属性
        if (field.FieldType == typeof(float))
        {
            var sourceValue = Convert.ToSingle(field.GetValue(memberData));
            var plusValue = Convert.ToSingle(field.GetValue(changeData)) * (changeDataType == ChangeDataType.Absolute ? 1 : sourceValue);
            field.SetValue(memberData, sourceValue + plusValue);
            // 保存变更数据
            field.SetValue(changedData, plusValue);
        }
        // 修改bool类型属性
        else if (field.FieldType == typeof(bool))
        {
            field.SetValue(memberData, Convert.ToBoolean(field.GetValue(changeData)));
            // 保存变更数据
            field.SetValue(changedData, field.GetValue(memberData));
        }
        // 修改int,short,long类型属性
        else if (field.FieldType == typeof(long) || field.FieldType == typeof(short) || field.FieldType == typeof(int))
        {
            var sourceValue = Convert.ToInt64(field.GetValue(memberData));
            var plusValue = Convert.ToInt64(field.GetValue(changeData)) * (changeDataType == ChangeDataType.Absolute ? 1 : sourceValue);
            field.SetValue(memberData, Convert.ChangeType((sourceValue + plusValue), field.FieldType));
            // 保存变更数据
            field.SetValue(changedData, Convert.ChangeType(plusValue, field.FieldType));
        }
    }

    /// <summary>
    /// 减去增加的属性
    /// </summary>
    public static void SubAttribute(MemberData memberData, MemberData changedData)
    {
        if (memberData == null || changedData == null)
        {
            return;
        }
        
        // 获取可被变更的数据列表
        var propertyList = typeof(MemberData).GetProperties().Where((property) =>
        {
            if (property.GetCustomAttributes(typeof(SkillAddition), false).Any())
            {
                return true;
            }
            return false;
        });

        foreach (var property in propertyList)
        {
            // 修改float类型属性
            if (property.PropertyType == typeof(float))
            {
                var sourceValue = Convert.ToSingle(property.GetValue(memberData, null));
                var subValue = Convert.ToSingle(property.GetValue(changedData, null));
                property.SetValue(memberData, sourceValue - subValue, null);
            }
            // 修改bool类型属性
            else if (property.PropertyType == typeof(bool))
            {
                property.SetValue(memberData, Convert.ToBoolean(property.GetValue(changedData, null)), null);
            }
            // 修改int,short,long类型属性
            else if (property.PropertyType == typeof(long) || property.PropertyType == typeof(short) || property.PropertyType == typeof(int))
            {
                var sourceValue = Convert.ToInt64(property.GetValue(memberData, null));
                var plusValue = Convert.ToInt64(property.GetValue(changedData, null));
                property.SetValue(memberData, Convert.ChangeType((sourceValue - plusValue), property.PropertyType), null);
            }
        }

        var fieldList = typeof(MemberData).GetFields().Where((property) =>
        {
            if (property.GetCustomAttributes(typeof(SkillAddition), false).Any())
            {
                return true;
            }
            return false;
        });
        foreach (var field in fieldList)
        {
            // 修改float类型属性
            if (field.FieldType == typeof(float))
            {
                var sourceValue = Convert.ToSingle(field.GetValue(memberData));
                var subValue = Convert.ToSingle(field.GetValue(changedData));
                field.SetValue(memberData, sourceValue - subValue);
            }
            // 修改bool类型属性
            else if (field.FieldType == typeof(bool))
            {
                field.SetValue(memberData, Convert.ToBoolean(field.GetValue(changedData)));
            }
            // 修改int,short,long类型属性
            else if (field.FieldType == typeof(long) || field.FieldType == typeof(short) || field.FieldType == typeof(int))
            {
                var sourceValue = Convert.ToInt64(field.GetValue(memberData));
                var plusValue = Convert.ToInt64(field.GetValue(changedData));
                field.SetValue(memberData, Convert.ChangeType((sourceValue - plusValue), field.FieldType));
            }
        }
    }


    /// <summary>
    /// 获取替换数据后的技能说明
    /// </summary>
    /// <returns>技能说明(用数据替换掉替换符的说明)</returns>
    public string GetReplacedDescription(int level)
    {
        if (level <= 0)
        {
            return null;
        }
        if (DataList == null || DataList.Count == 0)
        {
            throw new Exception("没有数据,DataList为空");
        }
        if (level > DataList.Count)
        {
            throw new Exception("输入等级:" + level + "大于最高等级" + DataList.Count);
        }

        // 复制说明, 防止修改原有数据
        var result = string.Copy(Description);

        var dataLine = DataList[level - 1];
        for (var i = 0; i < dataLine.Count; i++)
        {
            result = result.Replace("{%" + i + "}", dataLine[i]);
        }

        return result;
    }
}


/// <summary>
/// 数据域
/// </summary>
public class DataScope
{

    /// <summary>
    /// 数据域-float
    /// </summary>
    private Dictionary<string, float> dataScopeFloat = new Dictionary<string, float>();

    /// <summary>
    /// 数据域-int
    /// </summary>
    private Dictionary<string, int> dataScopeInt = new Dictionary<string, int>();

    /// <summary>
    /// 数据域-long
    /// </summary>
    private Dictionary<string, long> dataScopeLong = new Dictionary<string, long>();

    /// <summary>
    /// 数据域-bool
    /// </summary>
    private Dictionary<string, bool> dataScopeBool = new Dictionary<string, bool>();

    /// <summary>
    /// 数据域-string
    /// </summary>
    private Dictionary<string, string> dataScopeString = new Dictionary<string, string>();



    public float? GetFloat(string key)
    {
        if (dataScopeFloat.ContainsKey(key))
        {
            return dataScopeFloat[key];
        }
        else
        {
            return null;
        }
    }


    public int? GetInt(string key)
    {
        if (dataScopeInt.ContainsKey(key))
        {
            return dataScopeInt[key];
        }
        else
        {
            return null;
        }
    }


    public long? GetLong(string key)
    {
        if (dataScopeLong.ContainsKey(key))
        {
            return dataScopeLong[key];
        }
        else
        {
            return null;
        }
    }


    public bool? GetBool(string key)
    {
        if (dataScopeBool.ContainsKey(key))
        {
            return dataScopeBool[key];
        }
        else
        {
            return null;
        }
    }


    public string GetString(string key)
    {
        return dataScopeString[key];
    }


    public void SetFloat(string key, float value)
    {
        if (dataScopeFloat.ContainsKey(key))
        {
            dataScopeFloat[key] = value;
        }
        else
        {
            dataScopeFloat.Add(key, value);
        }
    }
    public void SetInt(string key, int value)
    {
        if (dataScopeInt.ContainsKey(key))
        {
            dataScopeInt[key] = value;
        }
        else
        {
            dataScopeInt.Add(key, value);
        }
    }
    public void SetLong(string key, long value)
    {
        if (dataScopeLong.ContainsKey(key))
        {
            dataScopeLong[key] = value;
        }
        else
        {
            dataScopeLong.Add(key, value);
        }
    }
    public void SetBool(string key, bool value)
    {
        if (dataScopeBool.ContainsKey(key))
        {
            dataScopeBool[key] = value;
        }
        else
        {
            dataScopeBool.Add(key, value);
        }
    }
    public void SetString(string key, string value)
    {
        if (dataScopeString.ContainsKey(key))
        {
            dataScopeString[key] = value;
        }
        else
        {
            dataScopeString.Add(key, value);
        }
    }

    //public T Get<T>(string key)
    //{
    //    T result = default (T);
    //    var type = typeof (T);
    //    if (type == typeof(float))
    //    {
    //        result = (T)Convert.ChangeType(dataScopeFloat[key], type);
    //    }
    //    else if (type == typeof(int))
    //    {
    //        result = (T)Convert.ChangeType(dataScopeInt[key], type);
    //    }
    //    else if (type == typeof(long))
    //    {
    //        result = (T)Convert.ChangeType(dataScopeLong[key], type);
    //    }
    //    else if (type == typeof(bool))
    //    {
    //        result = (T)Convert.ChangeType(dataScopeBool[key], type);
    //    }
    //    else if (type == typeof(string))
    //    {
    //        result = (T)Convert.ChangeType(dataScopeString[key], type);
    //    }
    //    return result;
    //}


    //public void Set<T>(string key, T value)
    //{
    //    if (typeof (T) == typeof (float))
    //    {
    //        dataScopeFloat.Add(key, (float)value);
    //    }
    //    else if (typeof(T) == typeof(int))
    //    {

    //    }
    //    else if (typeof(T) == typeof(long))
    //    {

    //    }
    //    else if (typeof(T) == typeof(bool))
    //    {

    //    }
    //    else if (typeof(T) == typeof(string))
    //    {

    //    }
    //}
}

/// <summary>
/// 伤害附加/减免目标/来源类型
/// </summary>
public enum DemageAdditionOrReductionTargetType
{
    All = -1,               // 所有类型
    // -----空地建筑-----
    Air = 0,                // 空中
    Surface = 1,            // 地面
    Building = 2,           // 建筑
    // -----隐形-----
    Hide = 3,               // 隐形
    // -----种族-----
    RaceHuman = 4,          // 人族
    RaceOrc = 5,            // 兽族
    RaceMechanics = 6,      // 机械
    // -----属性分类-----
    Mechanics = 7,          // 机械单位
    Melee = 8,              // 近战单位
    Summoned = 9,           // 召唤单位

    NotMelee = 10,          // 非近战单位
    NotMechanics = 11,      // 非械单位

}