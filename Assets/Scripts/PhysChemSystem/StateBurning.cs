using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBurning : PhysChemState
{
    public StateBurning(PhysChemMaterial physChemMaterial) : base(physChemMaterial){

    }

    public override void RunState()
    {
        base.RunState();
        //start timer for burnTime
        if(!physChemMaterial.canBurnInfinitely){
            if(!physChemMaterial.statusTimerStarted){
                physChemMaterial.statusStartTime = Time.time;
                physChemMaterial.statusTimerStarted = true;
            }
            if(physChemMaterial.statusTimerStarted){
                if(Time.time - physChemMaterial.statusStartTime > physChemMaterial.statusTime){
                    Debug.Log("Object " + physChemMaterial.name + " has burnt completely!");
                    physChemMaterial.DestroySelf();
                }
            }
        }
    }

    public override void Start() {
        physChemMaterial.PopState(physChemMaterial.GetCurrentState());
        base.Start();
    }

}
