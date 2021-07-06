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

    public PlayerCombat(PlayerState state) : base(state)
    {
    }

    public override void StartPlayerState()
    {
        base.StartPlayerState();
        
    }

    public override void RunPlayerState()
    {
        base.RunPlayerState();
        GetCombatInput();
    }

    private void GetCombatInput()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            //TODO implement combos for light combat to chain attacks
            playerAnimationController.HandlePlayerIsAttacking(true, CombatInputType.Light, lightCombatInput);
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
    


}
