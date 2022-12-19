using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
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

        // activate the main menu panel
        ActivatePanel( PanelType.MainMenu );
    }

    public void EnterGamePlay()
    {
        Physics.autoSimulation = true;
        ActivatePanel( PanelType.GameplayHub );
    }
    public void ExitGamePlay()
    {
        Physics.autoSimulation = false;
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
