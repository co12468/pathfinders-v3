using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    public float startSpeed = 40f;

    private Transform _arrow;
    private bool _ballMoving;
    private Transform _startPosition;
    private List<GameObject> _pins = new();
    private readonly Dictionary<GameObject, Transform> _pinsDefaultTransform = new();
    public Pin[] pins; // Reference to all pins in the scene
    private int round;
    private UIManager uiManager;

    public int Point { get; set; }

    [SerializeField]
    GameObject ballPrefab;
    [SerializeField]
    Transform ballSpawnLocation;

    private TMP_Text feedBack;

    // Start is called before the first frame update
    void Start()
    {
        // _arrow = GameObject.FindGameObjectWithTag("Arrow").transform;

        rb = GetComponent<Rigidbody>();

        _startPosition = transform;
        _pins = GameObject.FindGameObjectsWithTag("Pin").ToList();

        foreach (var pin in _pins)
        {
            _pinsDefaultTransform.Add(pin, pin.transform);
        }
        round = 0;
        uiManager = FindObjectOfType<UIManager>(); // Reference to the UIManager

        // feedBack = GameObject.FindGameObjectWithTag("FeedBack").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_ballMoving)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        // cameraAnim.SetTrigger("Go");
        // cameraAnim.SetFloat("CameraSpeed", _arrow.transform.localScale.z);
        _ballMoving = true;
        _arrow.gameObject.SetActive(false);
        rb.isKinematic = false;

        Vector3 forceVector = _arrow.right * -1 * (startSpeed * _arrow.transform.localScale.z);
        Vector3 forcePosition = transform.position + (transform.right * 0.5f);
        rb.AddForceAtPosition(forceVector, forcePosition, ForceMode.Impulse);

        yield return new WaitForSecondsRealtime(7);

        _ballMoving = false;
        _arrow.gameObject.SetActive(true);
        // GenerateFeedBack();

        yield return new WaitForSecondsRealtime(2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lane"))
        {
            Debug.Log("Round started");
        }
        if (other.CompareTag("Gutter"))
        {
            round++;
            Debug.Log("In the gutter!");
            StartCoroutine(ResetAfterDelay());
            // spawnBall();
            ResetBall();
        }
    }

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(5f); // Adjust the delay time
        int fallenPinsCount = CountFallenPins(); // Count the number of fallen pins
        
        // Check if the ball has hit any pins
        bool ballHitPins = false;
        foreach (Pin pin in pins)
        {
            if (pin.PinHasFallen())
            {
                ballHitPins = true;
                break;
            }
        }

        if (!ballHitPins)
        {
            // If the ball didn't hit any pins, update the score to zero
            UpdateScore(0);
            Debug.Log("Gutter ball");
        }
        else
        {
            // If pins have fallen, update the score based on the number of fallen pins
            UpdateScore(fallenPinsCount);
            Debug.Log("Fallen pins: " + fallenPinsCount);
        }

        ResetPins();
    }
    
    private void ResetPins()
    {
        if (round % 2 == 0)
        {
            foreach (Pin pin in pins)
            {
                pin.ResetPin();
                pin.gameObject.SetActive(true);
                Debug.Log("Reset pins!");
            }
        } else
        {
            Debug.Log("Despawn fallen pins.");
            foreach (Pin pin in pins)
            {
                if (pin.PinHasFallen())
                {
                    pin.gameObject.SetActive(false);
                }
            }
        }
    }
    
    private void ResetBall()
    {
        // Logic to reset the ball to its initial position
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = ballSpawnLocation.position; // Set the ball position
        Debug.Log("Reset ball!");
    }

    public void spawnBall()
    {
        GameObject.Instantiate<GameObject>(ballPrefab,
            ballSpawnLocation.position, Quaternion.identity);
    }

    // Method to update the score
    private void UpdateScore(int score)
    {
        uiManager.UpdateFrameScores(score);
        Debug.Log("Updated Score: " + score);
    }
    private int CountFallenPins()
    {
        int fallenPins = 0;
        foreach (Pin pin in pins)
        {
            if (pin.PinHasFallen())
            {
                fallenPins++;
            }
        }
        return fallenPins;
    }

    /*
    private void GenerateFeedBack()
    {
        feedBack.text = Point switch
        {
            0 => "Nothing!",
            > 0 and < 3 => "You are learning Now!",
            >= 3 and < 6 => "It was close!",
            >= 6 and < 10 => "It was nice!",
            _ => "Perfect! You are a master!"
        };

        feedBack.GetComponent<Animator>().SetTrigger("Show");
    }
    */
}
