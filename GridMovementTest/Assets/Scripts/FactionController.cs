using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class FactionController : MonoBehaviour
{

    public void SaveFactionChoice(int faction)
    {
        string path = Path.Combine(Application.persistentDataPath, "faction.player");
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(path)))
        {
            writer.Write((byte)faction);
        }
    }

    public void GetChosenFaction()
    {
        string path = Path.Combine(Application.persistentDataPath, "faction.player");
        if (!File.Exists(path))
        {
            //Debug.LogError("File does not exist " + path);
            return;
        }
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            int faction = reader.ReadByte();
            IsometricMetrics.chosenFaction = (Factions)faction;
        }
    }

}
