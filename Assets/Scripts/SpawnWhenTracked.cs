using UnityEngine;
using Vuforia;
using UnityEngine.InputSystem;

public class SpawnWhenTracked : MonoBehaviour
{
    [SerializeField] ObserverBehaviour target;
    [SerializeField] GameObject prefab;
    [SerializeField] Transform anchor;

    // Arraste o Prefab isolado das suas partículas aqui no Inspector do Unity
    [SerializeField] GameObject particlePrefab;

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

        // Identifica o toque na Unity 6
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

        if (particlePrefab != null && instance != null)
        {
            GameObject novasParticulas = Instantiate(particlePrefab, instance.transform.position, instance.transform.rotation, instance.transform);
            Destroy(novasParticulas, 2.0f);
        }
    }
}
