using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
        {
            Destroy(gameObject);
        }
    }
}
