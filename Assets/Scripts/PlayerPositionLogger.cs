using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class PlayerPositionLogger : MonoBehaviour
{
    private GameObject player;
    private string logFilePath;
    private List<string> logEntries = new List<string>();
    private float writeToDiskInterval = 10f; // Log every 10 seconds
    private float logInterval = 0.2f;
    private float logIntervalIndex = 0f;
    private float writeTimer;
    private float logTimer;
    private string currentScene = string.Empty;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmssfff");

        #if UNITY_EDITOR
                logFilePath = Path.Combine(Application.persistentDataPath, $"{timestamp}.csv");
        #else
                logFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, $"{timestamp}.csv");
        #endif

        Debug.Log("LogFilePath: " + logFilePath);

        if (!File.Exists(logFilePath))
        {
            File.Create(logFilePath).Dispose(); // Ensure the file is created and closed
        }
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        FindPlayerHead();
        AddLogEntry(player.transform.position, "Experience started, will log after Tutorial");
    }   

    void Update()
    {
        if (currentScene.Equals(string.Empty) || currentScene.Equals("OpeningScene")) return;

        writeTimer += Time.deltaTime;
        logTimer += Time.deltaTime;

        if (player != null)
        {
            Vector3 currentPosition = player.transform.position;
            if (logTimer > logInterval)
            {
                AddLogEntry(currentPosition);
                logTimer = 0f;
            }
        }

        if (writeTimer >= writeToDiskInterval)
        {
            WriteLogEntries();
            writeTimer = 0f;
        }
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        currentScene = scene.name;
        FindPlayerHead();
    }

    void FindPlayerHead()
    {
        player = GameObject.FindWithTag("MainCamera");
    }

    void AddLogEntry(Vector3 position, string comment = "")
    {
        logIntervalIndex += logTimer;
        string logEntry = $"{logIntervalIndex},{position.x},{position.z},{currentScene},{comment}";
        logEntries.Add(logEntry);
    }

    void WriteLogEntries()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                foreach (string logEntry in logEntries)
                {
                    writer.WriteLine(logEntry);
                }
            }
            logEntries.Clear();
        }
        catch (IOException ex)
        {
            Debug.LogError("Failed to write to log file: " + ex.Message);
        }
    }

    void OnDestroy()
    {
        ExitLogging(false);
    }

    public void ExitLogging(Boolean userShutdown)
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        AddLogEntry(Vector3.zero, userShutdown ? "Application Closed Via Shortcut" : "Unity detected shutdown");
        WriteLogEntries(); // Ensure remaining entries are written on destroy
    }
}

