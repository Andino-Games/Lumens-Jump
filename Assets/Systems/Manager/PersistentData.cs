using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using System;
using Systems.Utils;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Systems.Manager
{
    public class PersistentData : Singleton<PersistentData>
    {
        private const string LEADERBOARD_ID = "Lumens_Rank";

        public Action<int> OnCurrentScoreChanged;
        
        private int _currentScore;
        private int _highScore;

        public int CurrentScore => _currentScore;
        public int HighScore => _highScore;
        public bool resetHighScore;
        public bool resetTimeWithoutAds;

        public float TimeWithoutAds
        {
            get
            {
                return PlayerPrefs.GetFloat("TWA", 0f);
            }
            set
            {
                PlayerPrefs.SetFloat("TWA", value);
            }
        }

        public float TimeBetweenAds
        {
            get
            {
                return PlayerPrefs.GetFloat("TBA", 0f);
            }
            set
            {
                PlayerPrefs.SetFloat("TBA", value);
            }
        }

        private async void Start()
        {
            if (resetHighScore == true)
            {
                PlayerPrefs.SetInt("Score", 0);
            }
            if (resetTimeWithoutAds == true)
            {
                PlayerPrefs.SetFloat("TWA", 0);
            }

            DontDestroyOnLoad(gameObject);

            // Inicializamos la carga de datos al arrancar el sistema.
            await SignInAnonymouslyAsync();
            LoadHighScore();
        }

        public int AddPoints()
        {
            _currentScore++;
            OnCurrentScoreChanged?.Invoke(_currentScore);
            return _currentScore;
        }        
        
        /// Carga el récord guardado localmente y actualiza la variable interna.
        public int LoadHighScore()
        {            
            _highScore = PlayerPrefs.GetInt("Score", 0);
            return _highScore;
        }
        
        public void ResetScore() => _currentScore = 0;
        
        
        /// Compara el puntaje actual con el récord y guarda si es mayor.
        public async Task SaveHighScore()
        {
            if (_currentScore > _highScore)
            {
                _highScore = _currentScore;
                PlayerPrefs.SetInt("Score", _currentScore);
                PlayerPrefs.Save();

                await SubmitScoreAsync(_currentScore);
            }
        }

        private async Task SignInAnonymouslyAsync()
        {
            try
            {
                await UnityServices.InitializeAsync();

                if (AuthenticationService.Instance.IsSignedIn == false)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error al inicializar Unity Services: {e.Message}");
            }
        }

        private async Task SubmitScoreAsync(int score)
        {
            try
            {
                await LeaderboardsService.Instance.AddPlayerScoreAsync(LEADERBOARD_ID, score);
            }
            catch (Exception e) 
            {
                Debug.LogError($"Error al subir el puntaje al Leaderboard: {e.Message}");
            }
        }

        public async Task<List<LeaderboardEntry>> GetTopScoresAsync(int limit = 10)
        {
            try
            {
                var options = new GetScoresOptions { Limit = limit };

                LeaderboardScoresPage result = await LeaderboardsService.Instance.GetScoresAsync(LEADERBOARD_ID, options);

                return result.Results;
            }
            catch (Exception e) 
            {
                Debug.LogError($"Error al obtener la lista del Leaderboard: {e.Message}");
                return null;
            }
        }
    }
}