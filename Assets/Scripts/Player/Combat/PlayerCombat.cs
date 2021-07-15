using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : PlayerState
{

    public CombatInputType latestInputType = CombatInputType.Block;
    [SerializeField] PlayerAnimationController playerAnimationController;
    private int lightCombatInput;
    private int heavyCombatInput;
    private bool blockInput;
    private PlayerState startingState;

    public PlayerCombat(PlayerState state) : base(state)
    {
    }

    public override void StartPlayerState()
    {

        base.StartPlayerState();
        playerAnimationController.HandleCombatAnimations();
    }

    public override void RunPlayerState()
    {
        base.RunPlayerState();
        GetCombatInput();
    
    }

    public void HandleInputCount()
    {
        lightCombatInput = 0;
        heavyCombatInput = 0;
        blockInput = false;
    }

    private void GetCombatInput()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            //TODO implement combos for light combat to chain attacks
            //playerAnimationController.HandlePlayerIsAttacking(true, CombatInputType.Light, lightCombatInput);
            playerAnimationController.HandleCombatAnimations();
        }

    }

    public override void EndPlayerState()
    {
        base.EndPlayerState();
    }

    public override void StateCheck(){

    }

    public override void SetStartCombatInput(CombatInputType type)
    {
        //Debug.Log("Setting combat type to " + type);
        latestInputType = type;
    }

    public override void SetStartingState(PlayerState state){
        startingState = state;
    }

    internal void HandleEndState()
    {
        //return to the starting state
        playerFSM.PushState(startingState);
        EndPlayerState();
    }
}
