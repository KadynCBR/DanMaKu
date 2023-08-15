// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Cinemachine;
// using UnityEngine.VFX;
// using UnityEngine.UI;


// public class TwinStickControllerOLD : MonoBehaviour
// {
//     public float speed = 1;
//     public float shootRatePerSec = 1;
//     public float shotTimer;
//     public Vector2 LStick;
//     public Vector2 RStick;
//     public Vector3 velocity;
//     public Vector3 friction = new Vector3(0.74f, 0.99f, 0.74f);

//     public bool Dashing { get; private set; }

//     public bool Firing;
//     public bool parrying;
//     public state currentState;
//     float rotationangle;

//     public CharacterController controller;
//     public GameObject bulletPrefab;
//     public Transform bulletOffset;
//     public Transform offset;
//     public Animator anim;

//     public CinemachineVirtualCamera vcam;
//     private CinemachineTransposer vTrans;
//     public float camera_min;
//     public float camera_max;

//     public float grav;
//     public bool transitioningHappening;

//     public bool isWhite;
//     public Material mat;
//     Color ColorWHITE = new Color(216 / 255f, 212 / 255f, 200 / 255f);
//     Color ColorBLACK = new Color(67 / 255f, 66 / 255f, 61 / 255f);
//     public AudioSource lazershotSFX;
//     public AudioSource hitSFX;
//     public VisualEffect engineTrail;
//     public VisualEffect dashVFX;
//     public Transform dashVFXRotation;
//     public bool engineEffectOn;


//     [SerializeField] private BulletManager bulletManager;
//     private bool parryhappening;
//     public AudioSource parrySFX;

//     // public ImpactReceiver impactReceiver;
//     public float dashForce;
//     // public ActionMeter actionMeter;

//     public float transitionMeterCost = 50;
//     public float parryMeterCost = 20;
//     public float dashMeterCost = 30;

//     // // Reaction Comands
//     // public ReactionCommandListener reactionCommandListener;
//     // public bool reactionHappening;

//     void Start()
//     {
//         shotTimer = 0;
//         transitioningHappening = false;
//         isWhite = true;
//         mat.color = ColorWHITE;
//         AutoHookup();
//     }

//     // Look up all objects to see if they're in the scene. probably dirty but using scene template
//     // means these should _probably_ always be there.
//     void AutoHookup()
//     {
//         vTrans = vcam.GetCinemachineComponent<CinemachineTransposer>();
//         GameObject gamemanagerobj = GameObject.Find("GameManager");
//         bulletManager = gamemanagerobj.GetComponent<BulletManager>();
//         gm = gamemanagerobj.GetComponent<GameManager>();
//         if (bulletManager == null)
//             Debug.Log("No bullet manager found, Is 'GameManager' in scene with bullet manager script?");
//         if (gm == null)
//             Debug.Log("No game manager object found, is there a 'GameManager' in the scene with GameManager script?");
//         hp.hp_bar = GameObject.FindGameObjectWithTag("PlayerHPBar").GetComponent<Slider>();
//         actionMeter.meter_bar = GameObject.FindGameObjectWithTag("PlayerMeterBar").GetComponent<Slider>();
//         actionMeter.OverheatedUIBanner = GameObject.FindGameObjectWithTag("OverheatUI").GetComponent<CanvasGroup>();
//     }

//     void Update()
//     {
//         UpdateInput();

//         UpdateState();

//         UpdatePhysics();

//         UpdateAnimation();
//     }

//     private void UpdateAnimation()
//     {
//         // throw new NotImplementedException();
//     }

//     private void UpdateState()
//     {
//         switch (currentState)
//         {
//             case state.NEUTRAL:
//                 Neutral();
//                 break;
//             case state.TRANSITIONING:
//                 Transition();
//                 break;
//             case state.PARRY:
//                 ParryStateExec();
//                 break;
//             case state.DAMAGE:

//                 break;
//             case state.DEATH:

//                 break;
//             case state.REACTIONCOMMAND:
//                 PerformReactionCommand();
//                 break;
//             case state.DOWNFORTHECOUNT:
//                 offset.localRotation = Quaternion.Euler(0, 0, 0);
//                 break;
//         }
//     }

//     private void UpdatePhysics()
//     {
//         controller.Move(velocity);
//         velocity.Scale(friction);
//     }

//     void UpdateInput()
//     {
//         if (currentState == state.DOWNFORTHECOUNT)
//             return; // Downforthecount handled via timeline and cutscenes.
//         if (gm.gamePaused)
//             return;
//         if (reactionHappening)
//             return;
//         LStick = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
//         RStick = Input.mousePosition;
//         Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
//         Vector2 mouse_pos = new Vector2();
//         mouse_pos.x = Input.mousePosition.x - objectPos.x;
//         mouse_pos.y = Input.mousePosition.y - objectPos.y;
//         rotationangle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg; // Needed for PC
//         // rotationangle = -rotationangle;




//         // For dual stick
//         RStick = new Vector2(Input.GetAxisRaw("RightStickHorizontal"), Input.GetAxisRaw("RightStickVertical"));
//         // Debug.Log($"RSTICK: {LStick}");
//         if (RStick.magnitude > .1)
//         {
//             rotationangle = Mathf.Atan2(RStick.x, RStick.y) * Mathf.Rad2Deg - 90;
//             // offset.rotation = Quaternion.Euler(0, angle, 0);
//         }
//         else
//         {

//         }

//         Dashing = Input.GetButtonDown("Dash");
//         Firing = Input.GetButton("Fire1");
//         if (Input.GetButtonDown("Fire2"))
//         {
//             currentState = state.PARRY;
//         }

//         if (Input.GetButtonDown("Reaction") && reactionCommandListener.reactionActive)
//         {
//             currentState = state.REACTIONCOMMAND;
//         }

//         if (Input.GetButtonDown("Transition") || transitioningHappening)
//         {
//             currentState = state.TRANSITIONING;
//         }

//     }

//     void Movement()
//     {
//         LStick.Normalize();
//         if (Mathf.Abs(LStick.x) > 0.1)
//             velocity.x = speed * LStick.x * Time.deltaTime;
//         if (Mathf.Abs(LStick.y) > 0.1)
//             velocity.z = speed * LStick.y * Time.deltaTime;
//         if (Dashing)
//         {
//             if (actionMeter.MeterAvailable(dashMeterCost))
//             {
//                 actionMeter.Consume(dashMeterCost);
//                 if (LStick.magnitude >= .1)
//                 {
//                     Vector3 dir = new Vector3(LStick.x, 0, LStick.y);
//                     impactReceiver.AddImpact(dir, dashForce);
//                     dir.z *= -1; // Invert Y for the rotation offset.
//                     dashVFXRotation.transform.localRotation = Quaternion.Euler(0, Vector3.SignedAngle(dir, transform.forward, transform.up), 0);
//                     Debug.Log(dashVFXRotation.transform.localRotation.eulerAngles.y);
//                     dashVFX.SetFloat("Rotation", dashVFXRotation.transform.localRotation.eulerAngles.y);
//                     dashVFX.Play();
//                 }
//                 else
//                 {
//                     impactReceiver.AddImpact(offset.forward * -1, dashForce);
//                     Vector3 dir = offset.transform.rotation.eulerAngles;
//                     dir.z *= -1;
//                     dir.x *= -1;
//                     dir.y -= 180;
//                     dashVFXRotation.transform.localRotation = Quaternion.Euler(dir);
//                     Debug.Log(dashVFXRotation.transform.localRotation.eulerAngles.y);
//                     dashVFX.SetFloat("Rotation", dashVFXRotation.transform.localRotation.eulerAngles.y);
//                     dashVFX.Play();
//                 }
//             }
//             Dashing = false;
//         }
//         if (LStick.magnitude >= .1)
//         {
//             if (!engineEffectOn)
//             {
//                 engineTrail.Play();
//                 engineEffectOn = true;
//             }
//         }
//         else
//         {
//             // if (engineEffectOn) {
//             //     engineTrail.Stop();
//             //     engineEffectOn = false;
//             // }
//         }
//     }
//     void Rotation()
//     {
//         offset.rotation = Quaternion.Euler(0, -rotationangle - 90, 0);
//     }

//     void Neutral()
//     {
//         Movement();
//         Rotation();
//         GravityEffect();
//         if (Firing && !parrying)
//         {
//             FireBullet();
//         }
//         else if (parrying)
//         {
//             ParryStateExec();
//         }
//         shotTimer += Time.deltaTime;
//     }

//     void FireBullet()
//     {
//         if (shotTimer >= (1 / shootRatePerSec))
//         {
//             shotTimer = 0;
//             if (bulletManager.SpawnBullet(BulletType.TypePlayer, bulletOffset.position, bulletOffset.rotation))
//             {
//                 // Maybe the bullet itself should have the SFX?
//                 lazershotSFX.Play();
//             }
//         }
//     }

//     // Transition takes 3 seconds, 
//     void Transition()
//     {
//         if (transitioningHappening)
//         {
//             return;
//         }
//         else if (!actionMeter.MeterAvailable(transitionMeterCost))
//         {
//             currentState = state.NEUTRAL;
//             return;
//         }
//         else
//         {
//             actionMeter.Consume(transitionMeterCost);
//             transitioningHappening = true;
//             StartCoroutine(TransitionIter());
//             // StartCoroutine(cameraEffect(3f));
//             anim.SetTrigger("Transition"); // Play the animation
//         }
//     }

//     IEnumerator TransitionIter()
//     {
//         anim.updateMode = AnimatorUpdateMode.UnscaledTime;
//         gm.SlowTime();
//         yield return new WaitForSecondsRealtime(1.5f);
//         flipColor();
//         gm.flipColor();
//         yield return new WaitForSecondsRealtime(1.2f);
//         currentState = state.NEUTRAL;
//         transitioningHappening = false;
//         anim.updateMode = AnimatorUpdateMode.Normal;
//     }

//     IEnumerator ParryIter()
//     {
//         // Vector3 previous_friction = friction;
//         // friction = Vector3.one;
//         yield return new WaitForSeconds(.5f);
//         // friction = previous_friction;
//         currentState = state.NEUTRAL;
//         parryhappening = false;
//     }

//     void flipColor()
//     {
//         if (isWhite)
//         {
//             mat.color = ColorBLACK;
//         }
//         else
//         {
//             mat.color = ColorWHITE;
//         }
//         isWhite = !isWhite;
//     }

//     // IEnumerator cameraEffect(float duration)
//     // {
//     //     float evalPoint;
//     //     for (float i = 0; i < duration / 2; i += Time.unscaledDeltaTime)
//     //     {
//     //         evalPoint = Mathf.Lerp(0f, 1f, i / (duration / 2));
//     //         vTrans.m_FollowOffset.y = gm.reMap(
//     //             gm.timeCurve.Evaluate(evalPoint),
//     //             gm.timeCurve.Evaluate(0),
//     //             gm.timeCurve.Evaluate(.99f),
//     //             camera_min,
//     //             camera_max);

//     //         yield return null;
//     //     }
//     //     yield return new WaitForSecondsRealtime(.55f);
//     //     for (float i = 0; i < duration / 2; i += Time.unscaledDeltaTime)
//     //     {
//     //         evalPoint = Mathf.Lerp(1f, 0f, i / (duration / 2));
//     //         vTrans.m_FollowOffset.y = gm.reMap(
//     //             gm.timeCurve.Evaluate(evalPoint),
//     //             gm.timeCurve.Evaluate(0),
//     //             gm.timeCurve.Evaluate(1),
//     //             camera_min,
//     //             camera_max);
//     //         yield return null;
//     //     }
//     //     vTrans.m_FollowOffset.y = camera_min;
//     // }

//     void GravityEffect()
//     {
//         velocity.y += grav;
//         if (controller.isGrounded)
//             velocity.y = 0;
//     }

//     void ParryStateExec()
//     {
//         Movement();
//         if (parryhappening)
//             return;
//         if (!actionMeter.MeterAvailable(parryMeterCost))
//         {
//             parrying = false;
//             currentState = state.NEUTRAL;
//             return;
//         }
//         actionMeter.Consume(parryMeterCost);
//         parryhappening = true;
//         StartCoroutine(ParryIter());
//         anim.SetTrigger("Parry");
//     }

//     private void PerformReactionCommand()
//     {
//         // Will be up to reaction listener to end state? seems dangerous.
//         if (reactionHappening)
//             return;
//         reactionHappening = true;
//         reactionCommandListener.TriggerReaction();
//     }

//     public void ReactionFinished()
//     {
//         reactionHappening = false;
//         currentState = state.NEUTRAL;
//     }


//     void ParryBullet()
//     {
//         float angle = Mathf.Atan2(velocity.normalized.x, velocity.normalized.z) * Mathf.Rad2Deg;
//         if (bulletManager.SpawnBullet(BulletType.TypeH, transform.position, Quaternion.Euler(0, angle, 0), null))
//         {
//             parrySFX.Play();
//         }
//     }

//     // If parry happening check bullet type. 
//     // if attack is parryable and current state is parry
//     // parry the attack.
//     public bool GetHit(BulletBase bullet)
//     {
//         if (currentState == state.DOWNFORTHECOUNT)
//             return false;
//         if (currentState == state.TRANSITIONING)
//             return false;
//         if (currentState == state.PARRY && bullet.bulletType == BulletType.TypeB)
//         { // maybe make a parryable var.
//             ParryBullet();
//             Debug.Log("Shouldn't be getting hit.");
//             return true;
//         }
//         if (bullet.bulletType == BulletType.TypeD)
//         {
//             BulletTypeD btd = (BulletTypeD)bullet;
//             Vector3 dir = transform.position - bullet.gameObject.transform.position;
//             impactReceiver.AddImpact(dir, btd.impactForce);
//         }
//         else if (bullet.bulletType == BulletType.TypeC)
//         {
//             BulletTypeC btc = (BulletTypeC)bullet;
//             impactReceiver.AddExplosion(btc.explosionForce, btc.transform.position);
//         }
//         else if (bullet.bulletType == BulletType.TypeE)
//         {
//             BulletTypeE bte = (BulletTypeE)bullet;
//             Vector3 dir = transform.position - bullet.gameObject.transform.position;
//             impactReceiver.AddImpact(dir, bte.impactForce);
//         }


//         hp.Damage(bullet.damage);
//         cinemachineShake.ShakeCamera(.9f, .25f);
//         hitSFX.Play();
//         hitSFX.timeSamples = Mathf.FloorToInt(hitSFX.clip.samples * .07f);
//         return true;
//     }
// }
