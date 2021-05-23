using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupUI : MonoBehaviour
{

    [SerializeField] GameObject inventoryPickupUI;

    [SerializeField] float UiElemetOffset;

    private GameObject uiElement;
    Camera mainCamera;

    bool isDestroyed = false;
    public bool pickupUIIsDestroyed{
        get {return isDestroyed;}
        set {isDestroyed = value;}
        
    }

    bool uiElementIsDestroyed = false;
    public bool uiElementDestroyed{
        get {return uiElementIsDestroyed;}
    }

    public void Setup() {
        //instantiate from prefab
        uiElement = Instantiate(inventoryPickupUI, Vector3.zero, Quaternion.identity, InteractionUIHandler.GetInteractionUIHandler().transform);
        mainCamera = Camera.main;
        HandleUIElementText();
    }

    private void Update() {
        if(isDestroyed == false){
            HandleUIElementLocation();
        }
        
    }

    public void SetUIElementActive(bool t){
        uiElement.SetActive(t);
    }
        
    private void HandleUIElementLocation(){
        uiElement.transform.position =  mainCamera.WorldToScreenPoint(this.transform.position) + new Vector3(0f, UiElemetOffset, 0);
    }

    private void HandleUIElementText()
    {
        if(TryGetComponent<Pickup>(out Pickup pickup)){
            uiElement.GetComponentInChildren<TMP_Text>().text = pickup.GetItem().GetDisplayName();
        }
    }

    public GameObject GetPickupUIElement(){
        return uiElement;
    }

    public void DestroyUIElement()
    {
        uiElementIsDestroyed = true;
        Destroy(uiElement);
    }

}
