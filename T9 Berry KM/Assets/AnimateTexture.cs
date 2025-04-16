using UnityEngine;

public class AnimateTexture : MonoBehaviour
{
    [SerializeField]
    private float rateU = 0.02f;
    [SerializeField]
    private float rateV = 0.05f;
    //combined U and V rates
    private Vector2 rate;
    //current offset
    private Vector2 uvOffset = Vector2.zero;
    //reference to locate material for efficiency
    private Material mat;
    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
        rate = new Vector2(rateU, rateV);
    }
    void Update()
    {
        uvOffset += (rate * Time.deltaTime);
        mat.SetTextureOffset("_MainTex", uvOffset);
    }

}
