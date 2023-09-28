using System;
using UnityEngine;

[Serializable]
public class BeatsResultsLvl2
{
    [SerializeField]
    private string missedSoundLvl2;

    [SerializeField]
    private float pithLvl2, volumeLvl2;

    public string GetMissedSoundLvl2()
    {
        return missedSoundLvl2;
    }

    public void SetMissedSoundLvl2(string value)
    {
        missedSoundLvl2 = value;
    }

    public float GetPitchLvl2()
    {
        return pithLvl2;
    }

    public void SetPitchLvl2(float value)
    {
        pithLvl2 = value;
    }

    public float GetVolumeLvl2()
    {
        return volumeLvl2;
    }

    public void SetVolumeLvl2(float value)
    {
        volumeLvl2 = value;
    }
}