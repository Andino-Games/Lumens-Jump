using System.Collections;

namespace Systems.PowerUps
{
    public class Rocket : PowerUpBase
    {
        // Requirements
        
        protected override IEnumerator HandlePowerUp()
        {
            
            PlayerGameObject.gameObject.name = powerUpName;
            yield return null;
        }
    }
}
