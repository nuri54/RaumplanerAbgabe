using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SImpleGunScript : MonoBehaviour
{
    public Rigidbody Ball;

    public float velocity = 50;

    private bool fire = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float triggerLeft = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
        float triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);

        if (triggerRight > 0.9f && fire == false)
        {
            fire = true;
            Rigidbody clone = Instantiate(Ball, transform.position, transform.rotation) as Rigidbody;
            clone.velocity = transform.TransformDirection(new Vector3(0, 0, velocity));
            Destroy(clone.gameObject, 3);
        }

        if (fire == true && triggerRight < 0.1f)
        {
            fire = false;
        }
    }
}
