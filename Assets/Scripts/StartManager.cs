using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
   public void Task1()
    {
        SceneManager.LoadScene(1);
    }

    public void Task2()
    {
        SceneManager.LoadScene(2);
    }
}
