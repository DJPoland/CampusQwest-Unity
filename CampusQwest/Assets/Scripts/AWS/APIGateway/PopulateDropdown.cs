using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class PopulateDropdown : MonoBehaviour
{
    private readonly List<string> _mDropOptions = new List<string>();
    private readonly List<string> _indexToClue = new List<string>();
    private List<Qwest> _qwests;
    private HashSet<long> _completedQwests;
    private User _user;
    private bool _updatedHome = false;


    public Dropdown mDropdown;
    public Text clueText;

    void Start()
    {
        mDropdown.onValueChanged.AddListener(delegate
        {
            UpdateScrollText(mDropdown);
        });
    }

    void Update()
    {
        if (_user == null)
        {
            _user = APIGatewayController.GETUserData();
        }
        else if (_qwests == null)
        {
            _qwests = APIGatewayController.GETUserQwests();
        }
        else if (!_updatedHome)
        {
            _completedQwests = new HashSet<long>();
            foreach (QwestsCompleted completed in _user.QwestsCompleted)
            {
                _completedQwests.Add(completed.QwestId);
            }

            CreateDropdown();
            clueText.text = _indexToClue[0];
            _updatedHome = true;
        }
        
    }

    // Create list to select Qwests from and remove Qwests a User has already finished
    private void CreateDropdown()
    {
        for (int i = 0; i < _qwests.Count; i++)
        {
            Debug.Log(_qwests[i].Name);
            if (!_completedQwests.Contains(_qwests[i].Id))
            {
                _mDropOptions.Add(_qwests[i].Name);
                _indexToClue.Add(_qwests[i].Locations[0].Clue);
            }
        }
        mDropdown.ClearOptions();
        mDropdown.AddOptions(_mDropOptions);
    }

    private void UpdateScrollText(Dropdown change)
    {
        clueText.text = _indexToClue[change.value];
    }
}
