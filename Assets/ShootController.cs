using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootController : MonoBehaviour
{
    public Image m_powerbarMask; // drag and drop the PowerbarMask in the inspector
    public ScreenManager m_screenManager = null; // drag and drop the ScreenManager in the inspector
    public GameObject m_aimPoint; // drag and drop the AimPoint in the inspector
    public GameObject m_cueBall; // drag and drop the CueBall in the inspector
    public GameObject m_rack; // drag and drop the Rack in the inspector

    public float k_maxPower = 1.0f;
    private float m_power = 0.0f;

    private void Update() {
        SetPowerbarMask( m_power / k_maxPower );
        HandleInput();
    }
    private void HandleInput()
    {
        // if ESC is pressed, go to the main menu
        if ( Input.GetKeyDown( KeyCode.Escape ) )
        {
            if (m_screenManager)
            {
                m_screenManager.ExitGamePlay();
            }
        }
    }
    private void Shoot()
    {
        //get the vector from the aim point to the ball
        Vector3 direction = m_aimPoint.transform.position - transform.position;
        //normalize the vector
        direction.Normalize();
        //multiply the vector by the power
        direction *= m_power;
        //apply the force to the cue ball
        m_cueBall.GetComponent<Rigidbody>().AddForce( direction, ForceMode.Impulse );
    }
    private void SetPowerbarMask(float value)
    {
        if ( value > 1 ) value = 1;
        if ( value < 0 ) value = 0;
        m_powerbarMask.fillAmount = value;
    }


    abstract class ShootControllerState
    {
        ShootController m_shootController = null;

        //constructor
        public ShootControllerState(ShootController shootController, ScreenManager screenManager)
        {
            m_shootController = shootController;
        }

        public virtual void HandleInput(){}
    }

    class ShootControllerState_Static : ShootControllerState
    {
        public ShootControllerState_Static(ShootController shootController, ScreenManager screenManager) : base(shootController, screenManager)
        {
        }

        public override void HandleInput()
        {
        }
    }

    class ShootControllerState_IncreasePower : ShootControllerState
    {
        public ShootControllerState_IncreasePower(ShootController shootController, ScreenManager screenManager) : base(shootController, screenManager)
        {
        }

        public override void HandleInput()
        {
        }
    }

    class ShootControllerState_BallsMoving : ShootControllerState
    {
        public ShootControllerState_BallsMoving(ShootController shootController, ScreenManager screenManager) : base(shootController, screenManager)
        {
        }

        public override void HandleInput()
        {
        }
    }
}
