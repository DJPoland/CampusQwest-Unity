using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public GameObject PopupBox;
    public Text text;

    public string[] descriptions;

    public void enablePopupBox(int itemNumber)
    {
        PopupBox.SetActive(true);
        text.text = descriptions[itemNumber];
    }
}
