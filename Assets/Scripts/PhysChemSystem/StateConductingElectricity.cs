using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateConductingElectricity : PhysChemState
{

    public StateConductingElectricity(PhysChemMaterial physChemMaterial) : base(physChemMaterial){

    }

    public override void RunState()
    {
        base.RunState();
        //start timer for burnTime
        if(!physChemMaterial.canConductInfinitely){
            if(!physChemMaterial.statusTimerStarted){
                physChemMaterial.statusStartTime = Time.time;
                physChemMaterial.statusTimerStarted = true;
            }
            if(physChemMaterial.statusTimerStarted){
                if(Time.time - physChemMaterial.statusStartTime > physChemMaterial.statusTime){
                    Debug.Log("Object " + physChemMaterial.name + " has stopped conducting!");
                    EndState();
                    
                }
            }
        }
    }

    public override void Start() {
        physChemMaterial.PopState(physChemMaterial.GetCurrentState());
        base.Start();
    }

    public override void EndState()
    {
        physChemMaterial.isConducting = false;
        physChemMaterial.ResetStatusTimer();
        base.EndState();
    }
}
