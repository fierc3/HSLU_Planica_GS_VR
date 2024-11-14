using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class BalloonTransporter : MonoBehaviour
{
    bool qPressed = false;
    bool qTriggered = false;
    bool isGoing = false;

    // Teleporting
    Globals globals;

    public string sceneName = "InspectScene";
    [SerializeField]
    private int active = 0;

    [SerializeField]
    private List<LocationInformation> locations;

    // Portal attributes
    [SerializeField]
    MeshRenderer portal;
    [SerializeField]
    private float startValue = 20.0f;
    [SerializeField]
    private float endValue = 0.9f;
    [SerializeField]
    private float duration = 2.0f;
    
    // Movement attributes
    [SerializeField]
    private float moveDuration = 12.0f;
    [SerializeField]
    private SplineAnimate splineAnimate;

    // Screen
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private Image image;

    // Hot Air Balloon
    [SerializeField]
    private GameObject hotAirBalloon;

    [SerializeField]
    private CanvasGroup travelIndicator;

    // Sounds
    [SerializeField]
    private AudioClip changeDestinationAudioClip;
    [SerializeField]
    private AudioClip confirmAudioClip;
    [SerializeField]
    private AudioClip moveSound;
    [SerializeField]
    private AudioClip directionChangeSound;
    [SerializeField]
    private AudioClip teleportSound;

    private Vector3 lastPosition;
    private bool hasMoved = false;


    // Start is called before the first frame update
    void Start()
    {
        this.globals = GameObject.FindGameObjectWithTag("Globals").GetComponent<Globals>();
        var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        locations = locations.Where(x => !x.id.Equals(activeScene.name)).ToList();
        sceneName = locations[active].id;
        UpdateScreen();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasMoved && transform.position != lastPosition)
        {
            hasMoved = true;
            SoundManager.Instance.PlaySound(moveSound, transform.parent, 1.5f);
            StartCoroutine(PlaySoundEvery15Seconds());
            lastPosition = transform.position;
        }

        if (isGoing) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Previous();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Next();
        }


        if (Input.GetKeyDown(KeyCode.Q)) // Detect right arrow key press
        {
            qPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.Q)) // Detect right arrow key release
        {
            qPressed = false;
            qTriggered = false;
        }

        if (qPressed && !qTriggered)
        {
            qTriggered = true;
            GoToScene();
        }
    }

    public void Next()
    {
        if (isGoing) return;

        Debug.Log("Next: " + active);
        active = (active + 1) % locations.Count();
        sceneName = locations[active].id;
        SoundManager.Instance.PlaySound(changeDestinationAudioClip, transform, 0.7f);
        UpdateScreen();
    }

    public void Previous()
    {
        if(isGoing) return;

        Debug.Log("Previous: " + active);
        active = (active - 1 + locations.Count) % locations.Count;
        sceneName = locations[active].id;
        SoundManager.Instance.PlaySound(changeDestinationAudioClip, transform, 0.7f);
        UpdateScreen();
    }

    private void UpdateScreen()
    {
        title.SetText(locations[active].text);
        description.SetText(locations[active].additionalText);
        image.sprite = locations[active].image;
    }

    public void GoToScene()
    {
        if (isGoing || Time.timeScale < 1f)
        {
            Debug.Log("Not triggering transition again because were already on the way, isgoing: " + isGoing + " timescale: " + Time.timeScale);
            return;
        }

        isGoing = true;
        travelIndicator.gameObject.SetActive(true);
        SoundManager.Instance.PlaySound(confirmAudioClip, transform, 0.7f);
        SoundManager.Instance.PlaySound(directionChangeSound, transform.parent, 1.2f);

        if (this.splineAnimate == null)
        {
            GoToLocation();
        }

        this.splineAnimate.Pause();
        StartCoroutine(ShowPortal());
        StartCoroutine(MoveToTarget());
        portal.gameObject.GetComponent<AudioSource>().Play(); 
    }

    private IEnumerator ShowPortal()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            portal.material.SetFloat("_Brightness", currentValue);
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        portal.material.SetFloat("_Brightness", endValue);
    }

    private IEnumerator MoveToTarget()
    {
        Vector3 startPosition = hotAirBalloon.transform.position;
        Vector3 endPosition = portal.gameObject.transform.position;
        float timeElapsed = 0f;

        while (timeElapsed < moveDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / moveDuration;

            Vector3 curvePosition = (1 - t) * (1 - t) * startPosition + 2 * (1 - t) * t * portal.gameObject.transform.position + t * t * endPosition;
            hotAirBalloon.transform.position = curvePosition;

            Vector3 direction = (endPosition - hotAirBalloon.transform.position).normalized;
            direction.y = 0; // Keep the y component zero to avoid tilting up or down
            Quaternion lookRotation = Quaternion.LookRotation(direction);
           // transform.rotation = Quaternion.Slerp(hotAirBalloon.transform.rotation, lookRotation, Time.deltaTime * (1 / duration));

            if(Vector3.Distance(hotAirBalloon.transform.position, endPosition) < 5)
            {
                SoundManager.Instance.PlaySound(teleportSound, transform.parent, 1.5f);
                GoToLocation();
                yield break;
            }

            yield return null;
        }

        // Ensure the object reaches the exact target position at the end
        hotAirBalloon.transform.position = endPosition;
    }

    private void GoToLocation()
    {
        EventManager.HideComplete += OnHidden;
        globals.Mode = Mode.NonInteractive;
        EventManager.FireHideEvent();
    }

    private void OnHidden()
    {
        EventManager.HideComplete -= OnHidden;
        Debug.Log("OnHidden for " + sceneName);
        EventManager.FireSwitchSceneEvent(sceneName);
    }

    private IEnumerator PlaySoundEvery15Seconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            SoundManager.Instance.PlaySound(moveSound, transform.parent, 1.5f);
        }
    }
}