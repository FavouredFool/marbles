using System.IO;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] GameObject visuals;
    [SerializeField] EntryLine[] entryLines;
    [SerializeField] string fileName = "Scores";

    ScoreList scoreList;

    string playerName = "Unnamed Driver";
    string dataPath;

    public void Awake()
    {
        scoreList = new();
    }

    public void Start()
    {
        dataPath = Application.persistentDataPath + "/" + fileName + ".json";

        // Deserialize instantly
        if (File.Exists(dataPath))
        {
            string fileContents = File.ReadAllText(dataPath);
            scoreList = JsonUtility.FromJson<ScoreList>(fileContents);
        }

        scoreList.Scores.Sort();
        UpdateLines();
    }

    public void UpdateLines()
    {
        for (int i = 0; i < 3; i++)
        {
            EntryLine line = entryLines[i];

            string searchedName;
            float searchedTime;
            if (scoreList.Scores.Count > i)
            {
                searchedName = scoreList.Scores[i].Name;
                searchedTime = scoreList.Scores[i].Time;
            }
            else
            {
                searchedName = "No Entry";
                searchedTime = float.PositiveInfinity;
            }

            line.nameField.text = searchedName;

            if (searchedTime == float.PositiveInfinity || searchedTime == float.NegativeInfinity)
            {
                line.timeField.text = "XX.XXX" + "s";
            }
            else
            {
                line.timeField.text = searchedTime.ToString("00.000") + "s";
            }
            
        }
    }

    public void AddScore(float time)
    {
        scoreList.Scores.Add(new Score(playerName, time));
        scoreList.Scores.Sort();

        // Serialize instantly
        string jsonString = JsonUtility.ToJson(scoreList);
        File.WriteAllText(dataPath, jsonString);
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public void ToggleVisuals(bool toggle)
    {
        visuals.SetActive(toggle);
    }

    [System.Serializable]
    public class EntryLine
    {
        public TMP_Text nameField;
        public TMP_Text timeField;
    }
}
