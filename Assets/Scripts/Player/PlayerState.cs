using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    protected readonly PlayerState playerState;

    public PlayerFSM playerFSM;
    public PlayerState defaultState;


    public PlayerState(PlayerState state){
        playerState = state;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerFSM = PlayerFSM.GetPlayerFSM();
    }

    public virtual void StartPlayerState(){

    }

    public virtual void RunPlayerState(){
        StateCheck();
    }

    public virtual void EndPlayerState(){
        playerFSM.PopState(this);
        if(playerFSM.GetCurrentState() == null){
            playerFSM.PushState(defaultState);
        }
    }

    public virtual void StateCheck()
    {
        
    }


}
