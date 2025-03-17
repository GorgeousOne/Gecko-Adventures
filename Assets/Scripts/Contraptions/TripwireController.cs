using UnityEngine;
using UnityEditor;

public class TripwireController : Trigger, IResettable  {
    public Transform startPoint;
    public Transform endPoint;
    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider;
    public AudioSource tripSound;
    private bool _savedIsIntact = true;
    private bool _isIntact = true;
    
    private void Start() {
       if (startPoint == null || endPoint == null) {
          Debug.LogError("Start point or end point is not assigned!");
          return;
       }
       // Set the line renderer to have 2 points
       lineRenderer.positionCount = 2;
       UpdateLinePositions();
    }
    
    public void UpdateLinePositions() {
       if (lineRenderer == null || startPoint == null || endPoint == null) {
          return;
       }
       // Update line positions
       lineRenderer.SetPosition(0, startPoint.position);
       lineRenderer.SetPosition(1, endPoint.position);
       
       // Update edge collider to match line renderer
       UpdateEdgeCollider();
    }
    
    private void UpdateEdgeCollider() {
        if (edgeCollider == null || startPoint == null || endPoint == null) {
            return;
        }
        
        // Convert world positions to local positions for the edge collider
        Vector2[] points = new Vector2[2];
        points[0] = transform.InverseTransformPoint(startPoint.position);
        points[1] = transform.InverseTransformPoint(endPoint.position);
        
        // Set the points to the edge collider
        edgeCollider.points = points;
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
       if (other.CompareTag("Player")) {
          Activate();
          tripSound.Play();
          edgeCollider.enabled = false;
          lineRenderer.enabled = false;
          _isIntact = false;
       }
    }

    public void SaveState() {
       _savedIsIntact = _isIntact;
    }

    public void ResetState() {
       if (!_isIntact && _savedIsIntact) {
          edgeCollider.enabled = true;
          lineRenderer.enabled = true;
       }
       _isIntact = _savedIsIntact;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TripwireController))]
public class LineEndpointsEditor : Editor {
    private TripwireController tripwireController;
    private Transform selectedPoint;
    private Vector3 lastParentPosition;
    private Quaternion lastParentRotation;

    private void OnEnable() {
       tripwireController = (TripwireController) target;
       if (tripwireController != null) {
          lastParentPosition = tripwireController.transform.position;
          lastParentRotation = tripwireController.transform.rotation;
          // Ensure SceneView repaints on selection
          SceneView.duringSceneGui += OnSceneGUIDelegate;
       }
    }

    private void OnDisable() {
       SceneView.duringSceneGui -= OnSceneGUIDelegate;
    }

    private void OnSceneGUIDelegate(SceneView sceneView) {
       // This will be called every frame in the Scene view
       if (tripwireController != null) {
          // Check if parent transform has changed
          if (tripwireController.transform.position != lastParentPosition ||
              tripwireController.transform.rotation != lastParentRotation) {
             lastParentPosition = tripwireController.transform.position;
             lastParentRotation = tripwireController.transform.rotation;
             tripwireController.UpdateLinePositions();
             SceneView.RepaintAll();
          }
       }
    }

    public override void OnInspectorGUI() {
       DrawDefaultInspector();
    }

    private void OnSceneGUI() {
       if (tripwireController == null || tripwireController.startPoint == null ||
           tripwireController.endPoint == null)
          return;

       // Draw handles for start and end points
       EditorGUI.BeginChangeCheck();
       Vector3 newStartPos = Handles.PositionHandle(tripwireController.startPoint.position, Quaternion.identity);
       if (EditorGUI.EndChangeCheck()) {
          Undo.RecordObject(tripwireController.startPoint, "Move Start Point");
          tripwireController.startPoint.position = newStartPos;
          tripwireController.UpdateLinePositions();
          EditorUtility.SetDirty(tripwireController.startPoint);
       }

       EditorGUI.BeginChangeCheck();
       Vector3 newEndPos = Handles.PositionHandle(tripwireController.endPoint.position, Quaternion.identity);
       if (EditorGUI.EndChangeCheck()) {
          Undo.RecordObject(tripwireController.endPoint, "Move End Point");
          tripwireController.endPoint.position = newEndPos;
          tripwireController.UpdateLinePositions();
          EditorUtility.SetDirty(tripwireController.endPoint);
       }

       // Force update when parent transform changes
       if (Event.current.type == EventType.MouseUp) {
          lastParentPosition = tripwireController.transform.position;
          lastParentRotation = tripwireController.transform.rotation;
          tripwireController.UpdateLinePositions();
       }
    }
}
#endif