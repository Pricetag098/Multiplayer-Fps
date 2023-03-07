using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStates
{
    [CreateAssetMenu(fileName = "MvState", menuName = "MoveStates/InAir")]
    public class MsInAir : MoveState
    {
		[SerializeField] float gravityScale = 9.8f*4;


		[SerializeField] float wallCheckDist =1;
		[SerializeField] float wrMinMvSpeed;
		[SerializeField] float wrMaxFallSpeed = 9.8f*4;
		[SerializeField] LayerMask wallLayer = 1;

		[SerializeField] float maxSpeed;
		[SerializeField] float acceleration;
		

		[SerializeField] float colOffset = 0;
		[SerializeField] float colHeight = 2;
		[Header("States")]
		[SerializeField] int running;
		[SerializeField] int wallRun;
		Vector2 inputDir;
		public override void EnterState()
		{
			
			CapsuleCollider col = player.col;
			col.height = colHeight;
			col.center = Vector3.up * colOffset;
		}
		public override void StateUpdate()
		{
			if (IsGrounded())
			{
				player.ChangeState(player.moveStates[running]);
			}
			ControlCamera();

            inputDir.y = Input.GetAxisRaw("Vertical");
            inputDir.x = Input.GetAxisRaw("Horizontal");
            inputDir = inputDir.normalized;

            if (Physics.Raycast(player.transform.position, player.transform.right, wallCheckDist,wallLayer) || Physics.Raycast(player.transform.position, -player.transform.right, wallCheckDist, wallLayer))
			{
				Vector3 vel = player.rb.velocity;
				vel.y = 0;
				if (player.rb.velocity.y < wrMaxFallSpeed || vel.magnitude > wrMinMvSpeed)
				{
					player.ChangeState(player.moveStates[wallRun]);
				}
			}
			
        }

		public override void StateFixedUpdate()
		{
			player.rb.AddForce(Vector3.down * gravityScale);
			if(Vector3.Dot(player.transform.forward * inputDir.y, player.rb.velocity) < maxSpeed * Mathf.Abs(inputDir.y))
			{
				player.rb.AddForce(player.transform.forward * acceleration * inputDir.y);
			}
            if (Vector3.Dot(player.transform.right * inputDir.x, player.rb.velocity) < maxSpeed * Mathf.Abs(inputDir.x))
            {
                player.rb.AddForce(player.transform.right * acceleration * inputDir.x);
            }
        }

		public override void OnExitState()
		{
			CapsuleCollider col = player.col;
			col.height = 2;
			col.center = new Vector3(0, 0, 0);
		}
		public override void OnDrawGizmos()
		{
			if (player)
			{
				
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(player.transform.position, player.transform.position + player.transform.right * wallCheckDist);
				Gizmos.DrawLine(player.transform.position, player.transform.position + -player.transform.right * wallCheckDist);
			}
		}

	}
}

