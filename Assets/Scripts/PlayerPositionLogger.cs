using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerPositionLogger : MonoBehaviour
{
    private GameObject player;
    private string logFilePath;
    private List<string> logEntries = new List<string>();
    private float writeToDiskInterval = 10f; // Log every 10 seconds
    private float logInterval = 0.333f; // Log every 1/3 of a second
    private float logIntervalIndex = 0f;
    private float writeTimer;
    private float logTimer;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
        logFilePath = Path.Combine(Application.persistentDataPath, $"{timestamp}.csv");
        if (!File.Exists(logFilePath))
        {
            File.Create(logFilePath).Dispose(); // Ensure the file is created and closed
        }
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        FindPlayerHead();
        AddLogEntry(player.transform.position, "Experience started");
    }

    void Update()
    {
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
        FindPlayerHead();
    }

    void FindPlayerHead()
    {
        player = GameObject.FindWithTag("MainCamera");
    }

    void AddLogEntry(Vector3 position, string comment = "")
    {
        logIntervalIndex += logTimer;

        string logEntry = $"{logIntervalIndex},{position.x},{position.y},{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name},{comment}";
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
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        AddLogEntry(Vector3.zero, "Experience ended");
        WriteLogEntries(); // Ensure remaining entries are written on destroy
    }
}

