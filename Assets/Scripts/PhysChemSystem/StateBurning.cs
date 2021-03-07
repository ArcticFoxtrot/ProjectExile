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

        Debug.Log("This was called by the stateburning class from object " + physChemMaterial.gameObject.name);
        //start timer for burnTime
        if(!physChemMaterial.canBurnInfinitely){
            if(!physChemMaterial.burnTimerStarted){
                physChemMaterial.burnStartTime = Time.time;
                physChemMaterial.burnTimerStarted = true;
            }
            if(physChemMaterial.burnTimerStarted){
                if(Time.time - physChemMaterial.burnStartTime > physChemMaterial.burnTime){
                    Debug.Log("Object " + physChemMaterial.name + " has burnt completely!");
                    physChemMaterial.DestroySelf();
                }
            }
        }
    }

}
