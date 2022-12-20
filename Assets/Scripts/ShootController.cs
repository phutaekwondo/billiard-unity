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
    public LineRenderer m_aimLineRenderer; // drag and drop the LineRenderer in the inspector
    public GameplayManager m_gameplayManager; // drag and drop the GameplayManager in the inspector

    private bool m_isEnabled = true;
    private bool m_isAbleToShoot = true;

    public float m_maxPower = 1.0f;
    public float m_powerIncreaseRate = 0.2f;
    private float m_power = 0.0f;

    private ShootControllerState m_state = null;

    private void Start() {
        if ( m_state == null )
        {
            m_state = new ShootControllerState_Static( this );
        }
    }
    private void Update() {
        if ( !m_isEnabled ) return;

        // handle state input and update state
        m_state = m_state.Update();

        // update aim visibility depend on the ability to shoot
        if ( m_isAbleToShoot )
        {
            ShowAim();
        }
        else
        {
            HideAim();
        }

        // update the powerbar mask
        SetPowerbarMask( m_power / m_maxPower );

        // handle input
        HandleInput();
    }
    private void HandleInput()
    {
        // if ESC is pressed, go to the main menu
        if ( Input.GetKeyDown( KeyCode.Escape ) )
        {
            if (m_screenManager)
            {
                m_screenManager.PauseGamePlay();
            }
        }
    }

    public bool IsAbleToShoot()
    {
        return m_isAbleToShoot;
    }

    public void SetAbleToShoot( bool isAbleToShoot )
    {
        m_isAbleToShoot = isAbleToShoot;
    }

    public bool IsEnable()
    {
        return m_isEnabled;
    }

    public void Enable() 
    {
        m_isEnabled = true;
    }

    public void Disable()
    {
        m_isEnabled = false;
        HideAim();
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
        if ( !m_isAbleToShoot ) return;

        m_power += m_powerIncreaseRate * m_maxPower * Time.deltaTime;
        if ( m_power > m_maxPower ) m_power = m_maxPower;
    }

    public void ResetPower()
    {
        m_power = 0.0f;
    }

    public bool IsBallsMoving()
    {
        return m_gameplayManager.IsBallsMoving();
    }

    private void Shoot()
    {
        if ( !m_isAbleToShoot ) return;

        //get the vector from the aim point to the ball
        Vector3 direction = m_aimPoint.transform.position - m_cueBall.transform.position;
        direction.y = 0;
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

        public abstract ShootControllerState Update();
    }

    class ShootControllerState_Static : ShootControllerState
    {
        public ShootControllerState_Static(ShootController shootController) : base(shootController)
        {
            m_shootController.SetAbleToShoot(true);
        }

        public override ShootControllerState Update()
        {
            //hanlde input
            if (Input.GetMouseButtonDown(0))
            {
                return new ShootControllerState_IncreasePower(m_shootController);
            }

            // if balls are moving, go to the balls moving state
            if (m_shootController.IsBallsMoving())
            {
                return new ShootControllerState_BallsMoving(m_shootController);
            }

            return this;
        }
    }

    class ShootControllerState_IncreasePower : ShootControllerState
    {
        public ShootControllerState_IncreasePower(ShootController shootController) : base(shootController)
        {
            m_shootController.SetAbleToShoot(true);
        }

        public override ShootControllerState Update()
        {
            //hanlde input
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

            //if the balls are moving, go to the balls moving state
            if (m_shootController.IsBallsMoving())
            {
                return new ShootControllerState_BallsMoving(m_shootController);
            }

            return this;
        }
    }

    class ShootControllerState_BallsMoving : ShootControllerState
    {
        public ShootControllerState_BallsMoving(ShootController shootController) : base(shootController)
        {
            m_shootController.SetAbleToShoot(false);
        }

        public override ShootControllerState Update()
        {
            if (!m_shootController.IsBallsMoving())
            {
                return new ShootControllerState_Static(m_shootController);
            }
            return this;
        }
    }
}
