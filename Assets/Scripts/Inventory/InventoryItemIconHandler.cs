using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

    /// <summary>
    /// To be put on the icon to show/hide item icons. Also updates the number icon.
    /// </summary>
[RequireComponent(typeof(Image))]
public class InventoryItemIconHandler : MonoBehaviour
{

    [SerializeField] GameObject numberTextContainer;
    [SerializeField] TextMeshProUGUI numberText;


    public void SetItem(InventoryItem item, int number){
        Image image = GetComponent<Image>();
        if(item == null){
            image.enabled = false;
        } else {
            image.enabled = true;
            image.sprite = item.GetIcon();
        }

        if(numberText){
            if(number <= 1){
                numberTextContainer.SetActive(true);
                numberText.text = number.ToString();
            }
        }
    }


}
