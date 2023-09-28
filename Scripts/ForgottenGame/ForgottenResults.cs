using System;
using UnityEngine;

[Serializable]
public class ForgottenResults
{
    [SerializeField]
    private float timeToFinish;

    [SerializeField]
    private int attempts;

    [SerializeField]
    private string date;

    public string GetDate()
    {
        return date;
    }

    public void SetDate(string value)
    {
        date = value;
    }

    public float GetTimeToFinish()
    {
        return timeToFinish;
    }

    public void SetTimeToFinish(float value)
    {
        timeToFinish = value;
    }

    public int GetAttempts()
    {
        return attempts;
    }

    public void SetAttempts(int value)
    {
        attempts = value;
    }
}