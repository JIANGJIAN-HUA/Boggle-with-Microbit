using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;

public struct Vocabulary : IComparable<Vocabulary>
{
    public string English;
    public string Chinese;

    public int CompareTo(Vocabulary Voc)
    {
        // 區分大小寫比較
        //return English.CompareTo(Voc.English);

        // 不區分大小寫比較
        return string.Compare(English, Voc.English, StringComparison.OrdinalIgnoreCase);
    }
}

public class GetVoc : MonoBehaviour
{
    static bool isFirstRead = true;
    
    public static Vocabulary[] Voc = new Vocabulary[Constants.VocNum];

    private string conn, sqlQuery;
    IDbConnection dbconn; // 建立與資料庫的連線
    IDbCommand dbcmd;
    private IDataReader reader;

    string DatabaseName = "English_1200_Voc.s3db";

    void Start()
    {
        if(isFirstRead)
        {
            isFirstRead = false;
#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX
            //string filepath = Application.dataPath + "/SQLitePlugins/" + DatabaseName;
            string filepath = Application.streamingAssetsPath + "/Voc/" + DatabaseName;
#elif UNITY_ANDROID
        // Android 存放資料庫的位置
        string filepath = Application.persistentDataPath + "/" + DatabaseName;

        // 如果資料庫不存在
        if (!File.Exists(filepath))
        {
            // If not found on android will create Tables and database

            /*
            Debug.LogWarning("File \"" + filepath + "\" does not exist. Attempting to create from \"" +
                             Application.dataPath + "!/assets/Employers");
            */
        
            // 從 streamingAssets 資料夾內取得資料庫
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/Voc/" + DatabaseName);
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDB.bytes);
        }  
#elif UNITY_IOS
        // IOS 存放資料庫的位置
        string filepath = Application.persistentDataPath + "/" + DatabaseName;

        // 如果資料庫不存在
        if (!File.Exists(filepath))
        {
            // 從 streamingAssets 資料夾內取得資料庫
            var loadDb = Application.dataPath + "/Raw/" + DatabaseName;
            // then save to Application.persistentDataPath
            File.Copy(loadDb, filepath);
        }
#endif
            //open db connection
            conn = "URI=file:" + filepath;

            //Debug.Log("Stablishing connection to: " + conn);
            dbconn = new SqliteConnection(conn);
            dbconn.Open();
            /*
#if UNITY_ANDROID
            // 執行 SQL 指令
            string query; // 存放 SQL 語法
            query = "CREATE TABLE Voc (English varchar(50), Chinese nvarchar(250))";
            try
            {
                dbcmd = dbconn.CreateCommand(); // create empty command
                dbcmd.CommandText = query; // fill the command
                reader = dbcmd.ExecuteReader(); // execute command which returns a reader
            }
            catch (Exception e)
            {

                Debug.Log(e);

            }
#endif
            */
            reader_function();
        }
    }

    void reader_function()
    {
        int i = 0;
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT  English, Chinese " + "FROM Voc";// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Voc[i].English = reader.GetString(0);
                Voc[i++].Chinese = reader.GetString(1);

                // for Debug
                /*
                Debug.Log("num : " + i + " English : " + Voc[i].English + " Chinese : " + Voc[i].Chinese);
                i++;
                */
            }

            sortVoc();

            reader.Close();
            reader = null;
            dbcmd.Dispose(); // 釋放資料
            dbcmd = null;
            dbconn.Close();
        }
    }

    void sortVoc()
    {
        Array.Sort(Voc);

        // For Debug
        /*
        int i = 0;
        foreach(var Voca in Voc)
        {
            i++;
            Debug.Log("num : " + i + " English : " + Voca.English + " Chinese : " + Voca.Chinese);
        }
        */
    }

}
