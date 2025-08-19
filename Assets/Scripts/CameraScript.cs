using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Camera cam;
    public bool isInScope = false;
    public Vector3 nonScopPos = new Vector3(0, 7.3f, -12);
    public Vector3 scopePos = new Vector3(0, 1.5f, 3.8f);
    public void Start()
    {
        cam = GetComponent<Camera>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isInScope = !isInScope;
            if (isInScope)
            {
                transform.localPosition = scopePos;
                cam.fieldOfView = 30;
            }
            else
            {
                transform.localPosition = nonScopPos;
                cam.fieldOfView = 60;
            }
        }
    }
}
