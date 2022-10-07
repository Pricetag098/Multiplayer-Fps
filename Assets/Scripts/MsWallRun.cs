using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MoveStates
{
	[CreateAssetMenu(fileName = "MvState", menuName = "MoveStates/WallRun")]
	public class MsWallRun : MoveState
	{
        [SerializeField] float entryBoost;
		[SerializeField] float gravityScale;
        [SerializeField] float checkDist;
        [SerializeField] float minMvSpeed;
        [SerializeField] float maxFallSpeed;
        [SerializeField] float stickForce;
        [SerializeField] Vector3 exitVel;
        [SerializeField] LayerMask wallLayer;

        [SerializeField] MoveState inAir;
        [SerializeField] MoveState running;
        Vector3 fwd,side;

        bool jump;
		public override void EnterState()
		{
            Vector3 vel = player.rb.velocity;
            vel.y = 0;
            player.rb.velocity = vel;
            player.rb.AddForce(Vector3.up * entryBoost);
            
        }
		public override void StateUpdate()
		{
			ControlCamera();
			if (IsGrounded())
			{
				player.ChangeState(running);
			}

            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, player.transform.right, out hit, checkDist,wallLayer))
            {
                //calculate the fwd vect
                //do not invert for left

                //todo: add a check for vertical walls
                fwd = -Vector3.Cross(hit.normal, Vector3.up);
                
                fwd = fwd.normalized;



                side = -hit.normal;
                side.y = 0;
                side = side.normalized;
                
            }
            else if (Physics.Raycast(player.transform.position, -player.transform.right, out hit, checkDist,wallLayer))
            {
                fwd = Vector3.Cross(hit.normal, Vector3.up);
                fwd.y = 0;
                fwd = fwd.normalized;



                side = -hit.normal;
                side.y = 0;
                side = side.normalized;
                
            }
			else
			{
                player.ChangeState(inAir);
            }
            Vector3 vel = player.rb.velocity;
            vel.y = 0;
            if (player.rb.velocity.y > maxFallSpeed || vel.magnitude < minMvSpeed)
            {
                player.ChangeState(inAir);
            }
			if (Input.GetKey(KeyCode.Space))
			{
                jump = true;
			}

        }

		public override void StateFixedUpdate()
		{
            player.rb.AddForce(side * stickForce);
            player.rb.AddForce(Vector3.down * gravityScale);
			if (jump)
			{
                player.rb.AddForce(Vector3.up * exitVel.y + -side*exitVel.x + fwd * exitVel.z);
                player.ChangeState(inAir);
            }
		}
		public override void OnExitState()
		{
            jump = false;
        }

		public override void OnDrawGizmos()
		{
			if (player)
			{
                Gizmos.color = Color.red;
                Gizmos.DrawLine(player.transform.position, player.transform.position + fwd * 5);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(player.transform.position, player.transform.position + player.transform.right * checkDist);
                Gizmos.DrawLine(player.transform.position, player.transform.position + -player.transform.right * checkDist);
            }
		}

	}
}