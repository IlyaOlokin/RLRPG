using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float secondsToDeath;
    
    void Start()
    {
        Destroy(gameObject, secondsToDeath);
    }
}
