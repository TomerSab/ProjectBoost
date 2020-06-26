using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField]float rotatBoost = 100f;
    [SerializeField]float mainBoost = 100f;
    Rigidbody rigidBody;
    AudioSource boostAudio;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        boostAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        Booosting();
        Rotate();
    }

    private void Booosting()
    {
        if (Input.GetKey(KeyCode.Space))
        {

            rigidBody.AddRelativeForce(Vector3.up * mainBoost);
            if (!boostAudio.isPlaying)
            {
                boostAudio.Play();
            }
        }
        else boostAudio.Stop();
    }
    private void Rotate()
    {
        rigidBody.freezeRotation = true; // Take  Manual Control

        
        float rotationsThisFrame = rotatBoost * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward* rotationsThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {

            transform.Rotate(-Vector3.forward* rotationsThisFrame);
        }

        rigidBody.freezeRotation = false; // Take  Manual Control
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                print("Hit Finish");//TODO Remove
                SceneManager.LoadScene(1);
                break;
            default:
                print("Dead");
                SceneManager.LoadScene(0);
                break;
        }
    }
}
