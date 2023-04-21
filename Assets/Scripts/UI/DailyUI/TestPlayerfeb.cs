using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerfeb : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            setsomething();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            getsomething();
        }

    }
    public void setsomething() {
        PlayerPrefs.SetInt("test", 1);
    }
    public void getsomething() {
        Debug.Log(PlayerPrefs.GetInt("test", 0));
    }
}
