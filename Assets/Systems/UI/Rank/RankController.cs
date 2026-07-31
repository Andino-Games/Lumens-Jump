using UnityEngine;

public class RankController : MonoBehaviour
{
    [SerializeField] private Transform leftContent;
    [SerializeField] private Transform rightContent;
    [SerializeField] private RankLabel labelPrefab;

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
            bool isEvenNumber = i % 2 == 0;
            RankLabel newLabel = GetNewLabel();

            newLabel.SetInfo(positions[i]);
            newLabel.transform.SetParent(isEvenNumber? leftContent : rightContent);
        }
    }

    private void Clear()
    {
        for (int i = leftContent.childCount - 1; i >= 0; i--)
        {
            Destroy(leftContent.GetChild(i).gameObject);
        }

        for (int i = rightContent.childCount - 1; i >= 0; i--)
        {
            Destroy(rightContent.GetChild(i).gameObject);
        }
    }

    private RankLabel GetNewLabel()
    {
        return Instantiate(labelPrefab);
    }
}
