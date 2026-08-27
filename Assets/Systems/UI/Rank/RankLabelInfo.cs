using UnityEngine;
using System;

[Serializable]
public struct RankLabelInfo
{
    [SerializeField] private string rank;
    [SerializeField] private string score;

    public RankLabelInfo(string rank, string score)
    {
        this.rank = rank;
        this.score = score;
    }

    public string Rank => rank;
    public string Score => score;
}
