using UnityEngine;

[CreateAssetMenu(fileName = "AdsConfiguration", menuName = "Scriptable Objects/AdsConfiguration")]
public class AdsConfiguration : ScriptableObject
{
    private string appKey;
    private string rewardedKey;
    private string interstitialKey;
#if UNITY_ANDROID || UNITY_IOS
    [SerializeField] private string appKey;
    [SerializeField] private string rewardedKey;
    [SerializeField] private string interstitialKey;
#endif

    public string AppKey => appKey;
    public string RewardedKey => rewardedKey;
    public string InterstitialKey => interstitialKey;
}
