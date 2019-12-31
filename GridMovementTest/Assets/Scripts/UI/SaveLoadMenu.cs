using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class SaveLoadMenu : MonoBehaviour
{
    public IsometricGrid grid;
    public Text menuLabel, actionButtonLabel;
    public InputField nameInput;
    public RectTransform listContent;
    public SaveLoadItem itemPrefab;

    public IsometricFeatureManager features;
    public IsometricUnitManager manager;

    bool saveMode;

    private void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
        //Debug.Log(IsometricMetrics.editorMode);
    }

    public void Open(bool saveMode)
    {
        this.saveMode = saveMode;
        if (saveMode)
        {
            menuLabel.text = "Save Map";
            actionButtonLabel.text = "Save";
        }
        else
        {
            menuLabel.text = "Load Map";
            actionButtonLabel.text = "Load";
        }
        FillList();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    string GetSelectedPath()
    {
        string mapName = nameInput.text;
        if (mapName.Length == 0)
        {
            return null;
        }
        return Path.Combine(Application.persistentDataPath, mapName + ".map");
    }

    public void Action()
    {
        string path = GetSelectedPath();
        if (path == null)
        {
            return;
        }
        if (saveMode)
        {
            Save(path);
        }
        else
        {
            Load(path);
        }
        Close();
    }

    void Save(string path)
    {
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            writer.Write(0);
            grid.Save(writer);
            nameInput.text = "";
        }
    }

    void Load(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("File does not exist " + path);
            return;
        }
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            int header = reader.ReadInt32();
            if (header == 0)
            {
                grid.Load(reader, features, manager);
            }
            else
            {
                Debug.LogWarning("Unknown map format " + header);
            }
        }
        nameInput.text = "";

        //if (IsometricMetrics.editorMode)
        //{
        //    SceneManager.LoadScene(2);
        //    grid.OpenPanels();
        //}
        //else
        //{
        //    Debug.Log("Loaded battle");
        //    SceneManager.LoadScene(1);
        //    grid.OpenPanels();
        //}
    }

    public void SelectItem(string name)
    {
        nameInput.text = name;
    }

    void FillList()
    {
        for (int i = 0; i < listContent.childCount; i++)
        {
            Destroy(listContent.GetChild(i).gameObject);
        }

        string[] paths =  Directory.GetFiles(Application.persistentDataPath, "*.map");
        Array.Sort(paths);

        for (int i = 0; i < paths.Length; i++)
        {
            SaveLoadItem item = Instantiate(itemPrefab);
            item.menu = this;
            item.MapName = Path.GetFileNameWithoutExtension(paths[i]);
            item.transform.SetParent(listContent, false);
        }
    }

    public void Delete()
    {
        string path = GetSelectedPath();
        if (path == null)
        {
            return;
        }
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        nameInput.text = "";
        FillList();
    }
}
