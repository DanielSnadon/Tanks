using UnityEngine;

public class MenuSpinScript : MonoBehaviour
{
    public float spinSpeed = 1.0f;
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * spinSpeed);
    }
}
