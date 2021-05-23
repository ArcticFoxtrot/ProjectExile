using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUIHandler : MonoBehaviour
{

    public static InteractionUIHandler GetInteractionUIHandler(){
        GameObject handlerObject = GameObject.FindWithTag("Interaction Canvas");
        return handlerObject.GetComponent<InteractionUIHandler>();
    }

}
