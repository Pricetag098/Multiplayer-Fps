using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStates
{
	[CreateAssetMenu(fileName = "MvState", menuName = "MoveStates/Slide")]
	public class MsSlide : MoveState
	{
		[SerializeField] float gravityScale = 9.8f * 8;
		[SerializeField] float jumpForce = 800;
		[Header("States")]
		[SerializeField] int inAir;
		[SerializeField] int running;
		[SerializeField] int crouch;
		
		bool jump = false;
		public override void EnterState()
		{
			CapsuleCollider col = player.col;
			col.height = 1;
			col.center = new Vector3(0, -0.5f, 0);

			if (!Input.GetKey(KeyCode.LeftControl))
			{
				player.ChangeState(player.moveStates[running]);
			}
		}

		public override void StateUpdate()
		{
			if (!IsGrounded())
			{
				player.ChangeState(player.moveStates[inAir]);
			}
			ControlCamera();
			if (!Input.GetKey(KeyCode.LeftControl))
			{
				player.ChangeState(player.moveStates[running]);
			}
			if (Input.GetKey(KeyCode.Space))
			{
				jump = true;
			}

		}

		public override void StateFixedUpdate()
		{
			player.rb.AddForce(Vector3.down * gravityScale);
			if (jump)
			{
				player.rb.AddForce(Vector3.up * jumpForce);
				player.ChangeState(player.moveStates[inAir]);
				
			}
		}


		public override void OnExitState()
		{
			CapsuleCollider col = player.col;
			col.height = 2;
			col.center = new Vector3(0, 0, 0);
			jump = false;
		}

		public override void OnDrawGizmos()
		{
			if (player)
			{

				Gizmos.color = Color.blue;
				
			}
		}
		private void ApplyCounterForce(Rigidbody rb, float scale)
		{
			Vector3 vel = rb.velocity;
			//	vel.y = 0;
			vel = -vel;
			rb.AddForce(vel * scale);
			if (Mathf.RoundToInt(rb.velocity.sqrMagnitude) == 0)
			{
				rb.velocity = Vector3.zero;
			}

		}
	}
}