using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE.Patterns;
using System.Linq;

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

    private bool isPunching = false; // Added flag to track if punching is in progress

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
        if (!isPunching) // Check if not already punching
        {
            mPlayer.mAnimator.SetBool(mAttackName, true);
            isPunching = true;

            // Start coroutine to wait for the punch animation to complete
            mPlayer.StartCoroutine(WaitForPunchAnimation());
        }
    }

    private IEnumerator WaitForPunchAnimation()
    {
        // Determine the animation clip based on the current attack ID
        string currentAnimationName = "Attack" + (mAttackID + 1).ToString();
        AnimationClip animationClip = mPlayer.mAnimator.runtimeAnimatorController.animationClips.FirstOrDefault(clip => clip.name == currentAnimationName);

        if (animationClip != null)
        {
            // Wait for the duration of the animation
            yield return new WaitForSeconds(animationClip.length);

            // Code to execute after punch animation is complete
            isPunching = false; // Reset the punching flag
            mPlayer.mPunchCount++;
            Debug.Log(mPlayer.mPunchCount);

            if (mPlayer.mPunchCount == mPlayer.mMaxPunchCount)
            {
                mPlayer.mPunchCount = 0;
                mFsm.SetCurrentState((int)PlayerStateType.RELOAD);
            }
        }
        else
        {
            Debug.LogError("Animation clip not found for: " + currentAnimationName);
            foreach (var clip in mPlayer.mAnimator.runtimeAnimatorController.animationClips)
            {
                Debug.Log("Clip Name: " + clip.name);
            }

        }
    }


    public override void Exit()
    {
        mPlayer.mAnimator.SetBool(mAttackName, false);
        // No need to increment punch count or set state here
    }

    public override void Update()
    {
        base.Update();

        if (!isPunching && mPlayer.mAttackButtons[mAttackID])
        {
            mPlayer.mFsm.SetCurrentState((int)PlayerStateType.ATTACK);
        }
        else
        {
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
