using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStates
{
	
	[CreateAssetMenu(fileName = "MvState", menuName = "MoveStates/Base")]
	public class MoveState : ScriptableObject
	{
		protected PlayerMove player;

		public static float sensitivity =1;
		[SerializeField] protected float gcCheckDist = .61f;
		[SerializeField] protected float gcRad = .4f;
		[SerializeField] protected LayerMask groundLayer = 1;
		[SerializeField] protected float camHeight = 1;

		public bool useCrouchAnimation;
		public void OnEnterState(PlayerMove p)
		{
			player = p;
			EnterState();
		}

		public virtual void EnterState()
		{

		}

		public virtual void StateUpdate()
		{

		}
		/// <summary>
		/// A virtual function used like fixed update
		/// </summary>
		public virtual void StateFixedUpdate()
		{

		}
		public virtual void OnExitState()
		{

		}
		public virtual void OnDrawGizmos()
		{

		}
		[HideInInspector]
		public float camRotX = 0;
		public virtual void ControlCamera()
		{
			if(player.cam == null) return;
			player.cam.position = player.transform.position + Vector3.up* camHeight;
			camRotX = Mathf.Clamp(-Input.GetAxisRaw("Mouse Y") * sensitivity + camRotX, -90, 90);
			player.cam.rotation = Quaternion.Euler(camRotX, player.cam.rotation.eulerAngles.y + Input.GetAxisRaw("Mouse X") * sensitivity, player.cam.rotation.eulerAngles.z);
			player.transform.rotation = Quaternion.Euler(0, player.cam.rotation.eulerAngles.y, 0);
		}

		protected virtual bool IsGrounded()
		{
			return Physics.CheckSphere(player.transform.position + Vector3.down * gcCheckDist, gcRad, groundLayer);
		}


	}
}

