using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysChemState
{

    //protected readonly PhysChemFSM physChemFSM;
    protected readonly PhysChemMaterial physChemMaterial;

    public PhysChemState(PhysChemMaterial _physChemMaterial){
        physChemMaterial = _physChemMaterial;
    }    


    public virtual void Start(){

    }

    public virtual void RunState(){

    }

    public virtual void EndState(){
        physChemMaterial.SetState(this);
    }

}
