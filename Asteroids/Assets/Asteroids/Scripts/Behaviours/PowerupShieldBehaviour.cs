using UnityEngine;

    public class PowerupShieldBehaviour : PowerupBehaviour
    {
        public override void GrantPower()
        {
            ShieldBehaviour shield = GetShield();
            shield.activateShield(powerDuration);
            base.GrantPower();
        }
        public override void DenyPower()
        {
            ShieldBehaviour shield = GetShield();
            shield.deactivateShield();
            base.DenyPower();
        }
        ShieldBehaviour GetShield()
        {
            GameObject shield = ship.transform.Find("Shield").gameObject;
            return shield.GetComponentsInChildren<ShieldBehaviour>(true)[0];
        }
    }