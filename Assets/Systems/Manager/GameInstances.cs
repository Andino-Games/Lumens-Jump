using Systems.Utils;
using UnityEngine;

namespace Systems.Manager
{
    public class GameInstances : Singleton<GameInstances>
    {
        public GameObject player;

        private void Start()
        {
            if (!player)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }
    }
}