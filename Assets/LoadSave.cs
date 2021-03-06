﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LoadSave : MonoBehaviour
{
    public GameObject player;
    private Brain savedGame;

    //it's static so we can call it from anywhere
    private void Awake()
    {
        savedGame = player.GetComponent<Brain>();
    }
    public void Save()
    {
        savedGame = player.GetComponent<Brain>();
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want
        bf.Serialize(file, savedGame);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            Brain savedGame = (Brain)bf.Deserialize(file);
            player.GetComponent<Brain>().SendMessage("LoadNew", savedGame);
            file.Close();
        }
    }
}