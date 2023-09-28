using System;
using UnityEngine;

[Serializable]
public class BeatsResultsLvl3
{
    [SerializeField]
    private string missedSoundLvl3, side;

    [SerializeField]
    private float pithLvl3, volumeLvl3;

    public string GetSide()
    {
        return side;
    }

    public void SetSide(string value)
    {
        side = value;
    }

    public string GetMissedSoundLvl3()
    {
        return missedSoundLvl3;
    }

    public void SetMissedSoundLvl3(string value)
    {
        missedSoundLvl3 = value;
    }

    public float GetPitchLvl3()
    {
        return pithLvl3;
    }

    public void SetPitchLvl3(float value)
    {
        pithLvl3 = value;
    }

    public float GetVolumeLvl3()
    {
        return volumeLvl3;
    }

    public void SetVolumeLvl3(float value)
    {
        volumeLvl3 = value;
    }
}