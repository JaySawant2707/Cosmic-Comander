using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] GameObject optionsMenu;

    public void Options()
    {
        optionsMenu.SetActive(true);
    
    }

    public void BackToMM()
    {
        optionsMenu.SetActive(false);
       
    }
}
