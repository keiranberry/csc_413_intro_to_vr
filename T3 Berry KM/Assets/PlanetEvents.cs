using TMPro;
using UnityEngine;

public class PlanetEvents : MonoBehaviour
{
    [SerializeField]
    private TMP_Text infoText;

    // target values
    private Vector3 growPos;
    private Vector3 shrinkPos;
    private float growSize;

    // grow or shrink
    private bool grow = false;

    // animation control
    [SerializeField]
    private float growTime = 1;

    private float startTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(infoText == null)
            infoText = GetComponentInChildren<TMP_Text>();

        if(infoText == null)
        {
            Debug.LogError("TextMeshPro not found in children or set"); 
            return;
        }

        // assuming scaling is uniform, save a copy of the target scale
        growSize = infoText.transform.localScale.x;

        // figure out what the world up is in local coordinates
        Vector3 worldUp = infoText.rectTransform.InverseTransformDirection(Vector3.up);

        // how far is the text from the center of the planet in local coordinates
        worldUp = worldUp * infoText.rectTransform.localPosition.y;

        // the target position is along worldUp at the same distance
        growPos = worldUp
            + new Vector3(infoText.rectTransform.localPosition.x,
            0,
            infoText.rectTransform.localPosition.z);

        // starting at the local origin, and no size at first
        shrinkPos = new Vector3(0, 0, 0);
        infoText.transform.localPosition = new Vector3(0, 0, 0);
        infoText.transform.localScale = new Vector3(0, 0, 0);

        // start at completely shrunk time
        startTime = Time.time - growTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(infoText == null)
        {
            return;
        }

        float timeElapsed = Time.time - startTime;
        float percentDone = timeElapsed / growTime;

        // if shrinking, flip the percentage
        if (!grow)
            percentDone = 1 - percentDone;

        // lerp both size and position
        infoText.transform.localPosition =
            Vector3.Lerp(shrinkPos, growPos, percentDone);
        float size = Mathf.Lerp(0, growSize, percentDone);
        infoText.transform.localScale = new Vector3(size, size, size);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Right Controller")
        {
            grow = true;
            startTime = Time.time;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.name == "Right Controller")
        {
            grow = false;
            startTime = Time.time;
        }
    }

    void LateUpdate()
    {
        infoText.transform.rotation = Quaternion.LookRotation((
            infoText.transform.position - Camera.main.transform.position).normalized);
    }
}
