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
        mPlayer.Move();

        for (int i = 0; i < mPlayer.mAttackButtons.Length; ++i)
        {
            if (mPlayer.mAttackButtons[i])
            {
                PlayerState_ATTACK attack =
              (PlayerState_ATTACK)mFsm.GetState(
                        (int)PlayerStateType.ATTACK);

                attack.AttackID = i;
                mPlayer.mFsm.SetCurrentState(
                    (int)PlayerStateType.ATTACK);
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

    private bool isAttacking = false; //added flag to track if attacking is in progress

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
        if (!isAttacking) //check if not already attacking
        {
            mPlayer.mAnimator.SetBool(mAttackName, true);
            isAttacking = true; //set flag to true

            //start coroutine to wait for the attack animation to complete
            mPlayer.StartCoroutine(WaitForAnimation());
        }
    }

    private IEnumerator WaitForAnimation()
    {
        //determine the animation clip based on the current attack ID
        string currentAnimationName = "Attack" + (mAttackID + 1).ToString();
        AnimationClip animationClip = mPlayer.mAnimator.runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == currentAnimationName);

        if (animationClip != null)
        {
            //wait for the duration of the specific animation (since there are 3 attacks)
            yield return new WaitForSeconds(animationClip.length);

            //code to execute after punch animation is complete
            isAttacking = false; //reset the punching flag
            mPlayer.mAttackCount++;
            Debug.Log(mPlayer.mAttackCount);

            //if the amount of attacks are equal to the max amount, recharge
            if (mPlayer.mAttackCount == mPlayer.mMaxAttackCount)
            {
                mPlayer.mAttackCount = 0;
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
    }

    public override void Update()
    {
        base.Update();

        if (!isAttacking && mPlayer.mAttackButtons[mAttackID])
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
