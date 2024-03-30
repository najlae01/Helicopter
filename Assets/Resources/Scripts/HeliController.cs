using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Analytics;

public class HeliController : MonoBehaviour {

	public float speed = 10.0f;
	public int coinTotal = 0;
	private Rigidbody rb;
	private float vertical, horizontal;
	public ParticleSystem explosion;
	public AudioSource explosionSound;

	private CustomInput input;
	private Vector2 moveVector = Vector2.zero;
    private bool isGameOver = false;

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody>();
        input = new CustomInput();
        isGameOver = false;
        OnEnable();
    }

    // Update is called once per frame
    void Update () {
        if (!isGameOver)
        {
            // vertical axis is up or down on the keyboard
            vertical = moveVector.y * speed;

			// constraining movement within the bounds of the camera
			if (transform.position.y < -9.5f) {
				transform.position = new Vector3(transform.position.x, -9.5f, transform.position.z);
			}
			if (transform.position.y > 9) {
				transform.position = new Vector3(transform.position.x, 9, transform.position.z);
			}


            // horizontal axis is left or right on the keyboard
            horizontal = moveVector.x * speed;

			// constraining movement within the bounds of the camera
			if (transform.position.x < -12.5f) {
				transform.position = new Vector3(-12.5f, transform.position.y, transform.position.z);
			}
			if (transform.position.x > 15.5f) {
				transform.position = new Vector3(15.5f, transform.position.y, transform.position.z);
			}
		

		    // setting rigidbody's velocity to our input
		    rb.velocity = new Vector3(horizontal, vertical, 0);
        }
    }

	public void PickupCoin() {
		coinTotal += 1;

		// trigger audio playback and emit particles from particle system
		GetComponents<AudioSource>()[0].Play();
		GetComponent<ParticleSystem>().Play();
	}

    public void PickupGem()
    {
        coinTotal += 5;

        // trigger audio playback and emit particles from particle system
        GetComponents<AudioSource>()[0].Play();
        GetComponent<ParticleSystem>().Play();
    }

    private void OnEnable()
    {
		if (input != null)
		{
			input.Enable();
			input.Helicopter.Enable();
			input.Helicopter.movement.performed += OnMovementPerformed;
			input.Helicopter.movement.canceled += OnMovementCancelled;
		}
		else
		{
            Debug.LogWarning("Input is null in HeliController.");
        }
    }

    private void OnDisable()
    {
        input.Helicopter.movement.performed -= OnMovementPerformed;
        input.Helicopter.movement.canceled -= OnMovementCancelled;
        input.Helicopter.Disable();
        input.Disable();
    }

    public void Explode() {
		explosionSound.Play();

		// set explosion position to helicopter's and emit
		explosion.transform.position = transform.position;
		explosion.Play();

        isGameOver = true;
		OnDisable();
        Destroy(gameObject);
	}

	private void OnMovementPerformed(InputAction.CallbackContext value)
	{
		moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

}

