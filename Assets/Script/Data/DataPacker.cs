using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Debug = UnityEngine.Debug;


/// <summary>
/// 数据类
/// </summary>
public class DataPacker : SingleItem<DataPacker>
{
    /// <summary>
    /// 数据字典
    /// </summary>
    private Dictionary<string, DataItem> dataDic = new Dictionary<string, DataItem>();


    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="dataItem"></param>
    public void SetDataItem(string name, DataItem dataItem)
    {
        
    }
}


/// <summary>
/// 数据类
/// </summary>
public class DataItem
{
    /// <summary>
    /// 数据名称
    /// </summary>
    public string DataName { get; private set; }

    /// <summary>
    /// 数据行字典
    /// </summary>
    private Dictionary<int, DataRow> dataRowDic = new Dictionary<int, DataRow>();


    public DataRow GetRowById(int id)
    {
        if (!dataRowDic.ContainsKey(id))
        {
            throw new Exception("不存在数据ID:" + id);
        }
        return dataRowDic[id];
    }

    /// <summary>
    /// 添加数据行
    /// </summary>
    /// <param name="id">数据ID</param>
    /// <param name="row">数据内容</param>
    public void AddDataRow(int id, DataRow row)
    {
        // 验证数据
        if (row == null)
        {
            Debug.LogError("数据内容为空.");
            return;
        }

        // 是否已包含数据
        if (dataRowDic.ContainsKey(id))
        {
            Debug.LogError("数据ID已存在.");
            return;
        }
        // 添加数据
        dataRowDic.Add(id, row);
    }
}


/// <summary>
/// 数据行
/// </summary>
public class DataRow
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



    public float GetFloat(string key)
    {
        if (dataScopeFloat.ContainsKey(key))
        {
            return dataScopeFloat[key];
        }
        return 0;
    }


    public int GetInt(string key)
    {
        if (dataScopeInt.ContainsKey(key))
        {
            return dataScopeInt[key];
        }
        return 0;
    }


    public long GetLong(string key)
    {
        if (dataScopeLong.ContainsKey(key))
        {
            return dataScopeLong[key];
        }
        return 0;
    }


    public bool GetBool(string key)
    {
        if (dataScopeBool.ContainsKey(key))
        {
            return dataScopeBool[key];
        }
        return false;
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
}