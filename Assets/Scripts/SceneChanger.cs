using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void CardMemoryGame()
    {
        SceneManager.LoadScene("CardMemory");
    }
    public void ReturnMain()
    {
        SceneManager.LoadScene("Main");
    }
}
