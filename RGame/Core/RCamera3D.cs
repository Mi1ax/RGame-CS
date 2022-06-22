using System.Numerics;
using Raylib_cs;

using static Raylib_cs.Raylib;
using static Raylib_cs.Rlgl;

namespace RGame.Core;

public class RCamera3D
{
    private Camera3D m_Camera3D;
    private Vector2 m_CameraAngle;
    private Vector2 m_MousePrevPosition;
    private Matrix4x4 m_View;
    private float m_TargetDistance;

    private Vector3 m_Front;

    public ref Vector3 Position => ref m_Camera3D.position;
    public ref Vector2 Angle => ref m_CameraAngle;
    public ref Vector3 Target => ref m_Camera3D.target;
    public ref Vector3 Up => ref m_Camera3D.up;
    public ref float TargetDistance => ref m_TargetDistance;
    public ref float Fovy => ref m_Camera3D.fovy;

    public RCamera3D(
        Vector3? position = null, 
        Vector3? target = null, 
        Vector3? up = null, 
        float fovy = 45.0f, 
        CameraProjection cameraProjection = CameraProjection.CAMERA_PERSPECTIVE)
    {
        m_Camera3D = new Camera3D()
        {
            position = position == null ? new Vector3(0.0f, 0.0f, 3.0f) : position.Value,
            target = target == null ? new Vector3(0.0f, 0.0f, 0.0f) : target.Value,
            up = up == null ? new Vector3(0.0f, 1.0f, 0.0f) : up.Value,
            fovy = fovy,
            projection = cameraProjection
        };
        SetCameraMode(m_Camera3D, CameraMode.CAMERA_CUSTOM);
        m_CameraAngle = Vector2.Zero;
        m_TargetDistance = 10.0f;
        m_Front = new Vector3(0.0f, 0.0f, -1.0f);
    }

    private void CalculatePosition()
    {
        m_Camera3D.position.X = MathF.Sin(m_CameraAngle.X) * m_TargetDistance * MathF.Cos(m_CameraAngle.Y) + m_Camera3D.target.X;

        if (m_CameraAngle.Y <= 0.0f)
            m_Camera3D.position.Y = MathF.Sin(m_CameraAngle.Y) * m_TargetDistance * MathF.Sin(m_CameraAngle.Y) + m_Camera3D.target.Y;
        else
            m_Camera3D.position.Y = -MathF.Sin(m_CameraAngle.Y) * m_TargetDistance * MathF.Sin(m_CameraAngle.Y) + m_Camera3D.target.Y;

        m_Camera3D.position.Z = MathF.Cos(m_CameraAngle.X) * m_TargetDistance * MathF.Cos(m_CameraAngle.Y) + m_Camera3D.target.Z;
    }

    public void BeginDraw()
    {
        rlDrawRenderBatchActive();

        rlMatrixMode(MatrixMode.PROJECTION);
        rlPushMatrix();
        rlLoadIdentity();

        float aspect = (float)GetScreenWidth() / GetScreenHeight();

        double top = RL_CULL_DISTANCE_NEAR * MathF.Tan(Fovy * 0.5f * DEG2RAD);
        double right = top * aspect;
        rlFrustum(-right, right, -top, top, RL_CULL_DISTANCE_NEAR, RL_CULL_DISTANCE_FAR);

        rlMatrixMode(MatrixMode.MODELVIEW);
        rlLoadIdentity();

        //m_View = Raymath.MatrixLookAt(Position, Target, Up);
        m_View = Raymath.MatrixLookAt(Position, Position + m_Front, Up);
        rlMultMatrixf(m_View);

        rlEnableDepthTest();
    }

    public void EndDraw()
    {
        rlDrawRenderBatchActive();

        rlMatrixMode(MatrixMode.PROJECTION);
        rlPopMatrix();

        rlMatrixMode(MatrixMode.MODELVIEW);
        rlLoadIdentity();

        // Apply screen scaling if required
        //rlMultMatrixf(MatrixToFloat(screenScale)); 

        rlDisableDepthTest();
    }

    public void Update(float dt)
    {
        var mousePosition = GetMousePosition();
        var mousePositionDelta = mousePosition - m_MousePrevPosition;
        //CalculatePosition();

        if (IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT))
        {
            m_CameraAngle.X += mousePositionDelta.X * -0.3f * dt;
            m_CameraAngle.Y += mousePositionDelta.Y * -0.3f * dt;
        }

        if (GetMouseWheelMove() > 0)
        {
            m_TargetDistance -= 1.0f;
        }
        else if (GetMouseWheelMove() < 0)
        {
            m_TargetDistance += 1.0f;
        }

        if (IsKeyDown(KeyboardKey.KEY_W))
            Position += 10.0f * m_Front * dt;
        if (IsKeyDown(KeyboardKey.KEY_S))
            Position -= 10.0f * m_Front * dt;
        if (IsKeyDown(KeyboardKey.KEY_A))
            Position -= Raymath.Vector3Normalize(Raymath.Vector3CrossProduct(m_Front, Up)) * 10.0f * dt;
        if (IsKeyDown(KeyboardKey.KEY_D))
            Position += Raymath.Vector3Normalize(Raymath.Vector3CrossProduct(m_Front, Up)) * 10.0f * dt;

        m_MousePrevPosition = mousePosition;
    }

    public Camera3D GetCameraHandler() => m_Camera3D;
}