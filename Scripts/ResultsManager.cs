using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResultsManager
{
    [SerializeField]
    private List<VoiceResults> voiceResults = new List<VoiceResults>();

    [SerializeField]
    private List<ForgottenResults> forgottenResults = new List<ForgottenResults>();

    [SerializeField]
    private List<BeatsResults> beatsResults = new List<BeatsResults>();

    [SerializeField]
    private List<SharperResults> sharperResults = new List<SharperResults>();

    public List<VoiceResults> GetVoiceResults()
    {
        return voiceResults;
    }

    public void SetVoiceResults(VoiceResults value)
    {
        voiceResults.Add(value);
    }

    public List<ForgottenResults> GetForgottenResults()
    {
        return forgottenResults;
    }

    public void SetForgottenResults(ForgottenResults value)
    {
        forgottenResults.Add(value);
    }

    public List<BeatsResults> GetBeatsResults()
    {
        return beatsResults;
    }

    public void SetBeatsResults(BeatsResults value)
    {
        beatsResults.Add(value);
    }

    public List<SharperResults> GetSharperResults()
    {
        return sharperResults;
    }

    public void SetSharperResults(SharperResults value)
    {
        sharperResults.Add(value);
    }
}