using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    string playerName = "Unnamed Driver";

    public void SetPlayerName(string name)
    {
        playerName = name;
        Debug.Log("name changed to " + playerName);
    }
}
