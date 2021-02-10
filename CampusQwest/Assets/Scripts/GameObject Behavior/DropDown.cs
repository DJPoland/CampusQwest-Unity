using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDown : MonoBehaviour
{
    Dropdown dd;
    public Text tx;

    // Start is called before the first frame update
    void Start()
    {
        // Get attached component
        dd = GetComponent<Dropdown>();

        // Add listener
        dd.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dd);
        });
    }

    // This would execute when the selection is changed
    void DropdownValueChanged(Dropdown change)
    {
        tx.text = "THIS IS THE " + change.captionText.text + " LIST";
    }
}
