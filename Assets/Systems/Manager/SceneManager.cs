using System.Collections.Generic;
using Systems.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Systems.Manager
{
    public class SceneManager : Singleton<SceneManager>
    {
        public UnityEvent onScenesLoading;
        public UnityEvent onScenesLoaded;

        [SerializeField] private List<string> scenesToPreload = new List<string>();

        private readonly List<AsyncOperationHandle<SceneInstance>> _loadedSceneHandles =
            new List<AsyncOperationHandle<SceneInstance>>();

        private AsyncOperationHandle<SceneInstance>? _currentSceneHandle;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void StartPreloadScenes()
        {
            if (scenesToPreload.Count == 0) return;

            onScenesLoading?.Invoke();

            _loadedSceneHandles.Clear();
            foreach (var sceneName in scenesToPreload)
            {
                var handle = Addressables.LoadSceneAsync(
                    sceneName,
                    UnityEngine.SceneManagement.LoadSceneMode.Additive,
                    activateOnLoad: false);

                handle.Completed += _ =>
                {
                    _loadedSceneHandles.Add(handle);
                    if (_loadedSceneHandles.Count == scenesToPreload.Count)
                        onScenesLoaded?.Invoke();
                };
            }
        }

        public void LoadScene(string sceneName)
        {
            if (_currentSceneHandle.HasValue && _currentSceneHandle.Value.IsValid())
            {
                Addressables.UnloadSceneAsync(_currentSceneHandle.Value);
                _currentSceneHandle = null;
            }

            AsyncOperationHandle<SceneInstance> preloaded = _loadedSceneHandles
                .Find(h => h.IsValid() &&
                           h.Status == AsyncOperationStatus.Succeeded &&
                           h.Result.Scene.name == sceneName);

            if (preloaded.IsValid())
            {
                preloaded.Result.ActivateAsync()
                    .completed += _ =>
                    {
                        Debug.Log($"Scene {sceneName} activated from cache.");
                        _currentSceneHandle = preloaded;
                    };
            }
            else
            {
                onScenesLoading?.Invoke();
                AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(
                    sceneName,
                    UnityEngine.SceneManagement.LoadSceneMode.Additive,
                    activateOnLoad: true);

                handle.Completed += _ =>
                {
                    Debug.Log($"Scene {sceneName} loaded.");
                    onScenesLoaded?.Invoke();
                    _currentSceneHandle = handle;
                };
            }
        }

        private void OnDestroy()
        {
            foreach (AsyncOperationHandle<SceneInstance> h in _loadedSceneHandles)
            {
                Addressables.UnloadSceneAsync(h);
            }

            _loadedSceneHandles.Clear();

            if (_currentSceneHandle.HasValue && _currentSceneHandle.Value.IsValid())
            {
                Addressables.UnloadSceneAsync(_currentSceneHandle.Value);
            }
        }
    }
}