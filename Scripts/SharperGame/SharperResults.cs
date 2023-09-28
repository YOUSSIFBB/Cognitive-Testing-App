using System;
using UnityEngine;

[Serializable]
public class SharperResults
{
    [SerializeField]
    private string date, score;

    public string GetScore()
    {
        return score;
    }

    public void SetScore(string value)
    {
        score = value;
    }

    public string GetDate()
    {
        return date;
    }

    public void SetDate(string value)
    {
        date = value;
    }
}