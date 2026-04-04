using UnityEngine;
using System;

[Serializable]
public struct RankLabelInfo
{
    [SerializeField] private string name;
    [SerializeField] private string score;

    public string Name => name;
    public string Score => score;
}
