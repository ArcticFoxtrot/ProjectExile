using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysChemFSM : MonoBehaviour
{
    protected PhysChemState activeState;

    public List<PhysChemState> stateStack = new List<PhysChemState>();

    public virtual void Update()
    {
        activeState = GetCurrentState();
        if(activeState != null){
            activeState.RunState();
        }

    }

    public void PopState(PhysChemState state){
        stateStack.Remove(state);
    }

    public void PushState(PhysChemState state){
        stateStack.Insert(0, state);
        stateStack[0].Start();
    }

    public PhysChemState GetCurrentState(){
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
}
