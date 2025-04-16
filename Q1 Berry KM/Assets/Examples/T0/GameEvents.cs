using TMPro;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private int points;
    public TMP_Text score;
    void Awake()
    {
        points = 0;
        Debug.Log("in awake");
    }

    //update is called once per frame
    void Update()
    {
        score.text = "Score: " + points;

        // 1) detect space bar on down
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            // 2) find the prefab meta information
            GameObject temp = Resources.Load<GameObject>("PinBall");
            int val = (int)(Random.value * 3);
            switch(val)
            {
                case 0:
                    //already done
                    break;
                case 1:
                    temp = Resources.Load<GameObject>("HighValPinBall");
                    break;
                case 2:
                    temp = Resources.Load<GameObject>("ColorPinBall");
                    break;
            }

            // and then make the prefab at the given location, with no rotation
            GameObject pinball = Instantiate(temp, new Vector3(0, 4, 0), Quaternion.identity);

            // 3) reconnect to GameEvents script
            pinball.GetComponent<PinBallBehavior>().game = this;

            // 4) apply some random upwards force
            float theta = Random.value * Mathf.PI;
            float scale = 20;
            float x = Mathf.Cos(theta) * scale;
            float y = Mathf.Sin(theta) * scale;
            pinball.GetComponent<Rigidbody>().AddForce(new Vector3(x, y, 0), ForceMode.VelocityChange);
        }
    }

    public void AddPoint(int points) 
    { 
        this.points += points; 
    }
}
