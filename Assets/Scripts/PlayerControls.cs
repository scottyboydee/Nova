using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private Controls controls;

    [SerializeField]
    private Player player;

    [SerializeField]
    private float xVelMax = 500f;

    [SerializeField]
    private float xAccel = 1f;

    [SerializeField]
    private float xDecel = 0.9f;

    [SerializeField]
    private float xVel;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void Update()
    {
        Move();

        CheckFire();
    }

    private void CheckFire()
    {
        if( controls.Gameplay.Fire.WasPressedThisFrame() )
        {
//            Debug.Log("Fire pressed!");
            player.Fire();
        }
    }

    private void Move()
    {
        // Check if MoveRight button is pressed
        if (controls.Gameplay.MoveRight.ReadValue<float>() > 0)
        {
            if (controls.Gameplay.MoveLeft.ReadValue<float>() == 0)
            {
                Accelerate(1);
            }
        }
        // Check if MoveLeft button is pressed
        else if (controls.Gameplay.MoveLeft.ReadValue<float>() > 0)
        {
            if (controls.Gameplay.MoveRight.ReadValue<float>() == 0)
            {
                Accelerate(-1);
            }
        }
        else
        {
            Decelerate();
        }

        player.MovePlayer(xVel);

    }

    private void Accelerate( int dir )
    {
        xVel += dir * xAccel;

        if( xVel > xVelMax )
            xVel = xVelMax;
        if( xVel < -xVelMax )
            xVel = -xVelMax;

//        Debug.Log("Moving: dir: " + dir + " speed: " + xVel);
    }

    private void Decelerate()
    {
        xVel *= xDecel;

        if (xVel != 0 && Mathf.Abs(xVel) < 1)
        {
//            Debug.Log("Stopped moving.");
            xVel = 0;
        }
    }
}
