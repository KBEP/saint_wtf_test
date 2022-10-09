using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(PlayerInput))]
[RequireComponent (typeof(CharacterController))]

//класс отвечает за движение персонажа
public class PlayerController : MonoBehaviour
{
	float moveSpeed = 6.0f;
	Vector3 gravity = new Vector3 (0.0f, -9.8f, 0.0f);
	Vector2 stickValue;
	PlayerInput playerInput;
	CharacterController cc;

	void Start ()
	{
		playerInput = GetComponent<PlayerInput>();
		cc = GetComponent<CharacterController>();
	}

	void Update ()
	{
		Vector3 motion = new Vector3(stickValue.x, 0.0f, stickValue.y) * moveSpeed;
		if (motion != Vector3.zero) cc.transform.localRotation = Quaternion.LookRotation(motion);
		if (!cc.isGrounded) motion += gravity;
		cc.Move(motion * Time.deltaTime);
	}

	public void OnStickValueChanged (InputAction.CallbackContext context)
	{
		stickValue = context.ReadValue<Vector2>();
	}
}
