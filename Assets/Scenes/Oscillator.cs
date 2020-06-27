using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    
    [SerializeField] Vector3 movmentVector;
    [SerializeField] float period = 2f;

    //TODO Remove
    float movmentFactor;

    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; } // protect against Zero Diveiding
            float cycles = Time.time / period; // Grows Continually from 0

            const float tau = Mathf.PI * 2; // 6.28
            float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to +1

            print(rawSinWave);
            movmentFactor = rawSinWave / 2f + 0.5f;
            Vector3 offset = movmentVector * movmentFactor;
            transform.position = startingPos + offset;
        
    }
}
