using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GemiSource
{
    public string gemiName;
    public int gemiBuyukluk;
    public int gemiGold;
    public int gemiOdun;
    public int gemiKaynak;
    public bool isGemiKullanılan;
    public KaynakType gemiKaynakType;
    public List<int> gemiDepo = new List<int>() { 0, 0, 0};
}
public enum KaynakType
{
    Petrol,
    Tuğla
}
[System.Serializable]
public class PlayerData
{
    public List<GemiSource> kullanılanGemiler = new List<GemiSource>();
    public List<GemiSource> kullanılmayanGemiler = new List<GemiSource>();
    public List<int> kullanılanKargolar = new List<int>();
}
public static class SaveManager
{
    [Header("Script Atamaları")]
    // Game Manager scriptinde SavePlayer fonksiyonunda bunu çağıracaksın
    private static string path = "/HEC_XX.HEC";
    public static void SavePlayer(PlayerData playerData)
    {
        BinaryFormatter formater = new BinaryFormatter();
        string yol = Application.persistentDataPath + path;

        FileStream stream = new FileStream(yol, FileMode.Create);

        //playerData.SetBoolToByte();
        formater.Serialize(stream, playerData);
        stream.Close();
    }
    // Game Manager scriptinde LoadPlayer fonksiyonunda bunu çağıracaksın
    public static PlayerData LoadPlayer()
    {
        string yol = Application.persistentDataPath + path;

        PlayerData bilgi = null;
        if (File.Exists(yol))
        {
            BinaryFormatter formater = new BinaryFormatter();
            FileStream stream = new FileStream(yol, FileMode.Open);

            bilgi = (PlayerData)formater.Deserialize(stream);
            stream.Close();

            //bilgi.SetByteToBool();
        }
        else
        {
            Debug.LogWarning("You lost your data.");
            bilgi = new PlayerData();
        }
        return bilgi;
    }
}