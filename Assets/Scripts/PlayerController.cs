using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager m_gameManager = null; // drag and drop the GameManager in the inspector
    public GameplayManager m_gameplayManager; // drag GameplayManager into this field in the inspector
    public RefereeController m_refereeController; // drag RefereeController into this field in the inspector
    public ScreenManager m_screenManager; // drag ScreenManager into this field in the inspector

    protected ShootController m_shootController;
    protected CueBallPositionController m_cueBallPositionController;

    private PlayerControllerState m_state;
    private bool m_isEnabled = true;

    private void Start() 
    {
        m_shootController = GetComponent<ShootController>();
        m_shootController.m_OnShoot += OnShoot;
        m_cueBallPositionController = GetComponent<CueBallPositionController>();
        m_cueBallPositionController.m_OnChoosingPositionFinished += OnChoosingPositionFinished;
        m_gameplayManager.m_OnGameplayRestart += Restart;
        m_state = new PlayerControllerState_Aiming(this);
    }

    private void Update() 
    {
        if ( !m_isEnabled ) return;
        m_state = m_state.Update();

        HandleInput();
    }

    public void Restart()
    {
        m_state = new PlayerControllerState_Aiming(this);
    }

    public void Disable()
    {
        m_isEnabled = false;
        m_shootController.Disable();
        m_cueBallPositionController.Disable();
    }
    public void Enable()
    {
        m_isEnabled = true;
        m_state.SetEnablements();
    }

    private void HandleInput()
    {
        // if ESC is pressed, go to the main menu
        // if ( Input.GetKeyDown( KeyCode.Escape ) )
        // {   
        //     m_gameManager.PauseGameplay();
        // }
    }

    private void OnChoosingPositionFinished()
    {
        m_state = new PlayerControllerState_Aiming(this);
    }

    private void OnShoot()
    {
        //change state to waiting for balls stop
        m_state = new PlayerControllerState_WaitingForBallsStop(this);
    }

    abstract class PlayerControllerState
    {
        protected PlayerController m_playerController;
        protected ShootController m_shootController;
        protected CueBallPositionController m_cueBallPositionController;
        protected GameplayManager m_gameplayManager;
        protected RefereeController m_refereeController;

        public PlayerControllerState(PlayerController playerController)
        {
            m_playerController = playerController;
            m_shootController = playerController.m_shootController;
            m_cueBallPositionController = playerController.m_cueBallPositionController;
            m_gameplayManager = playerController.m_gameplayManager;
            m_refereeController = playerController.m_refereeController;
            SetEnablements();
        }
        public abstract void SetEnablements();
        public virtual PlayerControllerState Update()
        {
            if ( m_gameplayManager.IsBallsMoving() )
            {
                return new PlayerControllerState_WaitingForBallsStop(m_playerController);
            }
            else if ( m_refereeController.IsFoul() )
            {
                return new PlayerControllerState_ChoosingCueBallPosition(m_playerController);
            }
            else
            {
                return this;
            }
        }
    }

    class PlayerControllerState_Aiming : PlayerControllerState
    {
        public PlayerControllerState_Aiming(PlayerController playerController) : base(playerController)
        {
        }

        public override PlayerControllerState Update()
        {
            return base.Update();
        }

        public override void SetEnablements()
        {
            m_shootController.Enable();
            m_cueBallPositionController.Disable();
        }
    }

    class PlayerControllerState_WaitingForBallsStop : PlayerControllerState
    {
        public PlayerControllerState_WaitingForBallsStop(PlayerController playerController) : base(playerController)
        {
        }

        public override PlayerControllerState Update()
        {
            if ( !m_gameplayManager.IsBallsMoving() )
            {
                if ( m_refereeController.IsFoul() )
                {
                    return new PlayerControllerState_ChoosingCueBallPosition(m_playerController);
                }
                return new PlayerControllerState_Aiming(m_playerController);
            }
            return this;
        }
        public override void SetEnablements()
        {
            m_shootController.Disable();
            m_cueBallPositionController.Disable();
        }
    }

    class PlayerControllerState_ChoosingCueBallPosition : PlayerControllerState
    {
        public PlayerControllerState_ChoosingCueBallPosition(PlayerController playerController) : base(playerController)
        {
        }
        public override PlayerControllerState Update()
        {
            if ( m_gameplayManager.IsBallsMoving() )
            {
                return new PlayerControllerState_WaitingForBallsStop(m_playerController);
            }

            if ( !m_refereeController.IsFoul() )
            {
                return new PlayerControllerState_Aiming(m_playerController);
            }

            return this;
        }
        public override void SetEnablements()
        {
            m_shootController.Disable();
            m_cueBallPositionController.Enable();
        }
    }
}
