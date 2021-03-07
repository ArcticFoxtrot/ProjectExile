using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysChemFSM : MonoBehaviour
{
    protected PhysChemState activeState;

    void Update()
    {
        if(activeState != null){
            activeState.RunState();
        }
    }

    public void SetState(PhysChemState state){
        activeState = state;
        activeState.Start();
        
    }

    public PhysChemState GetCurrentState(){
        return activeState;
    }
}
