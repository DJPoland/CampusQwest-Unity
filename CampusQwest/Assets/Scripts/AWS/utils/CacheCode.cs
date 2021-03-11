using UnityEngine;

public class CacheCode : MonoBehaviour
{
    public static CacheCode Instance;
    public string confirmationCode;
    public string email;
    void Awake()
    {
        if (Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
		} else if (Instance != this)
        {
            Destroy(gameObject);
		}
    }
}
