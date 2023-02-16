using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MoveStates
{
	[CreateAssetMenu(fileName = "MvState", menuName = "MoveStates/WallRun")]
	public class MsWallRun : MoveState
	{
        [SerializeField] float coolDown = 1;
        [SerializeField] float entryBoost;
		[SerializeField] float gravityScale;
        [SerializeField] float checkDist;
        [SerializeField] float runSpeed;
        [SerializeField] float runForce;
        [SerializeField] float minMvSpeed;
        [SerializeField] float maxFallSpeed;
        [SerializeField] float stickForce;
        [SerializeField] Vector3 exitVel;
        [SerializeField] LayerMask wallLayer;
        [Header("States")]
        [SerializeField] int inAir;
        [SerializeField] int running;
        Vector3 fwd,side;
        float distToWall = 0;
        float lastTimeUsed = 0;
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
				player.ChangeState(player.moveStates[running]);
			}

            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, player.transform.right, out hit, checkDist,wallLayer))
            {
                //calculate the fwd vect
                //do not invert for left

                //todo: add a check for vertical walls
                Vector3 outWall = player.transform.position-hit.collider.ClosestPoint(player.transform.position);
                distToWall = Vector3.Distance(player.transform.position, hit.collider.ClosestPoint(player.transform.position));
                fwd = -Vector3.Cross(outWall, Vector3.up);
                
                fwd = fwd.normalized;



                side = -outWall;
                side.y = 0;
                side = side.normalized;
                
            }
            else if (Physics.Raycast(player.transform.position, -player.transform.right, out hit, checkDist,wallLayer))
            {

                Vector3 outWall = player.transform.position - hit.collider.ClosestPoint(player.transform.position);
                distToWall = Vector3.Distance(player.transform.position, hit.collider.ClosestPoint(player.transform.position));
                fwd = Vector3.Cross(outWall, Vector3.up);
                fwd.y = 0;
                fwd = fwd.normalized;



                side = -outWall;
                side.y = 0;
                side = side.normalized;
                
            }
			else
			{
                player.ChangeState(player.moveStates[inAir]);
            }
            Vector3 vel = player.rb.velocity;
            vel.y = 0;
            if (player.rb.velocity.y > maxFallSpeed || vel.magnitude < minMvSpeed)
            {
                player.ChangeState(player.moveStates[inAir]);
            }
			if (Input.GetKey(KeyCode.Space))
			{
                jump = true;
			}
            
            
        }

		public override void StateFixedUpdate()
		{
            player.rb.AddForce(side * (player.rb.velocity.sqrMagnitude/(stickForce)));
            player.rb.AddForce(Vector3.down * gravityScale);
			if (jump)
			{
                player.rb.AddForce(Vector3.up * exitVel.y + -side*(exitVel.x + player.rb.velocity.sqrMagnitude/(stickForce)) + fwd * exitVel.z);
                player.ChangeState(player.moveStates[inAir]);
                lastTimeUsed = Time.time;
            }
            Vector3 horizontalVel = player.rb.velocity;
            horizontalVel.y = 0;
            float vRel = Vector3.Dot(fwd, horizontalVel);
            //Debug.Log(vRel);
            if (vRel < runSpeed)
            {
                player.rb.AddForce(fwd * runForce * Input.GetAxisRaw("Vertical"));
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