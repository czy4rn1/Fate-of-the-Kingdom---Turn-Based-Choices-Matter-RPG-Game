using System.Collections;
using UnityEngine;

public class CombatEnemy : MonoBehaviour
{
    public Combat combat;
    public Enemy[] enemies = new Enemy[3];

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(StartCombat());
        }
    }

    IEnumerator StartCombat()
    {
        combat.enemies = enemies;
        StartCoroutine(combat.StartBattle());
        while (!combat.combatActive) yield return null;
        Destroy(gameObject);
    }
}
