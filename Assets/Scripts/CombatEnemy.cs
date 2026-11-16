using UnityEngine;

public class CombatEnemy : MonoBehaviour
{
    public Combat combat;
    public Enemy[] enemies = new Enemy[3];

    void Update()
    {
        if (combat.combatActive) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            combat.enemies = enemies;
            StartCoroutine(combat.StartBattle());
        }
    }
}
