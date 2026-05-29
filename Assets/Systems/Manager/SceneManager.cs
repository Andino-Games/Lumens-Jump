using System.Collections;
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
        private bool _bootSceneCleaned;
        private bool _isLoading;

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
            if (_isLoading)
            {
                Debug.LogWarning($"SceneManager: LoadScene('{sceneName}') ignored — another load is in progress.");
                return;
            }
            StartCoroutine(LoadSceneRoutine(sceneName));
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            _isLoading = true;
            onScenesLoading?.Invoke();

            // Guardar el handle de la escena anterior para descargarlo DESPUÉS de cargar la nueva
            var previousHandle = _currentSceneHandle;
            _currentSceneHandle = null;

            // --- PASO 1: CARGAR PRIMERO la nueva escena ---
            // Cargamos la nueva escena antes de descargar la anterior.
            // Esto evita el error de Unity: "Cannot unload the last loaded scene"
            AsyncOperationHandle<SceneInstance> preloaded = _loadedSceneHandles
                .Find(h => h.IsValid() &&
                           h.Status == AsyncOperationStatus.Succeeded &&
                           h.Result.Scene.name == sceneName);

            if (preloaded.IsValid())
            {
                var activateOp = preloaded.Result.ActivateAsync();
                while (!activateOp.isDone)
                    yield return null;

                Debug.Log($"Scene {sceneName} activated from cache.");
                _currentSceneHandle = preloaded;
            }
            else
            {
                var handle = Addressables.LoadSceneAsync(
                    sceneName,
                    UnityEngine.SceneManagement.LoadSceneMode.Additive,
                    activateOnLoad: true);

                yield return handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log($"Scene {sceneName} loaded.");
                    _currentSceneHandle = handle;
                }
                else
                {
                    Debug.LogError($"Failed to load scene: {sceneName}");
                    _isLoading = false;
                    yield break;
                }
            }

            // Establecer la nueva escena como la escena activa de Unity
            if (_currentSceneHandle.HasValue)
            {
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(
                    _currentSceneHandle.Value.Result.Scene);
            }

            // --- PASO 2: DESCARGAR DESPUÉS la escena anterior ---
            if (previousHandle.HasValue && previousHandle.Value.IsValid())
            {
                var unloadOp = Addressables.UnloadSceneAsync(previousHandle.Value);
                if (unloadOp.IsValid())
                    yield return unloadOp;
            }

            // --- PASO 3: Limpiar la escena nativa de arranque (IntroScene) ---
            CleanupBootSceneIfNeeded();

            _isLoading = false;
            onScenesLoaded?.Invoke();
        }

        private void CleanupBootSceneIfNeeded()
        {
            if (_bootSceneCleaned) return;

            var bootScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("IntroScene");
            if (bootScene.isLoaded)
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(bootScene);
                Debug.Log("IntroScene cleaned up successfully.");
                _bootSceneCleaned = true;
            }
        }

        private void OnDestroy()
        {
            foreach (var h in _loadedSceneHandles)
            {
                if (h.IsValid())
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