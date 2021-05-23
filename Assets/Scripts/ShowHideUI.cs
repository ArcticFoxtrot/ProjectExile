using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideUI : MonoBehaviour
{

    [SerializeField] KeyCode toggleKey;
    [SerializeField] GameObject showHideObject;

    private bool showHide = true;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(toggleKey)){
            showHide = !showHide;
            SetUIActive(showHide);
        } 
    }

    public bool GetShowHide(){
        return showHide;
    }

    public void SetUIActive(bool t){
        showHideObject.SetActive(t);
    }
}
