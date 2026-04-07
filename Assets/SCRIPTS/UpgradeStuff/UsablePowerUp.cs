using UnityEngine;

public class UsablePowerUp
{
    public Augment data;
    public int charges = 1;

    public void Use()
    {
        data?.Apply();
    }
}
