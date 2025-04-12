using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustPower = 10f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] AudioClip thrustSound;
    [SerializeField] ParticleSystem leftThrustParticles;
    [SerializeField] ParticleSystem rightThrustParticles;
    [SerializeField] ParticleSystem mainThrustParticles;
    Rigidbody rb;
    AudioSource audioSource;
        
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            InitiateThrust();
        }
        else
        {
            StopThrustSFXVFX();
        }
    }

    private void InitiateThrust()
    {
        rb.AddRelativeForce(Vector3.up * thrustPower * Time.fixedDeltaTime);
        Debug.Log("Thrusting");

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustSound);
        }

        if (!mainThrustParticles.isPlaying)
        {
            mainThrustParticles.Play();
        }
    }

    private void StopThrustSFXVFX()
    {
        audioSource.Stop();
        mainThrustParticles.Stop();
    }

    private void ProcessRotation()
    {
       float rotationInput = rotation.ReadValue<float>();
       
       if (rotationInput < 0) //this rotates the ship left
        {
            RotateLeft();
        }
        else if (rotationInput > 0) //this rotates the ship right
        {
            RotateRight();
        }
        else
        {
            StopSideThrusterParticles();
        }
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationSpeed);

        if (!rightThrustParticles.isPlaying)
        {
            leftThrustParticles.Stop();
            rightThrustParticles.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationSpeed);

        if (!leftThrustParticles.isPlaying)
        {
            rightThrustParticles.Stop();
            leftThrustParticles.Play();
        }
    }

    private void StopSideThrusterParticles()
    {
        rightThrustParticles.Stop();
        leftThrustParticles.Stop();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
    }
}
