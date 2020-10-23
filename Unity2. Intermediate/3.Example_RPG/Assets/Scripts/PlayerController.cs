using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    
    void Start()
    {
        GameManager.Input.KeyAction -= OnKeyboard;
        GameManager.Input.KeyAction += OnKeyboard;
    }

    void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * (Time.deltaTime * _speed));
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * (Time.deltaTime * _speed));
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * (Time.deltaTime * _speed));
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * (Time.deltaTime * _speed));
    }
}
