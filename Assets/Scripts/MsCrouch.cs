using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStates
{
	[CreateAssetMenu(fileName = "MvState", menuName = "MoveStates/Crouch")]
	public class MsCrouch : MoveState
	{
		[Tooltip("Highest speed player can run to")]
		[SerializeField] float maxSpeed = 10;
		[SerializeField] float acceleration = 5;
		[SerializeField] float counterForce = 10;
		[SerializeField] float controlForce = 10;
		[SerializeField] float jumpForce = 800;

		[Header("States")]
		[SerializeField] int running;
		[SerializeField] int inAir;
		[SerializeField] int slide;


		float cfConst = 40;
		public Vector2 inputDir = Vector2.zero;


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
			Vector3 vel = player.rb.velocity;
			vel.y = 0;
			if (vel.magnitude > maxSpeed)
			{
				player.ChangeState(player.moveStates[slide]);
			}
			if (!IsGrounded())
			{
				player.ChangeState(player.moveStates[inAir]);
			}
		}
		public override void StateUpdate()
		{
			if (!IsGrounded())
			{
				player.ChangeState(player.moveStates[inAir]);
			}
			inputDir.y = Input.GetAxisRaw("Vertical");
			inputDir.x = Input.GetAxisRaw("Horizontal");
			//inputDir = inputDir.normalized;

			if (Input.GetKey(KeyCode.Space))
			{
				jump = true;
			}
			Vector3 vel = player.rb.velocity;
			vel.y = 0;
			if (vel.magnitude > maxSpeed)
			{
				player.ChangeState(player.moveStates[slide]);
			}
			if (!Input.GetKey(KeyCode.LeftControl))
			{
				player.ChangeState(player.moveStates[running]);
			}


			ControlCamera();

		}

		//Debug vector
		Vector3 totalForce;
		public override void StateFixedUpdate()
		{
			//jump
			if (jump)
			{
				player.rb.AddForce(Vector3.up * jumpForce);
				player.ChangeState(player.moveStates[inAir]);
			}

			if (inputDir.sqrMagnitude > 0)
			{
				Vector3 moveDir = player.transform.forward * inputDir.y * acceleration + player.transform.right * inputDir.x * acceleration;

				Vector3 idealVel = moveDir.normalized * maxSpeed;
				Vector3 vel = player.rb.velocity;
				idealVel.y = 0;
				//cap the force so velocity stays below max
				if (Mathf.Sign(moveDir.x) == 1)
				{

					if (vel.x + moveDir.x > idealVel.x)
					{
						moveDir.x = Mathf.Clamp(idealVel.x - vel.x, 0, float.PositiveInfinity);
					}
				}
				else
				{

					if (vel.x + moveDir.x < idealVel.x)
					{
						moveDir.x = Mathf.Clamp(idealVel.x - vel.x, float.NegativeInfinity, 0);
					}

				}
				if (Mathf.Sign(moveDir.z) == 1)
				{

					if (vel.z + moveDir.z > idealVel.z)
					{
						moveDir.z = Mathf.Clamp(idealVel.z - vel.z, 0, float.PositiveInfinity);
					}

				}
				else
				{

					if (vel.z + moveDir.z < idealVel.z)
					{
						moveDir.z = Mathf.Clamp(idealVel.z - vel.z, float.NegativeInfinity, 0);
					}

				}
				moveDir.y = 0;
				player.rb.AddForce(moveDir);


				//Counter Sliding and increase overall control
				vel.y = 0;

				Vector3 inputVel = moveDir.normalized * vel.magnitude;

				Vector3 counterVel = -(vel - inputVel);

				counterVel.y = 0;

				float cfScale = Vector3.Distance(vel, inputVel) / cfConst;
				player.rb.AddForce(counterVel * controlForce);

				totalForce = moveDir + counterVel;
			}
			else
			{
				ApplyCounterForce(player.rb, counterForce);
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




		public override void OnDrawGizmos()
		{

			if (player)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(player.transform.position + Vector3.down * gcCheckDist, gcRad);
				/*
				Gizmos.color = Color.magenta;
				Gizmos.DrawLine(player.transform.position, player.transform.position + totalForce);
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(player.transform.position, player.transform.position + player.transform.forward);
				*/
			}

		}
		public override void OnExitState()
		{
			jump = false;
			CapsuleCollider col = player.col;
			col.height = 1;
			col.center = new Vector3(0, -0.5f, 0);
		}


	}
}