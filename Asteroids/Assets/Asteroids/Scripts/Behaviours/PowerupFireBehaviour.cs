using UnityEngine;

    public class PowerupFireBehaviour : PowerupBehaviour
    {
        public override void GrantPower()
        {
            SpaceshipBehaviour shooter = ship.GetComponent<SpaceshipBehaviour>();
            int weaponChoice = Random.Range(1, (int)SpaceshipBehaviour.Weapons.Count);
            shooter.activeWeapon = weaponChoice;
            base.GrantPower();
        }

        public override void DenyPower()
        {
            SpaceshipBehaviour shooter = ship.GetComponent<SpaceshipBehaviour>();
            shooter.activeWeapon = (int)SpaceshipBehaviour.Weapons.Default;
            base.DenyPower();
        }
    }