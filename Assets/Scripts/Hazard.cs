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
            print(" player has touch hazard");
            var respawn = other.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                respawn.KillAndRespawn();
            }
        }
    }
}