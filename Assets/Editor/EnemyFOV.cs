using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyAI))]
public class EnemyFOV : Editor
{
    private void OnSceneGUI()
    {
        EnemyAI fov = (EnemyAI)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, fov.transform.forward, 360, fov.sightDistance);

        Vector3 viewAngle1 = DirectionFromAngle(fov.transform.localEulerAngles.y, -fov.fovAngle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(fov.transform.localEulerAngles.y, fov.fovAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle1 * fov.sightDistance);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle2 * fov.sightDistance);

        if (fov.seePlayer)
        {
            Handles.color = Color.red;
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle1 * fov.sightDistance);
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle2 * fov.sightDistance);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
