using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ChangeScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("R"))
        {
            Debug.Log("R pressed");
            StartCoroutine(SetScene());
        }
    }

    private IEnumerator SetScene()
    {
        DontDestroyOnLoad(this.gameObject);
        yield return SceneManager.LoadSceneAsync(1);
    }
}
