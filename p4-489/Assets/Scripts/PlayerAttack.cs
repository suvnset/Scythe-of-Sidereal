using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Game game;
    public UIController UI;

    // scythe weapon components
    public GameObject scythe;
    SpriteRenderer scytheSprite;
    BoxCollider2D scytheCollider;
    Transform scytheTransform;
    [SerializeField] private Animator anim;

    // beam special weapon components
    public GameObject beam;
    SpriteRenderer beamRenderer;
    BoxCollider2D beamCollider;
    Transform beamTransform;
    Animator beamAnimator;

    // projectile
    public GameObject projectile;
    ProjectileMovement projectileMovement;

    // dagger weapon components
    public GameObject dagger;
    SpriteRenderer daggerSprite;
    BoxCollider2D daggerCollider;
    Transform daggerTransform;

    // player components
    public PlayerMovement playerMovement;
    public SpriteRenderer playerRenderer;
    public Transform playerTransform;
    public Rigidbody2D playerRigidbody;

    // audio effects
    AudioSource audioSource;
    [SerializeField] AudioClip scytheSFX;
    [SerializeField] AudioClip hitSFX;
    [SerializeField] AudioClip specialChargeSFX;
    [SerializeField] AudioClip specialSFX;

    public bool isAttacking;
    public bool missingWeapon;
    bool charging;
    bool specialReady;

    public int specialPoints;
    void Start()
    {
        GameObject gameObj = GameObject.Find("Game");
        game = gameObj.GetComponent<Game>();

        scytheSprite = scythe.GetComponent<SpriteRenderer>();
        scytheCollider = scythe.GetComponent<BoxCollider2D>();
        scytheTransform = scythe.GetComponent<Transform>();

        beamRenderer = beam.GetComponent<SpriteRenderer>();
        beamCollider = beam.GetComponent<BoxCollider2D>();
        beamTransform = beam.GetComponent<Transform>();
        beamAnimator = beam.GetComponent<Animator>();

        projectileMovement = projectile.GetComponent<ProjectileMovement>();

        daggerSprite = dagger.GetComponent<SpriteRenderer>();
        daggerCollider = dagger.gameObject.GetComponent<BoxCollider2D>();
        daggerTransform = dagger.gameObject.GetComponent<Transform>();

        audioSource = GetComponent<AudioSource>();

        isAttacking = false;
        missingWeapon = false;
        charging = false;
        specialReady = false;
        specialPoints = 0;
    }
    // Sound resource (future reference) : https://levelup.gitconnected.com/how-to-play-sound-effects-in-unity-6a122bb32970
    // Sound credits:
    // https://freesound.org/people/SonoFxAudio/sounds/649359/ - hit sound
    // https://freesound.org/people/Robhog/sounds/684749/ - pierce sound
    // https://freesound.org/people/colorsCrimsonTears/sounds/641897/ - special charge sound
    // https://freesound.org/people/deleted_user_1941307/sounds/152322/ - special sound
    // https://freesound.org/people/code_box/sounds/521781/ - bgm 
    void Update()
    {
        GetInput();

        // Activate special every 15 kills 
        if (specialPoints % 15 == 0 && specialPoints != 0 && !specialReady)
        {
            specialPoints -= 15;
            specialReady = true;
            audioSource.PlayOneShot(specialChargeSFX);
            UI.SpecialUI(true);
        }
    }

    void GetInput()
    {
        if (!isAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // attack with main weapon or dagger if scythe was thrown
                if (missingWeapon)
                {
                    StartCoroutine(WeakAttack());
                }
                else
                {
                    StartCoroutine(MeleeAttack());
                    anim.SetTrigger("Attack");
                }
            }

            // Start charging throw
            else if (Input.GetKeyDown(KeyCode.Mouse1) && !missingWeapon)
            {
                StartCharge();
            }
        }

        // Release throw
        if (charging && Input.GetKeyUp(KeyCode.Mouse1))
        {
            EndCharge();
        }

        // Special attack
        if (Input.GetKeyDown(KeyCode.E) && !isAttacking && specialReady)
        {
            StartCoroutine(SpecialAttack());
        }

    }

    // enable weak attack

    private IEnumerator WeakAttack()
    {
        audioSource.PlayOneShot(scytheSFX);
        daggerSprite.enabled = true;
        daggerCollider.enabled = true;
        isAttacking = true;

        // wait for cooldown to attack again
        yield return new WaitForSeconds(0.4f);

        isAttacking = false;

        daggerSprite.enabled = false;
        daggerCollider.enabled = false;
    }

    // enable colliders
    private IEnumerator MeleeAttack()
    {
        isAttacking = true;
        audioSource.PlayOneShot(scytheSFX);
        scytheSprite.enabled = true;
        scytheCollider.enabled = true;

        // wait for cooldown to attack again
        //Invoke("Cooldown", 0.8f);
        yield return new WaitForSeconds(0.75f);
        isAttacking = false;
        scytheSprite.enabled = false;
        scytheCollider.enabled = false;
    }

    // start counting how long the throw is charged for
    void StartCharge()
    {
        isAttacking = true;
        charging = true;
        projectileMovement.StartCharge();
        Debug.Log("CHARGING THROW");
    }

    // throw if charged long enough
    void EndCharge()
    {
        charging = false;
        isAttacking = false;
        missingWeapon = projectileMovement.EndCharge();

        if (missingWeapon)
        {
            UI.WeaponUI(false);
        }

        Debug.Log("RELEASED THROW");
    }

    private IEnumerator SpecialAttack()
    {
        beamAnimator.SetTrigger("SpecialAttack");
        audioSource.PlayOneShot(specialSFX);
        beamCollider.enabled = true;
        beamRenderer.enabled = true;
        isAttacking = true;

        // reduce player mobility during special attack
        playerMovement.speed = 0f;
        // TODO: rotate beam somehow

        // lasts for 3 seconds
        yield return new WaitForSeconds(3.0f);

        UI.SpecialUI(false);
        specialReady = false;
        isAttacking = false;
        specialPoints = 0;

        beamRenderer.enabled = false;
        beamCollider.enabled = false;

        // player movement back to normal
        playerMovement.speed = 9f;
    }

    public void KillEnemy()
    {
        // gain one special point for every enemy killed (excluding special kills)
        if (!specialReady)
        {
            specialPoints++;
            Debug.Log("SP UP!" + specialPoints);
        }
        audioSource.PlayOneShot(hitSFX);
    }
}
