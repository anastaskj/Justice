using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;


public class IsometricGameManager : MonoBehaviour
{
    public IsometricUnitManager unitManager;
    public MusicManager musicManager;
    public SaveLoadMenu loadMenu;
    public ChampionManager champManager;
    public FactionController factionController;

    public void StartGame()
    {
        loadMenu.LoadRandomMap();
        loadMenu.gameObject.SetActive(false);

        factionController.GetChosenFaction();
        champManager.InstantiateChampions(IsometricMetrics.progress, IsometricMetrics.chosenFaction);

        unitManager.BeginGame();
        musicManager.PlayMusic();
        IsometricMetrics.state = BattleState.SETUP;
        IsometricMetrics.editorMode = false;
    }

    private void Awake()
    {
        GetGameState();
        unitManager.UpdateProgress();
        StartGame();
    }

    void GetGameState()
    {
        string path = Path.Combine(Application.persistentDataPath, "progress.player");
        if (!File.Exists(path))
        {
            //Debug.LogError("File does not exist " + path);
            return;
        }
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            int state = reader.ReadByte();
            IsometricMetrics.progress = (ProgressState)state;
        }
    }

    public void SaveGameState()
    {
        string path = Path.Combine(Application.persistentDataPath, "progress.player");
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(path)))
        {
            writer.Write((byte)IsometricMetrics.progress);
        }
    }

    
}
