using UnityEngine;
using Vuforia;
using UnityEngine.InputSystem;

public class SpawnWhenTracked : MonoBehaviour
{
    [SerializeField] ObserverBehaviour target;
    [SerializeField] GameObject prefab;
    [SerializeField] Transform anchor;
    [SerializeField] GameObject particle;

    GameObject instance;

    void Awake()
    {
        target.OnTargetStatusChanged += OnStatusChanged;
    }

    void OnDestroy()
    {
        target.OnTargetStatusChanged -= OnStatusChanged;
    }

    void Update()
    {
        if (instance == null) return;
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            CriarEfeitoDeParticula();
        }
    }

    void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool tracked =
            status.Status == Status.TRACKED ||
            status.Status == Status.EXTENDED_TRACKED;

        if (tracked && instance == null)
        {
            instance = Instantiate(prefab, anchor.position, anchor.rotation, anchor);
        }

        if (!tracked && instance != null)
        {
            Destroy(instance);
        }
    }

    void CriarEfeitoDeParticula()
    {
        #if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
        #endif

        if (particle != null && instance != null)
        {
            GameObject Particulas = Instantiate(particle, instance.transform.position, instance.transform.rotation, instance.transform);
            Destroy(Particulas, 2.0f);
        }
    }
}
