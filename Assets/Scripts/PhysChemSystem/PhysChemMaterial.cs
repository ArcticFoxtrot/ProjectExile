using System;
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

    public float temperature = 0f;
    public float burnTempThreshold = 100f;
    public float freezeTempThreshold = -10f;
    public float statusTime = 10f;

    public float statusStartTime;
    public bool statusTimerStarted = false;

    [Header("Damage from statuses")]
    public float electricDamage = 10f;
    public float burnDamage = 10f;

    public void Setup(PhysChemMaterialType type)
    {
        if(type == PhysChemMaterialType.Wood){
            canConduct = false;
            canBurn = true;
        } else if(type == PhysChemMaterialType.Metal){
            canBurn = false;
            canConduct = true;
        }
    }

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

        if(GetStateStackCount() <= 0){
            OnNoState();
        }
    }

    private void OnNoState()
    {
        PushState(new StateDefault(this));
    }

    //make sure OnSomething methods are called only once, as they will push their states to the stack
    private void OnBurning(){
        PushState(new StateBurning(this));
    }

    private void OnFreezing(){
        PushState(new StateFreezing(this));
    }

    private void OnConductingElectricity(){
        PushState(new StateConductingElectricity(this));        
    }

    private void OnCollisionEnter(Collision other) {
        PhysChemMaterial otherPhysChemMaterial;
        other.gameObject.TryGetComponent<PhysChemMaterial>(out otherPhysChemMaterial);
        if(otherPhysChemMaterial){
            if(otherPhysChemMaterial.isBurning && this.canBurn){
                this.isBurning = true;
                this.OnBurning();

            }

            if(otherPhysChemMaterial.isConducting && this.canConduct){
                this.isConducting = true;
                this.OnConductingElectricity();
            }

            if(otherPhysChemMaterial.isFreezing && this.canFreeze){
                if(this.isBurning == false){
                    this.isFreezing = true;
                    this.OnFreezing();
                }
                //needs two hits from freezing thing in order to be frozen, first hit only sets to default
                if(this.isBurning == true){
                    this.isBurning = false;
                    OnNoState(); //first go to default
                    //TODO transitions between states?
                }
                
            }
        }
    }

    public void HandleTemperatureChange(float incomingTemp){
        temperature = temperature + incomingTemp;
        if(temperature >= burnTempThreshold){
            isBurning = true;
            OnBurning();
        } if(temperature <= freezeTempThreshold){
            isFreezing = true;
            OnFreezing();
        } else {
            isBurning = false;
            isFreezing = false;
            OnNoState();
        }
    }

    public float GetCurrentTemperature(){
        return temperature;
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }

    public void ResetStatusTimer(){
        statusStartTime = 0f;
        statusTimerStarted = false;
    }
}
