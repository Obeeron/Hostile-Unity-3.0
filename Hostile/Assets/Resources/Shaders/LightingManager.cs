using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    #pragma warning disable 649
    [Header("References")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;
    
    [Header("Attributes")]
    [SerializeField, Min(0)] private int cycleLength = 1500; 
    [SerializeField, Range(0, 1500)] private float timeOfDay = 0f;
    #pragma warning restore 649
    private float timepercent;
    private bool isDay = true;

    public float TimePercent
    {
        get => timepercent;
    }

    public bool IsDay
    {
        get => isDay;
    }

    void Start()
    {
        timeOfDay=0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(preset==null) return;

        if (Application.isPlaying)
        {
            timeOfDay += Time.deltaTime;
            timeOfDay %= cycleLength;
        }
        timepercent = timeOfDay / cycleLength;
        UpdateLighting(timepercent);
    }

    private void UpdateLighting(float timePercent)
    {
        //Set ambient color and fog color
        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

        //Directional light rotation and color
        if (directionalLight != null)
        {
            directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);
            directionalLight.intensity = preset.IntensityCurve.Evaluate(timePercent)*1.3f;
            //RenderSettings.ambientIntensity = preset.IntensityCurve.Evaluate(timePercent);
            if(timeOfDay<cycleLength*0.6)
            {
                directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent/0.6f * 180f), 170f, 0));
                if(!isDay) isDay = true;
            }
            else{
                directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(180f+((timePercent-0.6f)/0.4f * 180f), 170f, 0));
                if(isDay) isDay = false;
            }
                
        }
    }
}
