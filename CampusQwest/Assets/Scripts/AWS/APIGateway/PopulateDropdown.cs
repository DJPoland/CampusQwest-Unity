using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


public class PopulateDropdown : MonoBehaviour
{
    private readonly List<string> _mDropOptions = new List<string>();
    private readonly List<Qwest> _availableQwests = new List<Qwest>();
    private List<Qwest> _qwests;
    private HashSet<long> _completedQwests;
    private User _user;
    private bool _updatedHome = false;

    public Dropdown mDropdown;
    public Text popUpText;
    public Button SubmitButton;
    public GameObject mPopUp;

    void Start()
    {
        mDropdown.value = -1;
        mDropdown.onValueChanged.AddListener(delegate
        {
            Debug.Log("clicked!");
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
            // Hacky way of checking if currentQwest is not null or not {}
            if (!string.IsNullOrEmpty(_user.CurrentQwest.TimeStarted))
            {
                _updatedHome = true;
                gameObject.SetActive(false);                
            }
            else
            {
                _completedQwests = new HashSet<long>();
                foreach (QwestsCompleted completed in _user.QwestsCompleted)
                {
                    _completedQwests.Add(completed.QwestId);
                }

                CreateDropdown();
                _updatedHome = true;
            }
        }
        
    }

    // Create list to select Qwests from and remove Qwests a User has already finished
    private void CreateDropdown()
    {
        foreach (var qwest in _qwests)
        {
            Debug.Log(qwest.Name);
            if (_completedQwests.Contains(qwest.Id)) continue;
            
            _mDropOptions.Add(qwest.Name);
            _availableQwests.Add(qwest);
        }
        mDropdown.AddOptions(_mDropOptions);
    }

    private void LockSelecting()
    {
        
    }

    private void UpdateScrollText(Dropdown change)
    {
        Debug.Log("Executed!");
        popUpText.text = "Do you wish to start this Qwest? \n\n" + _mDropOptions[change.value] + "\n\n You need to finish or abandon this Qwest to start a new one";
        SubmitButton.onClick.AddListener(() => StartCoroutine(QwestProcess.PostStartQwest("/user/qwests/startQwest", (int) _availableQwests[change.value].Id)));
        mPopUp.SetActive(true);
    }
}
