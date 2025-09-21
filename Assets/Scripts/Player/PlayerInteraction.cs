using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
   
        public float interactionRange = 3f;
        public LayerMask interactableLayer;

        private Camera playerCamera;
        private InteractableGlow currentHighlighted;

        void Start()
        {
            playerCamera = Camera.main;
        }

        void Update()
        {
            HighlightObjectInView();
        }

        void HighlightObjectInView()
        {
            // Turn off glow on previously highlighted object
            if (currentHighlighted != null)
            {
                currentHighlighted.SetGlow(false);
                currentHighlighted = null;
            }

            // Perform a raycast from center of screen
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
            {
                InteractableGlow glow = hit.collider.GetComponent<InteractableGlow>();

                if (glow != null)
                {
                    glow.SetGlow(true);
                    currentHighlighted = glow;
                }
            }
        }
    }


