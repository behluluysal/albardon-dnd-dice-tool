using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DiceAudio : MonoBehaviour
{
    public AudioClip collisionWithWallSound;

    public AudioClip collisionWithDiceSound;

    public AudioClip diceStopSound;

    private AudioSource audioSource;

    private Rigidbody rb;

    private bool hasStopped;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check if the dice has stopped and hasn't already played the stop sound
        if (!hasStopped && rb.IsSleeping())
        {
            audioSource.PlayOneShot(diceStopSound);
            hasStopped = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Play sound when dice collides with the wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            audioSource.PlayOneShot(collisionWithWallSound);
        }

        // Play sound when dice collides with another dice
        else if (collision.gameObject.CompareTag("Dice"))
        {
            audioSource.PlayOneShot(collisionWithDiceSound);
        }
    }
}