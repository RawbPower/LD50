using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Sun : MonoBehaviour
{
    public float startSunDamageRate;
    public float endSunDamageRate;
    public Camera cam;
    public Transform followTransform;
    public float setSpeed;
    public SpriteRenderer spriteRenderer;
    public Color startColor;
    public Color setColor;
    public Color endColor;
    public Color sunStartColor;
    public Color sunSetColor;
    public Color sunEndColor;
    public float setPosition;
    public float endPosition;

    public Tilemap tilemap;
    public SpriteRenderer[] palatterRenderers;

    private Color currentColor;
    private Color sunColor;
    private float sunDamageRate;
    Vector2 startPosition;
    float startSunOffset;
    float sunOffset;
    float setOffset;
    float endOffset;
    float setRange;
    float endRange;

    private bool sunset = false;
    private bool dusk = false;
    private bool freezeTime = false;

    public void ResumeTime() { freezeTime = false; }

    float viewWidth => spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit;

    Vector2 travel => (Vector2)cam.transform.position - startPosition; //2D distance travelled from our starting position

    public float GetSunDamageRate() { return sunDamageRate; }
    public Color GetCurrentColor() { return currentColor; }
    public Color GetSunColor() { return sunColor; }

    // Start is called before the first frame update
    void Start()
    {
        sunDamageRate = startSunDamageRate;
        startPosition = transform.position;
        sunOffset = transform.position.y - cam.transform.position.y;
        setOffset = setPosition- cam.transform.position.y;
        endOffset = endPosition - cam.transform.position.y;
        startSunOffset = sunOffset;
        setRange = (startPosition.y - setPosition);
        endRange = (startPosition.y - endPosition);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float t = freezeTime ? 0.0f : (startSunOffset - sunOffset) / endRange;
        if (t < 1)
        {
            sunDamageRate = startSunDamageRate + t * (endSunDamageRate + startSunDamageRate);
        }

        if (dusk == true)
        {
            ProceduralLevel level = FindObjectOfType<ProceduralLevel>();
            level.dusk = true;
            sunDamageRate = 0.0f;
            freezeTime = true;
        }

        if (!freezeTime)
        {
            if (sunOffset > setOffset)
            {
                t = (startSunOffset - sunOffset) / setRange;
                currentColor = LerpHSV(startColor, setColor, t);
                sunColor = LerpHSV(sunStartColor, sunSetColor, t);
            }
            else
            {
                if (!sunset)
                {
                    ProceduralLevel level = FindObjectOfType<ProceduralLevel>();
                    level.sunset = true;
                    sunset = true;
                    freezeTime = true;
                }
                t = (setOffset - sunOffset) / (endRange - setRange);
                currentColor = LerpHSV(setColor, endColor, t);
                sunColor = LerpHSV(sunSetColor, sunEndColor, t);
            }
            spriteRenderer.color = sunColor;
            tilemap.color = currentColor;
            foreach (SpriteRenderer sr in palatterRenderers)
            {
                if (sr != null)
                {
                    sr.color = currentColor;
                }
            }

            if ((startSunOffset - sunOffset) < endRange)
            {
                sunOffset -= setSpeed * Time.deltaTime;
            }
            else
            {
                dusk = true;
            }
        }

        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + sunOffset, 0.0f);
    }

    public static Color LerpHSV(Color a, Color b, float t)
    {
        float aHue, aSat, aVal;
        float bHue, bSat, bVal;
        Color.RGBToHSV(a, out aHue, out aSat, out aVal);
        Color.RGBToHSV(b, out bHue, out bSat, out bVal);

        // Hue interpolation
        float h;
        float d = bHue - aHue;
        if (aHue > bHue)
        {
            // Swap (a.h, b.h)
            var h3 = bHue;
            bHue = aHue;
            aHue = h3;
            d = -d;
            t = 1 - t;
        }
        if (d > 0.5) // 180deg
        {
            aHue = aHue + 1; // 360deg
            h = (aHue + t * (bHue - aHue)) % 1; // 360deg
        }
        else // 180deg
        {
            h = aHue + t * d;
        }
        // Interpolates the rest
        return Color.HSVToRGB(h, aSat + t * (bSat - aSat), aVal + t * (bVal - aVal));
    }
}
