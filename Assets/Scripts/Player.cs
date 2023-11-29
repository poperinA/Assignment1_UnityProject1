using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE.Patterns;
using PGGE;

public class Player : MonoBehaviour
{
  [HideInInspector]
  public FSM mFsm = new FSM();
  public Animator mAnimator;
  public PlayerMovement mPlayerMovement;

  // This is the maximum number of bullets that the player 
  // needs to fire before reloading.
  public int mMaxAttackBeforeReload = 1;

  // This is the total number of bullets that the 
  // player has.
  [HideInInspector]
  public int mAmunitionCount = 100;

  // This is the count of bullets in the magazine.
  [HideInInspector]
  public int mBulletsInMagazine = 1;

  public int mMaxPunchCount = 3;
  [HideInInspector]
  public int mPunchCount = 0;


    [HideInInspector]
  public bool[] mAttackButtons = new bool[3];

  public Transform mGunTransform;
  public LayerMask mPlayerMask;
  public AudioSource mAudioSource;


  public int[] RoundsPerSecond = new int[3];
  bool[] mFiring = new bool[3];


  // Start is called before the first frame update
  void Start()
  {
    mFsm.Add(new PlayerState_MOVEMENT(this));
    mFsm.Add(new PlayerState_ATTACK(this));
    mFsm.Add(new PlayerState_RELOAD(this));
    mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);

    PlayerConstants.PlayerMask = mPlayerMask;
  }

  void Update()
  {
    mFsm.Update();


    // For Student ----------------------------------------------------//
    // Implement the logic of button clicks for shooting. 
    //-----------------------------------------------------------------//

    if (Input.GetButtonDown("Fire1"))
    {
      mAttackButtons[0] = true;
      mAttackButtons[1] = false;
      mAttackButtons[2] = false;
    }
    else
    {
      mAttackButtons[0] = false;
    }

    if (Input.GetButtonDown("Fire2"))
    {
      mAttackButtons[0] = false;
      mAttackButtons[1] = true;
      mAttackButtons[2] = false;
    }
    else
    {
      mAttackButtons[1] = false;
    }

    if (Input.GetButtonDown("Fire3"))
    {
      mAttackButtons[0] = false;
      mAttackButtons[1] = false;
      mAttackButtons[2] = true;
    }
    else
    {
      mAttackButtons[2] = false;
    }
  }

  public void Move()
  {
    mPlayerMovement.HandleInputs();
    mPlayerMovement.Move();
  }

  public void NoAmmo()
  {

  }

  public void Reload()
  {
    StartCoroutine(Coroutine_DelayReloadSound());
  }

  IEnumerator Coroutine_DelayReloadSound(float duration = 1.0f)
  {
    yield return new WaitForSeconds(duration);
  }

}
