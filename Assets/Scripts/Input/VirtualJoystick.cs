using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WaterDropSurvival.Input
{
    /// <summary>
    /// Virtual joystick for mobile touch controls.
    /// Provides smooth directional input for player movement.
    /// </summary>
    public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Joystick Settings")]
        [SerializeField] private float handleRange = 50f;
        [SerializeField] private bool dynamicJoystick = false;
        
        [Header("Visual References")]
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;
        
        private Vector2 inputDirection = Vector2.zero;
        private Vector2 joystickPosition = Vector2.zero;
        private Canvas canvas;
        private Camera cam;
        
        public Vector2 InputDirection => inputDirection;
        
        private void Start()
        {
            canvas = GetComponentInParent<Canvas>();
            
            if (canvas == null)
            {
                Debug.LogError("VirtualJoystick must be a child of a Canvas!");
            }
            
            // Get camera for screen space calculations
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                cam = canvas.worldCamera;
            }
            
            // Store initial position
            joystickPosition = background.anchoredPosition;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (dynamicJoystick)
            {
                // Move joystick to touch position
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.GetComponent<RectTransform>(),
                    eventData.position,
                    cam,
                    out localPoint
                );
                background.anchoredPosition = localPoint;
            }
            
            OnDrag(eventData);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background,
                eventData.position,
                cam,
                out position
            );
            
            // Calculate normalized input direction
            position = Vector2.ClampMagnitude(position, handleRange);
            inputDirection = position / handleRange;
            
            // Update handle position
            handle.anchoredPosition = position;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            inputDirection = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            
            if (dynamicJoystick)
            {
                // Reset joystick to original position
                background.anchoredPosition = joystickPosition;
            }
        }
        
        // Editor testing with mouse
        private void Update()
        {
#if UNITY_EDITOR
            if (UnityEngine.Input.GetMouseButton(0))
            {
                Vector2 mousePos = UnityEngine.Input.mousePosition;
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    background,
                    mousePos,
                    cam,
                    out localPoint
                );
                
                localPoint = Vector2.ClampMagnitude(localPoint, handleRange);
                inputDirection = localPoint / handleRange;
                handle.anchoredPosition = localPoint;
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                inputDirection = Vector2.zero;
                handle.anchoredPosition = Vector2.zero;
            }
#endif
        }
    }
}
