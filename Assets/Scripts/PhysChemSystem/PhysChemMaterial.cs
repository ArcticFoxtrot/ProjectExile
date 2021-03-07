using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysChemMaterial : PhysChemFSM
{

    public bool isBurning;
    public bool isFreezing;
    public bool isConducting;


    public bool canBurn;
    public bool canBurnInfinitely;
    public bool canConduct;
    public bool canConductInfinitely;
    public bool canFreeze;

    public float burnTime = 10f;

    public float burnStartTime;
    public bool burnTimerStarted = false;

    private void Start() {
        if(isBurning){
            OnBurning();
        }

        if(isFreezing){
            OnFreezing();
        }

        if(isConducting){
            OnConductingElectricity();
        }
    }

    private void OnBurning(){
        SetState(new StateBurning(this));   
    }

    private void OnFreezing(){
        SetState(new StateFreezing(this));
    }

    private void OnConductingElectricity(){
        SetState(new StateConductingElectricity(this));        
    }

    private void OnCollisionEnter(Collision other) {
        PhysChemMaterial otherPhysChemMaterial;
        other.gameObject.TryGetComponent<PhysChemMaterial>(out otherPhysChemMaterial);
        if(otherPhysChemMaterial){
            if(otherPhysChemMaterial.isBurning && this.canBurn){
                this.isBurning = true;
                this.OnBurning();
            }
        }
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }
}
