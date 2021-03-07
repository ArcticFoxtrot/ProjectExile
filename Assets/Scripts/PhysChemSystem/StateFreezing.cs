using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFreezing : PhysChemState
{

    public StateFreezing(PhysChemMaterial physChemMaterial) : base(physChemMaterial){

    }

    public override void RunState()
    {
        base.RunState();
        base.physChemMaterial.SetState(new StateFreezing(physChemMaterial));
        Debug.Log("This was called by the statefreezing class!");
    }

}
