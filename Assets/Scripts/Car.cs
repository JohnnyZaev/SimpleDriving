using UnityEngine;

public class Car : MonoBehaviour
{

	[SerializeField] private float speed = 10f;
	[SerializeField] private float speedGainsPerSecond = 0.2f;
	[SerializeField] private float turnSpeed = 200f;

	private int _steerValue;
	

    // Update is called once per frame
    private void Update()
    {
	    speed += speedGainsPerSecond * Time.deltaTime;
	    
	    transform.Rotate(0, _steerValue * turnSpeed * Time.deltaTime, 0f);
	    
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    public void Steer(int value)
    {
	    _steerValue = value;
    }
}
