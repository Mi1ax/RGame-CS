using System.Numerics;
using Raylib_cs;

using static Raylib_cs.Raylib;
using static Raylib_cs.Rlgl;

namespace RGame.Core;

public class EditorCamera
{
    private float m_RotationSpeed;
    private float m_ZoomSpeed;
    private float m_Fov;

    private Matrix4x4 m_View;
    private Vector3 m_Position;
    private Vector3 m_FocalPoint;
    private Vector2 m_InitialMousePosition;

    private float m_Distance;
    private float m_Pitch;
    private float m_Yaw;

    public EditorCamera()
    {
        m_Fov = 45.0f;
        m_RotationSpeed = 0.8f;
        m_ZoomSpeed = 5f;

        m_FocalPoint = Vector3.Zero;
        m_InitialMousePosition = Vector2.Zero;
        
        m_Distance = 5.0f;
        m_Pitch = 0.0f;
        m_Yaw = 0.0f;
    }

    public ref float Fov => ref m_Fov;
    public ref float Distance => ref m_Distance;
    public ref float ZoomSpeed => ref m_ZoomSpeed;
    public ref float RotationSpeed => ref m_RotationSpeed;
    public ref Vector3 FocalPoint => ref m_FocalPoint;

    public void BeginDraw()
    {
        rlDrawRenderBatchActive();

        rlMatrixMode(MatrixMode.PROJECTION);
        rlPushMatrix();
        rlLoadIdentity();

        float aspect = (float)GetScreenWidth() / GetScreenHeight();

        double top = RL_CULL_DISTANCE_NEAR * MathF.Tan(m_Fov * 0.5f * DEG2RAD);
        double right = top * aspect;
        rlFrustum(-right, right, -top, top, RL_CULL_DISTANCE_NEAR, RL_CULL_DISTANCE_FAR);

        rlMatrixMode(MatrixMode.MODELVIEW);
        rlLoadIdentity();
        
        UpdateView();
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

    public void UpdateView()
    {
        // m_Yaw = m_Pitch = 0.0f; // Lock the camera's rotation
        m_Position = CalculatePosition();

        Quaternion orientation = Raymath.QuaternionFromEuler(-m_Pitch, -m_Yaw, 0.0f);
        m_View = Raymath.MatrixTranslate(m_Position.X, m_Position.Y, m_Position.Z) * Raymath.QuaternionToMatrix(orientation);
        m_View = Raymath.MatrixInvert(m_View);
    }

    public void Update(float dt)
    {
        if (IsKeyDown(KeyboardKey.KEY_LEFT_ALT))
        {
            var mousePosition = GetMousePosition();
            var delta = (mousePosition - m_InitialMousePosition) * 0.003f;
            m_InitialMousePosition = mousePosition;

            if (IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT))
                MousePan(delta);
            else if (IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
                MouseRotate(delta);
            else if (IsMouseButtonDown(MouseButton.MOUSE_BUTTON_MIDDLE))
                MouseZoom(delta.Y);

            MouseZoom(GetMouseWheelMove() * 0.1f);
        }

        UpdateView();
    }

    public void MousePan(Vector2 delta)
    {
        var speed = PanSpeed();
        m_FocalPoint += -GetRightDirection() * delta.X * speed.X * m_Distance;
        m_FocalPoint += GetUpDirection() * delta.Y * speed.Y * m_Distance;
    }

    public void MouseRotate(Vector2 delta)
    {
        float yawSign = GetUpDirection().Y < 0 ? -1.0f : 1.0f;
        m_Yaw += yawSign * delta.X * m_RotationSpeed;
        m_Pitch += delta.Y * m_RotationSpeed;
    }

    public void MouseZoom(float delta)
    {
        m_Distance -= delta * m_ZoomSpeed;
        if (m_Distance < 1.0f)
        {
            m_FocalPoint += GetForwardDirection();
            m_Distance = 1.0f;
        }
    }

    public Vector3 GetUpDirection()
    {
        return Raymath.Vector3RotateByQuaternion(Vector3.UnitY, Raymath.QuaternionFromEuler(-m_Pitch, -m_Yaw, 0.0f));
    }

    public Vector3 GetRightDirection()
    {
        return Raymath.Vector3RotateByQuaternion(Vector3.UnitX, Raymath.QuaternionFromEuler(-m_Pitch, -m_Yaw, 0.0f));
    }

    public Vector3 GetForwardDirection()
    {
        return Raymath.Vector3RotateByQuaternion(-Vector3.UnitZ, Raymath.QuaternionFromEuler(-m_Pitch, -m_Yaw, 0.0f));
    }

    public Vector3 CalculatePosition()
    {
        return m_FocalPoint - GetForwardDirection() * m_Distance;
    }

    public Vector2 PanSpeed()
    {
        float x = Math.Min(GetScreenWidth() / 1000.0f, 2.4f);
        float xFactor = 0.0366f * (x * x) - 0.1778f * x + 0.3021f;

        float y = Math.Min(GetScreenHeight() / 1000.0f, 2.4f); // max = 2.4f
        float yFactor = 0.0366f * (y * y) - 0.1778f * y + 0.3021f;

        return new Vector2(xFactor, yFactor);
    }
}