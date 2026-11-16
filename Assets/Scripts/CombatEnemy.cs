using UnityEngine;

public class CombatEnemy : MonoBehaviour
{
    public Combat combat;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(combat.StartBattle());
        }
    }
}
