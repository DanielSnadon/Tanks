using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float playerDamage = 500;
    public float enemyDamage = 200;
    EnemyMovementScript enemyScript;
    PlayerScript playerScript;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<EnemyMovementScript>())
            {
                enemyScript = collision.gameObject.GetComponent<EnemyMovementScript>();
                enemyScript.ChangeHealth(playerDamage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            playerScript = collision.gameObject.GetComponent<PlayerScript>();
            playerScript.ChangeHealth(enemyDamage);
            Destroy(gameObject);
        }
    }
}
