using TMPro;
using UnityEngine;

public class RankLabel : MonoBehaviour
{
    [SerializeField] private TextMeshPro rank;
    [SerializeField] private TextMeshPro score;

    public void SetInfo(RankLabelInfo info)
    {
        rank.text = info.Rank;
        score.text = info.Score;
    }
}
