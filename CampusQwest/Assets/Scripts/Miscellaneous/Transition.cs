using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public void toScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
