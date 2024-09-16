using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [Header("General Setup Settings")]
    [Tooltip("how fast the ship moves up and down")][SerializeField] float controlSpeed = 30f;
    [Tooltip("How far the player moves horizontally")][SerializeField] float xRange = 10f;
    [Tooltip("How far the player moves vertically")][SerializeField] float yRange = 7f;


    [Header("Laser Gun Array")]
    [Tooltip("Add all player lasers here")][SerializeField] GameObject[] lasers;


    [Header("Player Position based Tuning")]
    [SerializeField] float positionpitchFactor = -2f;
    [SerializeField] float positionYawFactor = 2f;


    [Header("Player Input based Tuning")]
    [SerializeField] float controlpitchFactor = -15f;
    [SerializeField] float controlrollFactor = -20f;
 

    float xThrow, yThrow;

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();

        void ProcessTranslation()
        {
            
            xThrow = Input.GetAxis("Horizontal");
            yThrow = Input.GetAxis("Vertical");

            float xOffset = xThrow * Time.deltaTime * controlSpeed; 
            float rawXpos = transform.localPosition.x + xOffset;
            float clampedXpos = Mathf.Clamp(rawXpos, -xRange, xRange);

            float yOffset = yThrow * Time.deltaTime * controlSpeed; 
            float rawYpos = transform.localPosition.y + yOffset;
            float clampedYpos = Mathf.Clamp(rawYpos, -yRange, yRange);
            
            transform.localPosition = new Vector3 (clampedXpos, clampedYpos, transform.localPosition.z);

        }

        void ProcessRotation()
        {
            float pitchDueToPosition = transform.localPosition.y * positionpitchFactor;
            float pitchDueToControlThrow = yThrow * controlpitchFactor;

            
            float pitch = pitchDueToPosition + pitchDueToControlThrow;
            float yaw = transform.localPosition.x * positionYawFactor;
            float roll = xThrow * controlrollFactor;

            
            transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
        }

        void ProcessFiring()
        {
            if(Input.GetButton("Fire1"))
            {
                SetLasersActive(true);
            }
            else
            {
                SetLasersActive(false);
            }
        }

        void SetLasersActive(bool isActive)
        {
            foreach (GameObject laser in lasers)
            {
                var emissionModule = laser.GetComponent<ParticleSystem>().emission;
                emissionModule.enabled = isActive; 
            }
        }
        
    }
}
