using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// 打包地图数据
/// </summary>
public class EditMapData : EditorWindow
{

    private static EditMapData window = null;

    [MenuItem("Tools/MapData Generate", false, 1200)]
    static void QKTools_BuildConfigs()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 500, 300);
        window = (EditMapData)EditorWindow.GetWindowWithRect(typeof(EditMapData), wr, true, "Build MapData");
        window.Show();
    }
    // 绑定Menu



    void OnGUI()
    {
        if (GUILayout.Button("导出地图数据", GUILayout.Width(140)))
        {
            BuildMapData();
            //关闭窗口
            this.Close();

            // 测试加载文件
            //var dicData = Utils.DepartFileData(Utils.LoadFileInfo(Application.streamingAssetsPath + @"\MapData\" + "mapdata"));

        }
    }

    /// <summary>
    /// 创建地图
    /// </summary>
    private void BuildMapData()
    {
        string mapDataPath = Path.Combine(Application.streamingAssetsPath, @"config\mapDatas");
        // 打包地图文件
        var dir = new DirectoryInfo(mapDataPath);
        if (dir.Exists)
        {
            var mapDataFiles = dir.GetFiles();
            if (mapDataFiles.Any())
            {
                List<string> pathList = new List<string>();
                foreach (var mapDataFileInfo in mapDataFiles)
                {
                    var path = mapDataFileInfo.FullName;
                    if (path.ToLower().EndsWith(".meta"))
                    {
                        continue;
                    }
                    //var dataPath = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
                    //var prePath = path.Substring(dataPath.Length, path.Length - dataPath.Length);
                    // 合并文件并输出到目录
                    pathList.Add(path);
                }

                var combineData = Utils.CombineFile(pathList);

                // 生成到Resources目录
                Utils.CreateOrOpenFile(Application.dataPath + @"\StreamingAssets\MapData\", "mapdata", combineData);
                Debug.Log("文件输出完毕:" + combineData);
            }
        }
    }
}