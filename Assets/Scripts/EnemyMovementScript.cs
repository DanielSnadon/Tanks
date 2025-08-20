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
    public float health;
    private AudioSource movementAudio;
    public ParticleSystem rideParticle1;
    public ParticleSystem rideParticle2;
    public AudioClip shootSound;
    void Start()
    {
        player = GameObject.Find("Tank");
        tankRb = GetComponent<Rigidbody>();
        currentTime = moveReload;
        movementAudio = GetComponent<AudioSource>();
    }
    void Update()
    {
        MoveEnemyTank();
        LookAtPlayer();
        checkDeath();
    }

    public void Shoot()
    {
        movementAudio.PlayOneShot(shootSound);
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
            rideParticle1.Play();
            rideParticle2.Play();
        }
        else if (currentTime > moveReload)
        {
            isGoingToPoint = false;
            TankRotationSide = Random.Range(-1, 1);
            rideParticle1.Stop();
            rideParticle2.Stop();
        }
    }

    void checkDeath()
    {
        if (health <= 0)
        {
            GameObject enemyBodyGO = gameObject.transform.Find("EnemyBody").gameObject;
            GameObject headGO = gameObject.transform.Find("Head").gameObject;
            GameObject cannonGO = headGO.transform.Find("Cannon").gameObject;
            enemyBodyGO.GetComponent<Renderer>().material.color = Color.white;
            headGO.GetComponent<Renderer>().material.color = Color.white;
            cannonGO.GetComponent<Renderer>().material.color = Color.white;
            movementAudio.Stop();
            Destroy(GetComponent<EnemyMovementScript>());
        }
    }

    public void ChangeHealth(float amount)
    {
        health -= amount;
    }
}
