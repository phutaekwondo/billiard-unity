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
    public LineRenderer m_aimLineRenderer; // drag and drop the LineRenderer in the inspector

    public bool m_isEnabled = true;

    public float m_maxPower = 1.0f;
    public float m_powerIncreaseRate = 0.2f;
    private float m_power = 0.0f;

    private bool m_isBallsMoving = false;

    private ShootControllerState m_state = null;

    private void Start() {
        if ( m_state == null )
        {
            m_state = new ShootControllerState_Static( this );
        }
    }
    private void Update() {
        if ( !m_isEnabled ) return;

        m_isBallsMoving = IsBallsMoving_Calculate();

        // handle state input and update state
        m_state = m_state.HandleInput();
        // handle input
        HandleInput();

        // update the powerbar mask
        SetPowerbarMask( m_power / m_maxPower );
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

    public void Enable() 
    {
        m_isEnabled = true;
    }

    public void Disable()
    {
        m_isEnabled = false;
    }

    public void ShowAim()
    {
        m_aimLineRenderer.enabled = true;
    }

    public void HideAim()
    {
        m_aimLineRenderer.enabled = false;
    }

    public void IncreasePower()
    {
        m_power += m_powerIncreaseRate * m_maxPower * Time.deltaTime;
        if ( m_power > m_maxPower ) m_power = m_maxPower;
    }

    public void ResetPower()
    {
        m_power = 0.0f;
    }

    public bool IsBallsMoving()
    {
        return m_isBallsMoving;
    }

    private bool IsBallsMoving_Calculate()
    {
        float speedThreshord = 0.01f;

        //return true if cue ball is moving
        if ( m_cueBall.GetComponent<Rigidbody>().velocity.magnitude > speedThreshord )
        {
            return true;
        }

        // return true if any other ball is moving
        foreach ( Transform ball in m_rack.transform )
        {
            if ( ball.GetComponent<Rigidbody>().velocity.magnitude > speedThreshord )
            {
                return true;
            }
        }

        return false;
    }

    private void Shoot()
    {
        //get the vector from the aim point to the ball
        Vector3 direction = m_aimPoint.transform.position - m_cueBall.transform.position;
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
        protected ShootController m_shootController = null;

        //constructor
        public ShootControllerState(ShootController shootController)
        {
            m_shootController = shootController;
        }

        public abstract ShootControllerState HandleInput();
    }

    class ShootControllerState_Static : ShootControllerState
    {
        public ShootControllerState_Static(ShootController shootController) : base(shootController)
        {
            m_shootController.ShowAim();
        }

        public override ShootControllerState HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                return new ShootControllerState_IncreasePower(m_shootController);
            }
            return this;
        }
    }

    class ShootControllerState_IncreasePower : ShootControllerState
    {
        public ShootControllerState_IncreasePower(ShootController shootController) : base(shootController)
        {
            m_shootController.ShowAim();
        }

        public override ShootControllerState HandleInput()
        {
            if (Input.GetMouseButton(0))
            {
                m_shootController.IncreasePower();
                return this;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                m_shootController.Shoot();
                m_shootController.ResetPower();
                return new ShootControllerState_BallsMoving(m_shootController);
            }
            return this;
        }
    }

    class ShootControllerState_BallsMoving : ShootControllerState
    {
        public ShootControllerState_BallsMoving(ShootController shootController) : base(shootController)
        {
            shootController.HideAim();
        }

        public override ShootControllerState HandleInput()
        {
            if (!m_shootController.IsBallsMoving())
            {
                return new ShootControllerState_Static(m_shootController);
            }
            return this;
        }
    }
}
