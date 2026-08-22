using Systems.Manager;
using UnityEngine;

public class RankController : MonoBehaviour
{
    [SerializeField] private Transform rankContent;
    [SerializeField] private RankLabel labelPrefab;
    [SerializeField] private RankLabel[] podium;

    [Header("Testing")]
    // This is a test variable, used to display fixed info in the rank to show its functionality
    [SerializeField] RankLabelInfo[] positions;

    private void Start()
    {
        SetContent(positions);
    }

    public void SetContent(RankLabelInfo[] positions)
    {
        Clear();

        for (int i = 0; i < positions.Length; i++)
        {
            RankLabel newLabel = GetNewLabel();

            newLabel.SetInfo(positions[i]);
            newLabel.transform.SetParent(rankContent);
        }
    }

    private void Clear()
    {
        for (int i = rankContent.childCount - 1; i > 0; i--)
        {
            Destroy(rankContent.GetChild(i).gameObject);
        }
    }

    private RankLabel GetNewLabel()
    {
        return Instantiate(labelPrefab);
    }

    public async void LoadRank()
    {
        var scores = await PersistentData.Instance.GetTopScoresAsync();

        if (scores == null || scores.Count == 0)
        {
            return;
        }

        RankLabelInfo[] labels = new RankLabelInfo[scores.Count];

        for (int i = 0; i < podium.Length; i++)
        {
            if (i < scores.Count)
            {
                var score = scores[i];
                var label = new RankLabelInfo((score.Rank + 1).ToString("0"), score.Score.ToString());
                podium[i].SetInfo(label);
            }
            else break;
        }

        for (int i = podium.Length; i < scores.Count; i++)
        {
            var score = scores[i];
            labels[i] = new RankLabelInfo((score.Rank + 1).ToString("0."), score.Score.ToString());
        }

        SetContent(labels);
    }
}
