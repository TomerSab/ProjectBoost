using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField]float rotatBoost = 100f;
    [SerializeField]float mainBoost = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip WiningSound;


    [SerializeField] ParticleSystem enginePartical1;
    [SerializeField] ParticleSystem enginePartical2;
    [SerializeField] ParticleSystem deathPrtical;
    [SerializeField] ParticleSystem winingPartical;

    bool isTransitioning = false;
    Rigidbody rigidBody;
    AudioSource rocketSounds;

    bool colisFlag = true;
    // Start is called before the first frame update
    void Start()
    {

        rigidBody = GetComponent<Rigidbody>();
        rocketSounds = GetComponent<AudioSource>();
           }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
        {
            RespondToBoostInput();
            RespondToRotateInput();
            
        }
        //Debug on off
        if (Debug.isDebugBuild)
        {
            DebugAndTestingOptions();
        }
    }

    private void DebugAndTestingOptions()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("L Pressed - Next Level");
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            colisFlag = !colisFlag;
          
            Debug.Log("C Pressed - Avoid Colisions");
              
        }
    }

    private void RespondToBoostInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyBoost();
        }
        else
        {
            rocketSounds.Stop();
            enginePartical1.Stop();
            enginePartical2.Stop();
        }

        }

    private void ApplyBoost()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainBoost);
        if (!rocketSounds.isPlaying)
        {
            StopApplyBoost();
        }


    }

    private void StopApplyBoost()
    {
        rocketSounds.PlayOneShot(mainEngine);
        enginePartical1.Play();
        enginePartical2.Play();
    }

    private void RespondToRotateInput()
    {
        rigidBody.angularVelocity = Vector3.zero; // Take  Manual Control

        
        float rotationsThisFrame = rotatBoost * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward* rotationsThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {

            transform.Rotate(-Vector3.forward* rotationsThisFrame);
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || !colisFlag) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartWiningSeq();
                break;
            default:
                StartDyingSeq();
                break;
        }
    }

    private void StartWiningSeq()
    {
        isTransitioning = true;
        rocketSounds.Stop();
        rocketSounds.PlayOneShot(WiningSound);
        print("Won Level");
        winingPartical.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDyingSeq()
    {

        isTransitioning = true;
        rocketSounds.Stop();
        rocketSounds.PlayOneShot(deathSound);
        print("hit Something");
        deathPrtical.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }
    
    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (currentSceneIndex == SceneManager.sceneCountInBuildSettings-1) {
            nextSceneIndex = 0;
        }
            SceneManager.LoadScene(nextSceneIndex);
    }

    private void LoadFirstLevel()
    {

        SceneManager.LoadScene(0);
    }
}
