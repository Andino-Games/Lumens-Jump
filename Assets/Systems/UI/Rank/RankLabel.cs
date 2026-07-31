using TMPro;
using UnityEngine;

public class RankLabel : MonoBehaviour
{
    [SerializeField] new private TextMeshPro name;
    [SerializeField] private TextMeshPro score;

    public void SetInfo(RankLabelInfo info)
    {
        name.text = info.Name;
        score.text = info.Score;
    }
}
