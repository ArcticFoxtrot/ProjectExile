using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DamageType {Electric, Fire, Ice}

public class Player : MonoBehaviour
{

    [SerializeField] float playerHealth = 100f;
    [SerializeField] float playerStamina = 100f;
    [SerializeField] float playerMana = 100f;

    [SerializeField] float damageProtectionTime = 1f;


    private float damageStartTime = 0f;
    private bool damageTimerStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.TryGetComponent<PhysChemMaterial>(out PhysChemMaterial mat)){
            if(mat.isConducting){
                HandleTakeDamage(mat.electricDamage, DamageType.Electric);
            }
            if(mat.isBurning){
                HandleTakeDamage(mat.burnDamage, DamageType.Fire);
            }
        }
    }

    private void HandleTakeDamage(float dmg, DamageType damageType)
    {
        if(damageTimerStarted == false){
            //if timer not currently active
            //set timer
            damageStartTime = Time.time;
            damageTimerStarted = true;
            //deal damage
            playerHealth -= dmg;
        } else if(damageTimerStarted = true && Time.time - damageStartTime > damageProtectionTime){
            //if timer is set, but the protection time has run out
            // reset timer
            damageTimerStarted = false;
            damageStartTime = 0f;
            //deal damage
            playerHealth -= dmg;

        } else {
            Debug.Log("Protected from damage!");
            //player is protected by damagetimer
            return;
        }
        
        //make sure you don't get damaged again too quickly?
        //add a timer to prevent from taking damage for a certain time
        if(playerHealth <= 0f){
            HandleDeath();
        }
    }

    private void HandleDeath(){
        Debug.Log("Player isded");
    }
}
