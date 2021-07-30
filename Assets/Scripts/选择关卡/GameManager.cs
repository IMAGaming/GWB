using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    // 关卡解锁情况
    public bool[] locks = new bool[4];

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        locks[0] = true;

        DontDestroyOnLoad(this); //使得切换场景后这个manager不会消失
    }

    //按下保存按钮将会调用此方法
    public void SaveButton()
    {
        SaveByJSON();
    }

    //按下加载按钮(继续游戏)将会调用此方法
    public void LoadButton()
    {
        LoadByJSON();
    }


    /// <summary>
    /// 实例化一个Save类型的对象，然后把要存的游戏数据
    /// 存储到这个对象中的数据成员里面，最后返回这对象
    /// 之后只需要将这个返回的对象转换成JSON格式的文件
    /// 即可完成保存数据的序列化过程
    /// </summary>
    /// <returns></returns>
    private Save CreateSaveGameObject()
    {
        Save save = new Save();

        //SaveData，把游戏中要保存的数据传到对象中
        save.unLock0 = locks[0];
        save.unLock1 = locks[1];
        save.unLock2 = locks[2];
        save.unLock3 = locks[3];

        return save;
    }

    /// <summary>
    /// 将Save对象转换为JSON数据格式文件
    /// </summary>
    void SaveByJSON()
    {
        Save save = CreateSaveGameObject();

        string JsonString = JsonUtility.ToJson(save);
        StreamWriter sw = new StreamWriter(Application.dataPath + "/JSONData.text");
        sw.Write(JsonString);
        sw.Close();
        Debug.Log("Check");
    }

    void LoadByJSON()
    {
        if (File.Exists(Application.dataPath + "/JSONData.text"))
        {
            //Load the Game
            StreamReader sr = new StreamReader(Application.dataPath + "/JSONData.text");
            string JsonString = sr.ReadToEnd();
            sr.Close();

            //把JSON转换为对象，开始反序列化过程
            Save save = JsonUtility.FromJson<Save>(JsonString);

            Debug.Log("Loading");

            //Load the Data to the Game
            locks[0] = save.unLock0;
            locks[1] = save.unLock1;
            locks[2] = save.unLock2;
            locks[3] = save.unLock3;

        }
        else
        {
            Debug.Log("Not Found File");
        }
    }

}
