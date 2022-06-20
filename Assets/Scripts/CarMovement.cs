using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    private Vector3 _thrustForce = new Vector3(0f, 0f, 45f);
    private Vector3 _rotationTorque = new Vector3(0f, 8f, 0f);
    private bool _areControlsEnabled;

    private void Update()
    {
        if(!_areControlsEnabled)
        {
            return;
        }

        if(Input.GetKey("w"))
        {
            _rigidbody.AddRelativeForce(_thrustForce);
        }

        if (Input.GetKey("s"))
        {
            _rigidbody.AddRelativeForce(-_thrustForce);
        }

        if (Input.GetKey("a"))
        {
            _rigidbody.AddRelativeTorque(-_rotationTorque);
        }

        if (Input.GetKey("d"))
        {
            _rigidbody.AddRelativeTorque(_rotationTorque);
        }
    }

    public void EnableControls(bool enable)
    {
        _areControlsEnabled = enable;
    }
}
