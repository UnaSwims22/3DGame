using UnityEngine;

public class BreakableSupport : MonoBehaviour
{
    [Header("Parent Structure")]
    public GameObject parentStructure;

    public void Break()
    {
        Destroy(gameObject);


        if (parentStructure != null)
        {
            if (parentStructure.GetComponentsInChildren<BreakableSupport>().Length <= 1)
            {
                Rigidbody rb = parentStructure.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }


            }
        }
    }
}
    

