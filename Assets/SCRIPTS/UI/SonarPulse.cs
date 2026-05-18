using UnityEngine;
using UnityEngine.UI;

public class SonarPulse : MonoBehaviour
{
    public float lifeTime = 3f;
    public float startSize = 0.2f;
    public float endSize = 3f;
    public bool isPredictedPriceHigherThanCurrent;

    private Image image;
    private float timer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        transform.localScale = Vector3.one * startSize;
        if (isPredictedPriceHigherThanCurrent)
        {
            image.color = GameConstants.greenColor;
        }
        else
        {
            image.color = Color.softRed;//GameConstants.redColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        float t = (timer / lifeTime) * (timer / lifeTime);
        
        float scale = Mathf.Lerp(startSize, endSize, t);
        transform.localScale = Vector3.one * scale;

        Color c = image.color;
        c.a = Mathf.Lerp(1, 0, t);
        image.color = c;

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
