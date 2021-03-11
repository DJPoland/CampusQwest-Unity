using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class PopulateDropdown : MonoBehaviour
{
    private List<string> m_DropOptions = new List<string>();
    private List<string> indexToClue = new List<string>();
    private List<Qwest> qwests;
    private bool UpdatedHome = false;


    public Dropdown m_Dropdown;
    public Text clueText;

    void Start()
    {
        m_Dropdown.onValueChanged.AddListener(delegate
        {
            UpdateScrollText(m_Dropdown);
        });
    }

    void Update()
    {
        if (qwests == null)
        {
            qwests = APIGatewayController.GETUserQwests();
        }
        else if (!UpdatedHome)
        {
            CreateDropdown();
            clueText.text = indexToClue[0];
            UpdatedHome = true;
        }
    }

    private void CreateDropdown()
    {
        for (int i = 0; i < qwests.Count; i++)
        {
            m_DropOptions.Add(qwests[i].Name);
            indexToClue.Add(qwests[i].Locations[i].Clue);
        }
        m_Dropdown.ClearOptions();
        m_Dropdown.AddOptions(m_DropOptions);
    }

    private void UpdateScrollText(Dropdown change)
    {
        clueText.text = indexToClue[change.value];
    }
}
