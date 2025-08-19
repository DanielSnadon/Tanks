using UnityEngine;
using UnityEngine.Animations;

public class EnemyMovementScript : MonoBehaviour
{
    private Rigidbody tankRb;
    public float enginePower;
    public bool isGoingToPoint;
    public bool isLookingAtPlayer;
    public float moveReload;
    public float currentTime;
    public float TankRotationSide;
    public float EngineRotationSpeed;
    public float headRotationSpeed;
    public float shootPower;
    private GameObject player;
    public GameObject head;
    public GameObject cannon;
    public GameObject bullet;
    void Start()
    {
        player = GameObject.Find("Tank");
        tankRb = GetComponent<Rigidbody>();
        currentTime = moveReload;
    }
    void Update()
    {
        MoveEnemyTank();
        LookAtPlayer();
    }

    public void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, cannon.transform.position, cannon.transform.rotation);
        Rigidbody newBulletRb = newBullet.GetComponent<Rigidbody>();
        newBulletRb.AddForce(newBullet.transform.forward * shootPower, ForceMode.Impulse);
        newBulletRb.AddForce(-newBullet.transform.up * 3, ForceMode.Impulse);
    }

    public void LookAtPlayer()
    {
        if (isLookingAtPlayer)
        {
            head.transform.Rotate(transform.up, Mathf.Sign(Vector3.Dot((player.transform.position - transform.position), head.transform.right)) * headRotationSpeed * Time.deltaTime);
        }
        Debug.DrawRay(cannon.transform.position, cannon.transform.forward * 100, Color.red);
    }

    public void MoveEnemyTank()
    {
        if (!isGoingToPoint)
        {
            currentTime -= Time.deltaTime;
            transform.Rotate(Vector3.up, TankRotationSide * EngineRotationSpeed * Time.deltaTime);
        }
        else
        {
            currentTime += Time.deltaTime;
            tankRb.AddForce(transform.forward * enginePower * tankRb.mass);
        }

        if (currentTime < 0)
        {
            Shoot();
            isGoingToPoint = true;
        }
        else if (currentTime > moveReload)
        {
            isGoingToPoint = false;
            TankRotationSide = Random.Range(-1, 1);
        }
    }
}
