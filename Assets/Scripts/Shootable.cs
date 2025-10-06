using UnityEngine;

public class Shootable : MonoBehaviour
{

    public virtual void ReactToHit(Vector3 hitPoint)
    {
        Debug.Log($"{gameObject.name} was hit but has no reaction.");
    }

}
