using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class AssetsRenameUtil
{
    [MenuItem("Assets/ReName")]

    public static void ToRename()
    {
        // 获取选择的所有对象（无序）
        Object[] m_objects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        // 获取所有对象名
        string[] fullSelectionPaths = new string[m_objects.Length];
        for (int i = 0; i < m_objects.Length; i++)
        {
            fullSelectionPaths[i] = AssetDatabase.GetAssetPath(m_objects[i]);
        }
        // 以对象名为键，对值排序
        System.Array.Sort(fullSelectionPaths, m_objects);

        int index = 16; // 序号

        foreach (Object item in m_objects)
        {

            //string m_name = item.name;
            if (Path.GetExtension(AssetDatabase.GetAssetPath(item)) != "") // 判断路径是否为空
            {

                string path = AssetDatabase.GetAssetPath(item);

                AssetDatabase.RenameAsset(path, index +  "_Stand");
                index--;
            }

        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}

