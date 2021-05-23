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
    public List<PlayerState> stateStack;

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
        activeState = GetCurrentState();
        if(activeState != null){
            activeState.RunPlayerState();
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
        Debug.Log("Inserting state");
        if(GetCurrentState() != state){
            stateStack.Insert(0, state);
            stateStack[0].StartPlayerState();
        }             
        
    }

}
