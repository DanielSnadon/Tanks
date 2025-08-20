using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GUILayout;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody tankRb;
    public GameObject scope;
    public GameObject realScope;

    public ParticleSystem rideParticle1;
    public ParticleSystem rideParticle2;

    private AudioSource headAudio;
    private AudioSource cameraAudio;
    private AudioSource movementAudio;
    public AudioClip shootSound;


    public bool isDriveParticlesOn;

    public float health;

    public Camera mainCamera;
    public float cameraOffset = 5.0f;
    public Vector3 neededSpin;
    public Vector3 currentSpin;
    public float spinSide;

    public float enginePower;
    public float engineRotationPower;

    public float shootPower;
    public float reloadTime;
    public float currentTime = 0;
    public Vector3 reloadTimeOffset = new Vector3(0, 60, 60);

    public TextMeshProUGUI reloadText;
    public GameObject head;
    public GameObject bullet;
    public GameObject cannon;
    public GameObject healthLine;
    public bool reloaded;

    public float headRotationPower;
    void Start()
    {
        tankRb = GetComponent<Rigidbody>();
        movementAudio = GetComponent<AudioSource>();
        headAudio = head.GetComponent<AudioSource>();
        cameraAudio = mainCamera.GetComponent<AudioSource>();
    }

    void Update()
    {
        MoveTank();
        RotateHead();
        Shoot();
        ShowHealth();
    }
    
    void ShowHealth()
    {
        healthLine.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health);
    }
    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && reloaded)
        {
            GameObject newBullet = Instantiate(bullet, cannon.transform.position, cannon.transform.rotation);
            Rigidbody newBulletRb = newBullet.GetComponent<Rigidbody>();
            newBulletRb.AddForce(newBullet.transform.forward * shootPower, ForceMode.Impulse);
            newBulletRb.AddForce(-newBullet.transform.up * 3, ForceMode.Impulse);
            reloaded = false;
            currentTime = reloadTime;
            reloadText.enabled = true;
            cameraAudio.PlayOneShot(shootSound);

        }
        if (!reloaded)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0) {
                reloadText.enabled = false;
                reloaded = true;
            }
            reloadText.text = System.Math.Round(currentTime, 2).ToString();
        }
    }

    void RotateHead()
    {
        //ScreenMouse
        Vector3 screenMousePos = Input.mousePosition;
        screenMousePos.z = transform.position.z + cameraOffset;

        //WorldMouse
        Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(screenMousePos);

        scope.transform.position = mainCamera.WorldToScreenPoint(worldMousePos);
        reloadText.transform.position = mainCamera.WorldToScreenPoint(worldMousePos) + reloadTimeOffset;

        worldMousePos.y = cannon.transform.forward.y;


        //Spins rotations
        neededSpin = (worldMousePos - cannon.transform.position).normalized;
        currentSpin = cannon.transform.right.normalized;

        realScope.transform.position = mainCamera.WorldToScreenPoint(transform.position + cannon.transform.forward.normalized * 30) + 50 * Vector3.up;

        spinSide = Mathf.Sign(Vector3.Dot(neededSpin, currentSpin));
        //Spin
        if (Mathf.Abs(Vector3.Dot(neededSpin, currentSpin)) > 0.01)
        {
            head.transform.Rotate(transform.up, headRotationPower * Time.deltaTime * spinSide);

            if (headAudio.volume <= 0.2f)
            {
                headAudio.volume += 0.15f * Time.deltaTime;
            }
        }
        else
        {
            headAudio.volume -= 0.25f * Time.deltaTime;
        }


            //Debug
            Debug.DrawRay(cannon.transform.position, cannon.transform.forward * 100, Color.green);
    }

    void MoveTank()
    {
        tankRb.AddForce(transform.forward * enginePower * tankRb.mass * Input.GetAxis("Vertical"));
        transform.Rotate(transform.up * engineRotationPower * Time.deltaTime * Input.GetAxis("Horizontal"));
        if (tankRb.GetAccumulatedForce().magnitude >= 4 || (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.6f))
        {
            if (movementAudio.volume < 0.5)
            {
                movementAudio.volume += 0.1f * Time.deltaTime;
            }
            if (!isDriveParticlesOn)
            {
                rideParticle1.Play();
                rideParticle2.Play();
                movementAudio.Play();
                isDriveParticlesOn = true;
            }
            
        }
        else if (tankRb.GetAccumulatedForce().magnitude == 0 || (Input.GetAxis("Horizontal") == 0))
        {
            movementAudio.volume -= 0.1f * Time.deltaTime;
            if (isDriveParticlesOn)
            {
                rideParticle1.Stop();
                rideParticle2.Stop();
                isDriveParticlesOn = false;
            }
        }

    }

    public void ChangeHealth(float amount)
    {
        health -= amount;
        ShowHealth();
    }
}
