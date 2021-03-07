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
        base.physChemMaterial.SetState(new StateConductingElectricity(physChemMaterial));
        Debug.Log("This was called by the StateConductingElectricity class!");
    }

}
