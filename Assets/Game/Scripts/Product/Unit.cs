using UnityEngine;

public abstract class Unit : MonoBehaviour,ISelectable
{
    public void Selected()
    {
        throw new System.NotImplementedException();
    }

    public abstract void Spawn();
}
