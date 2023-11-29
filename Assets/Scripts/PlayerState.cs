using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE.Patterns;

public enum PlayerStateType
{
    MOVEMENT = 0,
    ATTACK,
    RELOAD,
}

public class PlayerState : FSMState
{
    protected Player mPlayer = null;

    public PlayerState(Player player) 
        : base()
    {
        mPlayer = player;
        mFsm = mPlayer.mFsm;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

public class PlayerState_MOVEMENT : PlayerState
{
    public PlayerState_MOVEMENT(Player player) : base(player)
    {
        mId = (int)(PlayerStateType.MOVEMENT);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        // For Student ---------------------------------------------------//
        // Implement the logic of player movement. 
        //----------------------------------------------------------------//
        // Hint:
        //----------------------------------------------------------------//
        // You should remember that the logic for movement
        // has already been implemented in PlayerMovement.cs.
        // So, how do we make use of that?
        // We certainly do not want to copy and paste the movement 
        // code from PlayerMovement to here.
        // Think of a way to call the Move method. 
        //
        // You should also
        // check if fire buttons are pressed so that 
        // you can transit to ATTACK state.

        mPlayer.Move();

        for (int i = 0; i < mPlayer.mAttackButtons.Length; ++i)
        {
            if (mPlayer.mAttackButtons[i])
            {
                if (mPlayer.mBulletsInMagazine > 0)
                {
                    PlayerState_ATTACK attack =
                  (PlayerState_ATTACK)mFsm.GetState(
                            (int)PlayerStateType.ATTACK);

                    attack.AttackID = i;
                    mPlayer.mFsm.SetCurrentState(
                        (int)PlayerStateType.ATTACK);
                }
                else
                {
                    Debug.Log("No more ammo left");
                }
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

public class PlayerState_ATTACK : PlayerState
{
    private int mAttackID = 0;
    private string mAttackName;

    public int AttackID
    {
        get
        {
            return mAttackID;
        }
        set
        {
            mAttackID = value;
            mAttackName = "Attack" + (mAttackID + 1).ToString();
        }
    }

    public PlayerState_ATTACK(Player player) : base(player)
    {
        mId = (int)(PlayerStateType.ATTACK);
    }

    public override void Enter()
    {
        mPlayer.mAnimator.SetBool(mAttackName, true);
        mPlayer.mPunchCount++;
        Debug.Log(mPlayer.mPunchCount);
        Debug.Log(mPlayer.mMaxPunchCount);

        if (mPlayer.mPunchCount > mPlayer.mMaxPunchCount)
        {
            mPlayer.mPunchCount = 0;
            mPlayer.mFsm.SetCurrentState((int)PlayerStateType.RELOAD);
        }
    }
    public override void Exit()
    {
        mPlayer.mAnimator.SetBool(mAttackName, false);
    }
    public override void Update()
    {
        base.Update();

        if (mPlayer.mAttackButtons[mAttackID])
        {
            mPlayer.mAnimator.SetBool(mAttackName, true);
        }

        else
        {
            mPlayer.mAnimator.SetBool(mAttackName, false);
            mPlayer.mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
        }
    }
}

public class PlayerState_RELOAD : PlayerState
{
    public float ReloadTime = 3.0f;
    float dt = 0.0f;
    public int previousState;
    public PlayerState_RELOAD(Player player) : base(player)
    {
        mId = (int)(PlayerStateType.RELOAD);
    }

    public override void Enter()
    {
        mPlayer.mAnimator.SetTrigger("Reload");
        mPlayer.Reload();
        dt = 0.0f;
    }
    public override void Exit()
    {
    }

    public override void Update()
    {
        dt += Time.deltaTime;
        if (dt >= ReloadTime)
        {
            mPlayer.mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
        }
    }

    public override void FixedUpdate()
    {
    }
}
