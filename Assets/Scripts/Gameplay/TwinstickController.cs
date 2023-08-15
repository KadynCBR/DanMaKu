using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CherryTeaGames.Core.Events;
using DG.Tweening;
using UnityEngine.SceneManagement;

public enum state
{
    NEUTRAL,
    TRANSITIONING,
    DAMAGE,
    PARRY,
    DEATH,
    STUNLOCK,
    REACTIONCOMMAND,
    DOWNFORTHECOUNT
}

public class TwinstickController : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed = 1;
    public float dodgePower = 1.8f;
    public Vector3 velocity;
    public Vector3 friction = new Vector3(0.74f, 0.99f, 0.74f);
    public float gravity;

    [Header("Shooting")]
    public float shootRatePerSec = 1;
    public float shotTimer;
    public AudioSource bulletSFX;
    private bool isShooting;
    private Transform bulletOffset;

    [Header("Environment")]
    public LayerMask whatisground;
    private Vector3 mouseWorldPosition;
    private Camera main_cam;
    private CharacterController myController;
    private BulletManager bulletManager;
    private Animator animator;

    [Header("Visual")]
    public float impulseForce = 0.5f;
    Color colorWHITE = new Color(216 / 255f, 212 / 255f, 200 / 255f);
    Color colorBLACK = new Color(67 / 255f, 66 / 255f, 61 / 255f);
    private bool isWhite;
    private Material shipMaterial;

    [Header("Toggles")]
    public bool canMove;

    [Header("Events")]
    public GameEvent transitionEvent;

    private PlayerControls controls;
    private Vector2 rightStick;
    private Vector2 leftStick;
    private bool isUsingController;

    void OnEnable() { controls.Enable(); }
    void OnDisable() { controls.Disable(); }

    void Awake()
    {
        controls = new PlayerControls();
        controls.GamePlay.Dodge.performed += ctx => { OnDodge(); };
        controls.GamePlay.Special.performed += ctx => { OnParry(); };
        controls.GamePlay.Move.performed += ctx => { leftStick = ctx.ReadValue<Vector2>(); };
        controls.GamePlay.Move.canceled += ctx => { leftStick = new Vector2(0, 0); };
        controls.GamePlay.ShootLook.performed += ctx => { rightStick = ctx.ReadValue<Vector2>(); };
        controls.GamePlay.ShootLook.canceled += ctx => { rightStick = new Vector2(0, 0); };
        controls.GamePlay.Shoot.performed += ctx => { isShooting = true; };
        controls.GamePlay.Shoot.canceled += ctx => { isShooting = false; };
        controls.GamePlay.Transition.performed += ctx => { OnTransition(); };
        Setup();
    }

    void Setup()
    {
        main_cam = Camera.main;
        myController = GetComponent<CharacterController>();
        bulletManager = GameObject.Find("BulletManager").GetComponent<BulletManager>();
        bulletOffset = transform.Find("Offset/BulletOffset");
        shotTimer = 0;
        animator = GetComponentInChildren<Animator>();
        shipMaterial = GetComponentInChildren<Renderer>().material;
        isWhite = true;
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        GetMousePosition();
        DetermineControllerUse();
        HandleMovement();
        HandleAim();
        HandleShoot();
        shotTimer += Time.deltaTime;
        velocity.y += gravity;
        myController.Move(velocity);
        if (myController.isGrounded) velocity.y = 0;
        velocity.Scale(friction);
    }

    private void HandleShoot()
    {
        if (!isShooting) return;
        if (shotTimer >= (1 / shootRatePerSec))
        {
            shotTimer = 0;
            bulletManager.SpawnBullet(BulletType.TypePlayer, bulletOffset.position, bulletOffset.rotation);
            bulletSFX.Play();
        }
    }

    private void HandleAim()
    {
        Quaternion LookDir = transform.rotation;
        if (isUsingController)
        {
            if (rightStick.magnitude > 0.01f) // controller deadzones
            {
                float angle = Mathf.Atan2(rightStick.x, rightStick.y) * Mathf.Rad2Deg + main_cam.transform.eulerAngles.y;
                LookDir = Quaternion.Euler(0f, angle, 0f);
            }
        }
        else
        {
            Vector3 mov = mouseWorldPosition - transform.position;
            mov.Normalize();
            LookDir = Quaternion.LookRotation(mov);
        }
        transform.rotation = LookDir;
    }

    private void HandleMovement()
    {
        // Camera Relative stick movement
        if (leftStick.magnitude > 0.01f)
        {
            float angle = Mathf.Atan2(leftStick.x, leftStick.y) * Mathf.Rad2Deg + main_cam.transform.eulerAngles.y;
            Vector3 velocityDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            velocity += velocityDirection * moveSpeed;
        }
    }

    private Vector2 previousMousePos;
    private void DetermineControllerUse()
    {
        if (Mouse.current.position.ReadValue() != previousMousePos)
        {
            isUsingController = false;
            previousMousePos = Mouse.current.position.ReadValue();
        }
        if (rightStick.magnitude > 0.01f)
            isUsingController = true;
    }

    void GetMousePosition()
    {
        Ray _ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit _rayhit;
        float max_distance = 99999;
        if (Physics.Raycast(_ray, out _rayhit, max_distance, whatisground))
        {
            mouseWorldPosition = _rayhit.point;
            mouseWorldPosition.y = transform.position.y;
        }
    }

    void OnDodge()
    {
        if (!canMove) return;
        float angle = MathF.Atan2(leftStick.x, leftStick.y) * Mathf.Rad2Deg + main_cam.transform.eulerAngles.y;
        Vector3 velocityDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
        velocity += velocityDirection * dodgePower;
    }

    private void OnParry()
    {
        if (!canMove) return;
        animator.SetTrigger("Parry");
    }

    private void OnTransition()
    {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        transitionEvent.Raise();
        canMove = false;
        animator.SetTrigger("Transition");
        DOVirtual.DelayedCall(2.9f, () =>
        {
            canMove = true;
            animator.updateMode = AnimatorUpdateMode.Normal;
        });
        DOVirtual.DelayedCall(1.5f, () => { SwitchColors(); });
    }

    private void SwitchColors()
    {
        if (isWhite)
        {
            shipMaterial.color = colorBLACK;
        }
        else
        {
            shipMaterial.color = colorWHITE;
        }
        isWhite = !isWhite;

    }

    public void OnDeath()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnShotKnockback(float val)
    {
        velocity += -transform.forward * val;
    }

    public void ActivateShip()
    {
        DOVirtual.DelayedCall(1.2f, () =>
        {
            canMove = true;
        });
    }

}
