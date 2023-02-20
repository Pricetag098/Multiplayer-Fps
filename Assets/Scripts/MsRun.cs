using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStates
{
	[CreateAssetMenu(fileName = "MvState", menuName = "MoveStates/Run")]
	public class MsRun : MoveState
    {
		[Tooltip("Highest speed player can run to")]
        [SerializeField] float maxSpeed;
		[SerializeField] float acceleration;
		[SerializeField] float counterForce;
		[SerializeField] float controlForce;
		[SerializeField] float jumpForce;
		[Header("States")]
		[SerializeField] int inAir;
		[SerializeField] int crouch;


		float cfConst = 40;
		[HideInInspector]
		public Vector2 inputDir = Vector2.zero;

		
		bool jump = false;
		
		public override void EnterState()
		{
			if (!IsGrounded())
			{
				player.ChangeState(player.moveStates[inAir]);
			}
			if (Input.GetKey(KeyCode.Space))
			{
				jump = true;
			}
			if (Input.GetKey(KeyCode.LeftControl))
			{
				player.ChangeState(player.moveStates[crouch]);
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
			inputDir = inputDir.normalized;
			
			if (Input.GetKey(KeyCode.Space))
			{
				jump = true;
			}
			if (Input.GetKey(KeyCode.LeftControl))
			{
				player.ChangeState(player.moveStates[crouch]);
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

			if(inputDir.sqrMagnitude > 0)
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

		private void ApplyCounterForce(Rigidbody rb,float scale)
		{
			Vector3 vel = rb.velocity;
			//	vel.y = 0;
			vel = -vel;
			rb.AddForce(vel*scale);
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
		}

		
	}
}






/*----------------------------------------------- Move Script graveyard -----------------------------------------------*/
/*



				Vector3 maxVel = player.rb.velocity;
				if (maxVel.sqrMagnitude == 0) { maxVel = Vector3.one; }
				maxVel.y = 0;
				maxVel = maxVel.normalized * maxSpeed;

				Vector3 inputForceFwd = player.transform.forward * inputDir.y * acceleration.y + player.transform.right * inputDir.x * acceleration.x;


				Vector3 playerVel = player.rb.velocity;
				playerVel.y = 0;

				if (Mathf.Abs(inputForceFwd.x + playerVel.x) > Mathf.Abs(maxVel.x) && Mathf.Sign(inputForceFwd.x) == Mathf.Sign(maxVel.x))
				{
					inputForceFwd.x = 0;
					Debug.Log(inputForceFwd.x + playerVel.x);
				}
				if (Mathf.Abs(inputForceFwd.z + playerVel.x) > Mathf.Abs(maxVel.z) && Mathf.Sign(inputForceFwd.z) == Mathf.Sign(maxVel.z))
				{
					inputForceFwd.z = 0;
					Debug.Log(inputForceFwd.x + playerVel.x);
				}
				//inputForceFwd.x -= Mathf.Sign(inputForceFwd.z) * Mathf.Clamp(Mathf.Abs(playerVel.x - maxVel.x), 0, float.PositiveInfinity);
				//inputForceFwd.z -= Mathf.Sign(inputForceFwd.z) * Mathf.Clamp(Mathf.Abs(playerVel.z - maxVel.z), 0, float.PositiveInfinity);
				//inputForceFwd -= playerVel - maxVel;


				test = inputForceFwd;

				test2 = (inputForceFwd - playerVel) * Vector3.Distance(inputForceFwd, playerVel) / acceleration;
				test2.y = 0;
				player.rb.AddForce(inputForceFwd);
				//player.rb.AddForce(test2 );
				*/
/*
				
				vel = player.rb.velocity;
				vel.y = 0;
				//maxVel = maxVel.normalized * maxSpeed;

				debugVect1 = vel;


				Vector3 moveDir = player.transform.forward * inputDir.y + player.transform.right * inputDir.x;

				Vector3 idealVel = moveDir.normalized * maxSpeed;

				debugVect2 = moveDir;

				Vector3 forceDir = -(vel-idealVel);

				debugVect3 = forceDir;
				moveDir = moveDir.normalized * acceleration;
				debugVect2 = idealVel;


				forceDir.y = 0;
				player.rb.AddForce(forceDir.normalized * slowDownForce);
				//player.rb.AddForce(moveDir * acceleration);
				
				//Vector3 moveDir = player.transform.forward * inputDir.y + player.transform.right * inputDir.x;*/
/*
 * if(Mathf.Sign(moveDir.x) == 1)
				{
					if (vel.x + moveDir.x > idealVel.x)
					{
						moveDir.x = 0;
					}
					else if (Mathf.Sign(vel.x) != 1)
					{
						//moveDir.x += counterForce;
					}
					
				}
				else
				{
					if (vel.x + moveDir.x < idealVel.x)
					{
						moveDir.x = 0;
					}
					else if (Mathf.Sign(vel.x) != -1)
					{
						//moveDir.x -= counterForce;
					}
				}
				if (Mathf.Sign(moveDir.z) == 1)
				{
					if (vel.z + moveDir.z > idealVel.z)
					{
						moveDir.z = 0;
					}
					else if (Mathf.Sign(vel.z) != 1)
					{
						//moveDir.z += counterForce;
					}
				}
				else
				{
					if (vel.z + moveDir.z < idealVel.z)
					{
						moveDir.z = 0;
					}
					else if (Mathf.Sign(vel.z) != -1)
					{
						//moveDir.z -= counterForce;
					}
				}

				//ApplyCounterForce(player.rb, slowDownForce);

				debugVect2 = moveDir;

				*/

/*
 * 
			//counter sliding
			Vector2 mag = velRelativeToLook();

			if (inputDir.x > 0 && mag.x >  maxSpeed) inputDir.x = 0;
			if (inputDir.x < 0 && mag.x < -maxSpeed) inputDir.x = 0;
			if (inputDir.y > 0 && mag.y >  maxSpeed) inputDir.y = 0;
			if (inputDir.y < 0 && mag.y < -maxSpeed) inputDir.y = 0;

			player.rb.AddForce(player.transform.forward * inputDir.y * acceleration);
			player.rb.AddForce(player.transform.right   * inputDir.x * acceleration);
 */
/*
 * //Debug vector
		Vector3 totalForce;
		public override void StateFixedUpdate()
		{
			//jump
			if (jump)
			{
				player.rb.AddForce(Vector3.up * jumpForce);
				player.ChangeState(inAir);
			}

			if(inputDir.sqrMagnitude > 0)
			{
				Vector3 moveDir = player.transform.forward * inputDir.y * acceleration + player.transform.right * inputDir.x * acceleration;
				Vector3 idealVel = moveDir.normalized * maxSpeed;
				Vector3 vel = player.rb.velocity;

				//cap the force so velocity stays below max
				if (Mathf.Sign(moveDir.x) == 1)
				{
					if (Mathf.Sign(vel.x) != 1)
					{
						moveDir.x += -vel.x * counterForce;
					}
					if (vel.x + moveDir.x > idealVel.x)
					{
						moveDir.x = Mathf.Clamp(idealVel.x - vel.x,0,float.PositiveInfinity);
					}
				}
				else
				{
					if (Mathf.Sign(vel.x) != -1)
					{
						moveDir.x += -vel.x * counterForce;
					}
					if (vel.x + moveDir.x < idealVel.x)
					{
						moveDir.x = Mathf.Clamp(idealVel.x - vel.x, float.NegativeInfinity, 0);
					}
					
				}
				if (Mathf.Sign(moveDir.z) == 1)
				{
					if (Mathf.Sign(vel.z) != 1)
					{
						moveDir.z += -vel.z * counterForce;
					}
					if (vel.z + moveDir.z > idealVel.z)
					{
						moveDir.z = Mathf.Clamp(idealVel.z - vel.z, 0, float.PositiveInfinity);
					}
					
				}
				else
				{
					if (Mathf.Sign(vel.z) != -1)
					{
						moveDir.z += -vel.z * counterForce;
					}
					if (vel.z + moveDir.z < idealVel.z)
					{
						moveDir.z = Mathf.Clamp(idealVel.z - vel.z, float.NegativeInfinity, 0);
					}
					
				}
				moveDir.y = 0;
				player.rb.AddForce(moveDir);


				//Counter Sliding and increase overall control
				vel.y = 0;
				idealVel.y = 0;
				float vectAngle = Vector3.Angle(vel, idealVel) / 180;
				Vector3 counterVel = -(vel - idealVel) * (vectAngle * vectAngle);
				
				counterVel.y = 0;
				player.rb.AddForce(counterVel * controlForce);

				totalForce = moveDir + counterVel;
			}
			else
			{
				ApplyCounterForce(player.rb, counterForce);
			}
			

		}

 
 
 
 
 */