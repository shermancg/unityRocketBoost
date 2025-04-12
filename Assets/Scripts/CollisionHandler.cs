using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;
    AudioSource audioSource;

    bool isControllable = true;
    bool collisionEnabled = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();    
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextlevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionEnabled = !collisionEnabled;
            Debug.Log("Collision is " + collisionEnabled);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isControllable || !collisionEnabled)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                Debug.Log("Level complete");
                StartSuccessSequence();
                Invoke("LoadNextlevel", levelLoadDelay);
                break;
            default:
                Debug.Log("Dead");
                StartCrashSequence();
                Invoke("ReloadLevel", levelLoadDelay);
                break;
        }
    }
    
    void StartSuccessSequence()
    {
        GetComponent<Movement>().enabled = false;
        GetComponent<AudioSource>().Stop();
        GetComponent<Rigidbody>().isKinematic = true;
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
    }

    void StartCrashSequence()
    {
        isControllable = false;
        GetComponent<Movement>().enabled = false;
        GetComponent<AudioSource>().Stop();
        //GetComponent<Rigidbody>().isKinematic = true;
        audioSource.PlayOneShot(crashSound);
        crashParticles.Play();
        
    }

    void LoadNextlevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }    
            SceneManager.LoadScene(nextSceneIndex);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

}
