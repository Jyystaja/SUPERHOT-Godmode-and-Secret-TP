using MelonLoader;
using UnityEngine;
using System;
using System.Reflection;

[assembly: MelonInfo(typeof(MyMelonMod.MyMod), "MyMod", "1.0.0", "Author")]
[assembly: MelonGame("SUPERHOT_Team", "SUPERHOT")]

namespace MyMelonMod
{
    public class MyMod : MelonMod
    {
        // Variables for GUI position and visibility
        private bool isGUIVisible = false;
        private Vector2 guiPosition = new Vector2(100, 100); // Default position
        private Vector2 guiOffset = Vector2.zero; // Offset for dragging
        private bool isDragging = false; // Check if the GUI is being dragged
        
        private bool godMode = false;
        private bool aimBot = false;

        // Called when the mod is loaded
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("MyMod Loaded!");
        }

        // Called every frame
        public override void OnUpdate()
        {
            // Toggle GUI visibility with F1 key
            if (Input.GetKeyDown(KeyCode.F1))
            {
                isGUIVisible = !isGUIVisible;

                // If the GUI is visible, unlock and show the cursor
                if (isGUIVisible)
                {
                    GameObject Canvas = GameObject.Find("_Omni/Canvas");
                    GameObject Player = GameObject.Find("_Player");
                    if(Canvas != null)
                    {
                        Canvas.GetComponent<SHGUI>().enabled = false;
                        Player.GetComponent<PlayerActions>().enabled = false;
                    }
                    else{
                        MelonLogger.Msg("Canvas Not Found!");
                    }
                    Cursor.lockState = CursorLockMode.None;  // Unfreeze the cursor
                    Cursor.visible = true; // Make the cursor visible
                }
                else
                {
                    GameObject Canvas = GameObject.Find("_Omni/Canvas");
                    GameObject Player = GameObject.Find("_Player");
                    if(Canvas != null)
                    {
                        Canvas.GetComponent<SHGUI>().enabled = true;
                        Player.GetComponent<PlayerActions>().enabled = true;
                    }
                    else{
                        MelonLogger.Msg("Canvas Not Found!");
                    }
                    Cursor.lockState = CursorLockMode.Locked;  // Unfreeze the cursor
                    Cursor.visible = false; // Hide the cursor
                }
            }

            // Prevent game interaction when GUI is visible
            if (isGUIVisible)
            {
                // Disable the game's click input or any other interactions
                HandleGameInput(false);
            }
            else
            {
                // Re-enable game interaction when the GUI is hidden
                HandleGameInput(true);
            }
        }

        // Function to enable/disable game input based on the GUI's state
        private void HandleGameInput(bool isEnabled)
        {
            // You can customize this method to disable or enable game controls
            // For example, if you want to disable mouse input:
            if (!isEnabled)
            {
                // Disable mouse click detection or other inputs here
                // This can be done using custom logic or hooks to prevent input processing
                // For now, we'll just show how to lock cursor for now
            }
        }

        // GUI logic using Unity's IMGUI
        public override void OnGUI()
        {
            if (!isGUIVisible)
                return;

            // Start the GUI window
            GUI.Window(0, new Rect(guiPosition.x, guiPosition.y, 600, 500), DrawWindow, "HOTHACKS");

            // If dragging, update the position of the GUI window
            if (isDragging)
            {
                guiPosition = Event.current.mousePosition - guiOffset;
            }
        }

        // Drawing the content of the GUI window
        private void DrawWindow(int windowID)
        {
            // Simple label and button inside the window
            GUILayout.Label("HOTHACKS");

            if (GUILayout.Button("Teleport to Secret"))
            {
                GameObject Player = GameObject.Find("_Player");
                GameObject Secret = GameObject.Find("Secret");
                if(Secret != null && Player != null)
                {
                    Player.transform.position = new Vector3(Secret.transform.position.x,Secret.transform.position.y + 3,Secret.transform.position.z);
                }
                
            }
            if (GUILayout.Button("GOD Mode"))
            {
                godMode = !godMode;
                GameObject Player = GameObject.Find("_Player");
                if(Player != null)
                {
                    if(godMode == true)
                    {
                        Player.GetComponent<PlayerActions>().tempImmortality = true;
                    }
                    else
                    {
                        Player.GetComponent<PlayerActions>().tempImmortality = false;
                    }
                }
                
            }
            if (GUILayout.Button("Aimbot"))
            {
                aimBot = !aimBot;
                GameObject Player = GameObject.Find("_Player");
                Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
                GameObject[] enemies = GameObject.FindObjectsOfType<GameObject>();

                if(Player != null)
                {
                    if(godMode == true)
                    {
                        Player.GetComponent<PlayerActions>().tempImmortality = true;
                    }
                    else
                    {
                        Player.GetComponent<PlayerActions>().tempImmortality = false;
                    }
                }
                
            }

            // Handle dragging logic
            if (Event.current.type == EventType.MouseDown && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                isDragging = true;
                guiOffset = Event.current.mousePosition - guiPosition;
                Event.current.Use();
            }

            if (Event.current.type == EventType.MouseUp)
            {
                isDragging = false;
            }

            // Make the window draggable by its title bar
            GUI.DragWindow();
        }
    }
}
