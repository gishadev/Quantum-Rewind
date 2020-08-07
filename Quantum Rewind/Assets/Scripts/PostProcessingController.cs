using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PostProcessingController : MonoBehaviour
{
    #region Singleton
    public static PostProcessingController Instance { private set; get; }
    #endregion

    Volume volume;
    //LensDistortion ld;
    ChromaticAberration ca = null;
    LensDistortion ld = null;
    DepthOfField dof = null;

    void Awake()
    {
        Instance = this;

        volume = GetComponent<Volume>();
        volume.sharedProfile.TryGet(out ca);
        volume.sharedProfile.TryGet(out ld);
        volume.sharedProfile.TryGet(out dof);
    }

    void Start()
    {
        ResetValues();
    }

    public void TriggerChromaticAberration(float targetValue, float speedOfChanging, bool isLoop)
    {
        StartCoroutine(ControlChromaticAberration(targetValue, speedOfChanging, isLoop));
    }

    public void TriggerLensDistortion(float targetValue, float speedOfChanging, bool isLoop)
    {
        StartCoroutine(ControlLensDistortion(targetValue, speedOfChanging, isLoop));
    }

    public void TriggerDepthOfField(bool isEnabled)
    {
        dof.active = isEnabled;
    }

    void ResetValues()
    {
        ca.intensity.value = 0f;
        ld.intensity.value = 0f;
    }

    #region Coroutines
    IEnumerator ControlChromaticAberration(float newIntensity, float speedOfChanging, bool isLoop)
    {
        bool isProgressing = false;

        if (ca != null)
            isProgressing = true;
        else
            Debug.LogError("Setting was not found.");

        bool isNewHigher = newIntensity > ca.intensity.value;
        float oldIntensity = ca.intensity.value;

        if (!isNewHigher)
            speedOfChanging = -speedOfChanging;

        while (isProgressing)
        {
            // Changing value.
            ca.intensity.value += Time.deltaTime * speedOfChanging;

            // Checking for progressing state.
            if (isNewHigher)
                isProgressing = ca.intensity.value < newIntensity;
            else
                isProgressing = ca.intensity.value > newIntensity;

            // If it's looped => returning to start point.
            if (isLoop)
                if (!isProgressing)
                    StartCoroutine(ControlChromaticAberration(oldIntensity, speedOfChanging, false));

            if (!isProgressing)
                ca.intensity.value = newIntensity;

            yield return null;
        }
    }

    IEnumerator ControlLensDistortion(float newIntensity, float speedOfChanging, bool isLoop)
    {
        bool isProgressing = false;

        if (ld != null)
            isProgressing = true;
        else
            Debug.LogError("Setting was not found.");

        bool isNewHigher = newIntensity > ca.intensity.value;
        float oldIntensity = ca.intensity.value;

        if (!isNewHigher)
            speedOfChanging = -speedOfChanging;

        while (isProgressing)
        {
            // Changing value.
            ld.intensity.value += Time.deltaTime * speedOfChanging;

            // Checking for progressing state.
            if (isNewHigher)
                isProgressing = ld.intensity.value < newIntensity;
            else
                isProgressing = ld.intensity.value > newIntensity;

            // If it's looped => returning to start point.
            if (isLoop)
                if (!isProgressing)
                    StartCoroutine(ControlLensDistortion(oldIntensity, speedOfChanging, false));

            if (!isProgressing)
                ld.intensity.value = newIntensity;

            yield return null;
        }
    }
    #endregion
}
