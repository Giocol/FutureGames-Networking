using System;
using UnityEngine;

namespace Environment
{
    public class DamageDealer : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            Player.Player player = other.gameObject.GetComponent<Player.Player>();
            if(!player)
                return;
            player.OnTakeDamage();
            
            //play crash sfx, etc.
        }
    }
}