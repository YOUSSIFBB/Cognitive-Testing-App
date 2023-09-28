using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BeatsResults
{
    [SerializeField]
    private List<BeatsResultsLvl2> beatsResultsLvl2 = new List<BeatsResultsLvl2>();

    [SerializeField]
    private List<BeatsResultsLvl3> beatsResultsLvl3 = new List<BeatsResultsLvl3>();

    [SerializeField]
    private string date;

    public List<BeatsResultsLvl2> GetBeatsResultsLvl2()
    {
        return beatsResultsLvl2;
    }

    public void SetBeatsResultsLvl2(BeatsResultsLvl2 value)
    {
        beatsResultsLvl2.Add(value);
    }

    public List<BeatsResultsLvl3> GetBeatsResultsLvl3()
    {
        return beatsResultsLvl3;
    }

    public void SetBeatsResultsLvl3(BeatsResultsLvl3 value)
    {
        beatsResultsLvl3.Add(value);
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