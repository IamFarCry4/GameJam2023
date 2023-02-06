using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystemHandler : MonoBehaviour
{
    public static SaveSystemHandler instance;

    [Tooltip("Game Data")]
    private const string gameDataPath="/GameData";
    private const string gameDataFile="/GameFile";

    void Awake()
    {
        if(instance!=null)
        {
            return;
        }
        else
        {
            instance=this;
        }
    }

    void Start()
    {
        InitializeGameData();
    }

    #region  game data
    void InitializeGameData()
    {
        string path=Application.persistentDataPath+gameDataPath;
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            UpdateData();
        }
        else
        {
            LoadGameData();
        }
    }

    public void UpdateData()
    {
        string dataPath=Application.persistentDataPath+gameDataPath+gameDataFile;
        BinaryFormatter formatter=new BinaryFormatter();
        FileStream stream=new FileStream(dataPath,FileMode.Create);
        GameData data=new GameData();
        data.restart=GameController.instance.restartLevel;
        data.totalCoins=GameController.instance.collectedCoins;
        data.highScore=GameController.instance.infiHighScore;
        data.modeIndex=GameController.instance.modeIndex;
        data.musicId=GameController.instance.musicIndex;
        data.sfxId=GameController.instance.sfxIndex;
        data.vibrationId=GameController.instance.vibrationIndex;
       
        formatter.Serialize(stream,data);
        stream.Close();
        LoadGameData();
    }

    public void LoadGameData()
    {
        string dataPath=Application.persistentDataPath+gameDataPath+gameDataFile;
        if(File.Exists(dataPath))
        {
            BinaryFormatter formatter=new BinaryFormatter();
            FileStream stream=new FileStream(dataPath,FileMode.Open);
            GameData data=formatter.Deserialize(stream)as GameData;
            GameController.instance.restartLevel=data.restart;
            GameController.instance.collectedCoins=data.totalCoins;
            GameController.instance.infiHighScore=data.highScore;
            GameController.instance.modeIndex=data.modeIndex;
            GameController.instance.musicIndex=data.musicId;
            GameController.instance.sfxIndex=data.sfxId;
            GameController.instance.vibrationIndex=data.vibrationId;
            GameController.instance.UpdateGameData();
            stream.Close();
        }
        else
        {
            Debug.Log("file not found!");
        }
    }
    #endregion
}
