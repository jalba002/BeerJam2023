using BEER2023.Enemy;
using UnityEngine;

public class SMBox : MonoBehaviour
{
    public bool addToListAuto = true;
    public bool IsEnabled => isEnabled;
    private bool isEnabled = true;

    public void Enable()
    {
        GameDirector.Instance.AddBox(this);
        isEnabled = true;
        // Cast a sphere alerting everyone nearby
        var cols =  Physics.OverlapSphere(transform.position, 25f);
        foreach (var item in cols)
        {
            item.gameObject.transform.gameObject.GetComponent<EnemyController>()?.Alert(this);
        }
    }
    public void Disable()
    {
        GameDirector.Instance.RemoveBox(this);
        // 
        isEnabled = false;
    }


}