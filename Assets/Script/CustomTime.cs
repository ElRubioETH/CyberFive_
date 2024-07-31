using UnityEngine;

public class CustomTimeManager : MonoBehaviour
{
    public static CustomTimeManager instance;

    public float customTimeScale = 1f;
    public float customFixedDeltaTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            customFixedDeltaTime = Time.fixedDeltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Time.timeScale = customTimeScale;
        Time.fixedDeltaTime = customFixedDeltaTime * Time.timeScale;
    }
}
