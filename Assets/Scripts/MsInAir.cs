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

		[SerializeField] MoveState running;
		[SerializeField] MoveState wallRun;

		[SerializeField] float colOffset = 0;
		[SerializeField] float colHeight = 2;

		public override void EnterState()
		{
			CapsuleCollider col = player.GetComponent<CapsuleCollider>();
			col.height = colHeight;
			col.center = Vector3.up * colOffset;
		}
		public override void StateUpdate()
		{
			if (IsGrounded())
			{
				player.ChangeState(running);
			}
			ControlCamera();


            
			if (Physics.Raycast(player.transform.position, player.transform.right, wallCheckDist,wallLayer) || Physics.Raycast(player.transform.position, -player.transform.right, wallCheckDist, wallLayer))
			{
				Vector3 vel = player.rb.velocity;
				vel.y = 0;
				if (player.rb.velocity.y < wrMaxFallSpeed || vel.magnitude > wrMinMvSpeed)
				{
					player.ChangeState(wallRun);
				}
			}
			
        }

		public override void StateFixedUpdate()
		{
			player.rb.AddForce(Vector3.down * gravityScale);
		}

		public override void OnExitState()
		{
			CapsuleCollider col = player.GetComponent<CapsuleCollider>();
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

