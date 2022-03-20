using UnityEngine;

public class ThrowableScript : MonoBehaviour
{
    public float throwForce = 20f;
    public GameObject grenade;//{get;set;}
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Throw();
        }

        void Throw()
        {
            GameObject grenadeObject = Instantiate(grenade, transform.position, transform.rotation);
            Rigidbody rb = grenadeObject.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        }

    }
}
