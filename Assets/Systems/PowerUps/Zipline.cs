using System.Collections;

namespace Systems.PowerUps
{
    public class Zipline : PowerUpBase
    {
        protected override IEnumerator HandlePowerUp()
        {
            // Move the player to the origin.
            PlayerGameObject.gameObject.transform.position = new UnityEngine.Vector3(0, 0, 0);
            yield return null;
        }
    }
}
