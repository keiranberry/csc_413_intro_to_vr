using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LaserSelection : MonoBehaviour
{
    private LineRenderer line;

    [SerializeField]
    private float width = 0.05f;

    [SerializeField]
    private float distance = 5f;

    private Vector3[] points = new Vector3[2];

    [SerializeField]
    private InputActionProperty laserOnOff;

    private Button lastOver = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //(1) make a line renderer with a base material so the color can be changed
        line = gameObject.AddComponent<LineRenderer>();
        line.material = new Material((Material)Resources.Load("Materials/ColorChange"));
        line.startWidth = width;
        line.endWidth = width;
        SetColor(Color.magenta);

        line.enabled = false;

        // (3) register callback request
        if(laserOnOff == null)
        {
            Debug.Log("No laser on/off action registered");
        }
        else
        {
            laserOnOff.action.performed += OnLaserOn;
            laserOnOff.action.canceled += OnLaserOn;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //(2) update location if enabled
        if (line.enabled == true)
        {
            points[0] = transform.position + transform.up * transform.localScale.y; //point at hand
            points[1] = transform.position + transform.up * distance; //point 4 units forward from hand
            line.SetPositions(points);
        }

        Button hit = PerformButtonRayCast();
        if (hit != null)
        {
            SetColor(Color.green);

            ExecuteEvents.Execute(hit.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
            lastOver = hit;
        }
        else
        {
            SetColor(Color.magenta);
            if (lastOver != null)
            {
                ExecuteEvents.Execute(lastOver.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
                lastOver = null;
            }
        }
    }

    private void SetColor(Color color)
    {
        line.startColor = color;
        line.endColor = color;
        line.material.SetColor("_EmissionColor", color * 255);
    }    

    public void OnLaserOn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            line.enabled = true;
        }
        else if (context.canceled)
        {
            Button hit = PerformButtonRayCast();
            if (hit != null)
            {
                hit.onClick.Invoke();
            }

            line.enabled= false;
        }
    }

    private Button PerformButtonRayCast()
    {
        Ray r = new Ray(points[0], points[1] - points[0]);
        RaycastHit hit;
        if(Physics.Raycast(r, out hit, distance))
        {
            //update line regardless
            points[1] = hit.point;
            line.SetPositions(points);

            return hit.collider.gameObject.GetComponent<Button>();
        }

        return null;
    }
}
