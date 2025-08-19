using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float playerDamage = 500;
    public float enemyDamage = 200;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyMovementScript>().ChangeHealth(playerDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().ChangeHealth(enemyDamage);
            Destroy(gameObject);
        }
    }
}
