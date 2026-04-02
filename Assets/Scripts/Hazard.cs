using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    void Reset()
    {
        var c = GetComponent<Collider2D>();
        if (c) c.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("player has touch hazard");
            var respawn = other.GetComponent<PlayerRespawn>();
            
            if (respawn != null)
            {
                // 1. Shake THIS specific player's camera
                if (respawn.myCameraShake != null)
                {
                    respawn.myCameraShake.TriggerShake(0.2f, 0.4f);
                }

                // 2. Flash THIS specific player's screen
                if (respawn.myScreenFlash != null)
                {
                    respawn.myScreenFlash.FlashScreen();
                }

                // 3. Kill and respawn
                respawn.KillAndRespawn();
            }
        }
    }
}