using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameplayManager m_gameplayManager = null; // drag and drop the GameManager in the inspector

    // a public list of UI panels
    public List<GameObject> m_panels; // drag and drop the panels in the inspector


    public enum PanelType
    {
        MainMenu,
        Options,
        QuitComfirmation,
        GameplayHub,
    }

    private void Start() {
        if ( m_panels.Count != PanelType.GetNames( typeof( PanelType ) ).Length ) {
            throw new System.Exception( "Not Enough panel in m_panels" );
        }
        
        if (m_gameplayManager == null)
        {
            m_gameplayManager = FindObjectOfType<GameplayManager>();
        }

        // activate the main menu panel
        ActivatePanel( PanelType.MainMenu );
    }

    public void EnterGamePlay()
    {
        m_gameplayManager.EnterGamePlay();
        ActivatePanel( PanelType.GameplayHub );
    }
    public void ExitGamePlay()
    {
        m_gameplayManager.ExitGamePlay();
        ActivatePanel( PanelType.MainMenu );
    }

    public void DeactivateAllPanels()
    {
        // deactivate all panels
        foreach ( GameObject panel in m_panels )
        {
            panel.SetActive( false );
        }
    }

    public void ActivatePanel( PanelType panelType )
    {
        // deactivate all panels
        foreach ( GameObject panel in m_panels )
        {
            panel.SetActive( false );
        }

        // activate the panel that was passed in
        m_panels[(int)panelType].SetActive( true );
    }
}
