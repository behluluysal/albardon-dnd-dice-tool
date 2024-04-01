using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DiceRoller : MonoBehaviour
{
    public GameObject[] dicePrefabs;

    public TMP_Text countText, totalText, diceText;

    public Transform spawnPoint;

    private GameObject currentDice;

    private int diceCount = 0;

    private List<int> rollResults = new();

    private List<GameObject> rolledDice = new();

    public float WaitBeforeShrink = 1.5f;

    public float ShrinkDuration = 1.5f;

    private bool canRoll = true;

    private AudioSource audioSource;

    public AudioClip CriticalSuccess;

    public AudioClip CriticalFailure;

    private void OnEnable()
    {
        FaceDetector.OnDieStopped += UpdateRollResults;
    }

    private void OnDisable()
    {
        FaceDetector.OnDieStopped -= UpdateRollResults;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Detect which die to roll based on key press
        if (Input.GetKeyDown(KeyCode.Q)) SelectDice(0);
        if (Input.GetKeyDown(KeyCode.W)) SelectDice(1);
        if (Input.GetKeyDown(KeyCode.E)) SelectDice(2);
        if (Input.GetKeyDown(KeyCode.R)) SelectDice(3);
        if (Input.GetKeyDown(KeyCode.T)) SelectDice(4);

        // Detect number key press to set the count of dice
        for (KeyCode kc = KeyCode.Alpha1; kc <= KeyCode.Alpha5; kc++)
        {
            if (Input.GetKeyDown(kc))
            {
                diceCount = kc - KeyCode.Alpha0; // Get the numeric value
            }
        }

        // Roll the dice when space is pressed
        if (Input.GetKeyDown(KeyCode.Space) && currentDice != null && diceCount > 0 && canRoll)
        {
            countText.text = $"{diceCount}d20";
            diceText.text = "";
            totalText.text = "";
            RollDice();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveAllDice();
        }
    }

    private void SelectDice(int diceIndex)
    {
        if (currentDice != null)
        {
            Destroy(currentDice);
        }

        currentDice = Instantiate(dicePrefabs[diceIndex], spawnPoint.position, Quaternion.identity);
        currentDice.SetActive(false);
    }

    private void RollDice()
    {
        canRoll = false;
        for (int i = 0; i < diceCount; i++)
        {
            GameObject die = Instantiate(currentDice, spawnPoint.position + new Vector3(i, 0, 0), Quaternion.identity);
            die.SetActive(true);
            rolledDice.Add(die);
        }
    }

    private void UpdateRollResults(int result)
    {
        rollResults.Add(result);
        diceText.text = string.Join("+", rollResults);

        if (rollResults.Count == rolledDice.Count) // Check if all dice have stopped
        {
            int total = rollResults.Sum();
            totalText.text = "" + total;

            // Start coroutine to shrink dice after a delay
            StartCoroutine(ShrinkAndDestroyDice());

            // play critical sounds if any
            int max = 0;
            foreach (var dice in rollResults)
            {
                max = dice > max ? dice : max;
            }
            if (max >= 17)
            {
                audioSource.PlayOneShot(CriticalSuccess);
            }
            else if (max <= 5)
            {
                audioSource.PlayOneShot(CriticalFailure);
            }

            rollResults.Clear();
        }
    }

    private IEnumerator ShrinkAndDestroyDice()
    {
        yield return new WaitForSeconds(WaitBeforeShrink);

        foreach (GameObject die in rolledDice)
        {
            StartCoroutine(ShrinkDice(die));
        }
        rolledDice.Clear();
    }

    private IEnumerator ShrinkDice(GameObject die)
    {
        Vector3 originalScale = die.transform.localScale;
        float timer = 0;

        while (timer <= ShrinkDuration)
        {
            float scale = Mathf.Lerp(1, 0, timer / ShrinkDuration);
            die.transform.localScale = originalScale * scale;
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(die);
        canRoll = true; // actually need to check if the last die is destroyed however this works too so I will leave it like this for the sake of simplicity.
    }

    private void RemoveAllDice()
    {
        foreach (GameObject die in rolledDice)
        {
            Destroy(die);
        }

        rolledDice.Clear();

        rollResults.Clear();
        diceText.text = "";
        totalText.text = "0";

        canRoll = true;
    }
}