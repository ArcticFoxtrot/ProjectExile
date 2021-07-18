using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    public float wallCheckOffset;
    public float climbCheckDist;
    public LayerMask climbCheckIgnoreLayer;
    private PlayerState activeState;
    public PlayerState defaultState;
    public PlayerState climbLedgeState;
    public PlayerState combatState;
    public PlayerState zTargetState;
    public PlayerState transitionToWalkState;
    public List<PlayerState> stateStack;
    public bool isPaused;

    public static PlayerFSM GetPlayerFSM(){
        return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFSM>();
    }


    // Start is called before the first frame update
    void Start()
    {
        stateStack = new List<PlayerState>();
        PushState(defaultState);
        //stateStack.Insert(0, defaultState);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(!isPaused){
            Time.timeScale = 1f;
            activeState = GetCurrentState();
            if(activeState != null){
                activeState.RunPlayerState();
            }
        } else {
            Time.timeScale = 0f;
        }
    }

    public PlayerState GetCurrentState()
    {
        if(stateStack.Count > 0){
            return stateStack[stateStack.Count - 1];
        } else {
            return null;
        }
    }

    public int GetStateStackCount(){
        if(stateStack != null){
            return stateStack.Count;
        } else {
            return 0;
        }
        
    }

    public void PopState(PlayerState state){
        stateStack.Remove(state);
    }

    public void PushState(PlayerState state){
        if(GetCurrentState() != state){
            stateStack.Insert(0, state);
            stateStack[0].StartPlayerState();
        }             
        
    }

}
