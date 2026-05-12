using UnityEngine;

[CreateAssetMenu(fileName = "AdsConfiguration", menuName = "Scriptable Objects/AdsConfiguration")]
public class AdsConfiguration : ScriptableObject
{
#if UNITY_ANDROID
    [SerializeField] private string appKey;
    [SerializeField] private string rewardedKey;
    [SerializeField] private string interstitialKey;
#elif UNITY_IOS
    [SerializeField] private string appKey;
    [SerializeField] private string rewardedKey;
    [SerializeField] private string interstitialKey;
#endif

    public string AppKey => appKey;
    public string RewardedKey => rewardedKey;
    public string InterstitialKey => interstitialKey;
}
