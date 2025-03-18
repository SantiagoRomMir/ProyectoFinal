using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraspassFloor : MonoBehaviour
{
    private PlatformEffector2D effector;
    // Start is called before the first frame update
    void Start()
    {
        effector=gameObject.GetComponent<PlatformEffector2D>();       
    }
    public IEnumerator Rotar(){
        effector.rotationalOffset=180;
        yield return new WaitForSeconds(0.4f);
        effector.rotationalOffset=0;
    }

}
