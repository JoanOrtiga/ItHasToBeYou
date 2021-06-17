
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        float x = Mathf.Sin(Time.time);
        if (x >= 0)
            image.color = Color.Lerp(image.color, Color.white, Time.deltaTime * 1f);
        else
        {
            image.color = Color.Lerp(image.color, Color.black, Time.deltaTime *1f);
        }
    }
}
