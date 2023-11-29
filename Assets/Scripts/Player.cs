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

  //punch count and max punch count for recharge logic
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
}
