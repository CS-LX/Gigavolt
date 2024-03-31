using Engine;
using Engine.Graphics;
using Engine.Media;

namespace Game {
    public class CameraGVElectricElement : RotateableGVElectricElement {
        public SubsystemDrawing m_subsystemDrawing;
        public SubsystemGameWidgets m_subsystemGameWidgets;
        public SubsystemGVCameraBlockBehavior m_subsystemGVCameraBlockBehavior;
        public GameWidget m_gameWidget;
        public GVCamera m_camera;
        public Vector3 m_originalPosition;
        public RenderTarget2D m_renderTarget;
        public bool m_complex;
        public uint m_inputIn;
        public uint m_inputTop;
        public uint m_inputRight;
        public uint m_inputBottom;
        public uint m_inputLeft;

        public static readonly Vector3[] m_upVector3 = [
            Vector3.UnitY,
            Vector3.UnitX,
            -Vector3.UnitY,
            -Vector3.UnitX,
            Vector3.UnitY,
            -Vector3.UnitZ,
            -Vector3.UnitY,
            Vector3.UnitZ,
            Vector3.UnitY,
            -Vector3.UnitX,
            -Vector3.UnitY,
            Vector3.UnitX,
            Vector3.UnitY,
            Vector3.UnitZ,
            -Vector3.UnitY,
            -Vector3.UnitZ,
            -Vector3.UnitZ,
            Vector3.UnitX,
            Vector3.UnitZ,
            -Vector3.UnitX,
            Vector3.UnitZ,
            Vector3.UnitX,
            -Vector3.UnitZ,
            -Vector3.UnitX
        ];

        public CameraGVElectricElement(SubsystemGVElectricity subsystemGVElectricity, CellFace cellFace) : base(subsystemGVElectricity, cellFace) {
            m_subsystemDrawing = SubsystemGVElectricity.Project.FindSubsystem<SubsystemDrawing>(true);
            m_subsystemGameWidgets = SubsystemGVElectricity.Project.FindSubsystem<SubsystemGameWidgets>(true);
            m_subsystemGVCameraBlockBehavior = SubsystemGVElectricity.Project.FindSubsystem<SubsystemGVCameraBlockBehavior>(true);
        }

        public override void OnAdded() {
            GVCellFace cellFace = CellFaces[0];
            int data = Terrain.ExtractData(SubsystemGVElectricity.SubsystemTerrain.Terrain.GetCellValue(cellFace.X, cellFace.Y, cellFace.Z));
            int mountingFace = cellFace.Face;
            int rotation = RotateableMountedGVElectricElementBlock.GetRotation(data);
            m_complex = GVDisplayLedBlock.GetComplex(data);
            m_originalPosition = new Vector3(cellFace.X + 0.5f, cellFace.Y + 0.5f, cellFace.Z + 0.5f);
            m_gameWidget = new GameWidget(new PlayerData(SubsystemGVElectricity.Project), 3);
            m_subsystemGameWidgets.m_gameWidgets.Add(m_gameWidget);
            m_subsystemGVCameraBlockBehavior.m_gameWidgets.Add(m_gameWidget);
            m_camera = new GVCamera(m_gameWidget);
            m_gameWidget.m_activeCamera = m_camera;
            m_gameWidget.m_cameras = [m_camera];
            Vector3 forward = CellFace.FaceToVector3(mountingFace);
            if (m_complex) {
                m_camera.SetupPerspectiveCamera(m_originalPosition - forward * 0.4f, -Vector3.UnitZ, Vector3.UnitY);
            }
            else {
                m_camera.SetupPerspectiveCamera(m_originalPosition - forward * 0.4f, forward, m_upVector3[mountingFace * 4 + rotation]);
            }
        }

        public override void OnRemoved() {
            m_subsystemGameWidgets.m_gameWidgets.Remove(m_gameWidget);
            m_subsystemGVCameraBlockBehavior.m_gameWidgets.Remove(m_gameWidget);
            m_gameWidget.Dispose();
            m_gameWidget = null;
            m_camera = null;
            Utilities.Dispose(ref m_renderTarget);
        }

        public override bool Simulate() {
            int electricRotation = Rotation;
            uint lastInputIn = m_inputIn;
            m_inputIn = 0u;
            GVArrayData data = null;
            if (m_complex) {
                uint lastInputTop = m_inputTop;
                uint lastInputRight = m_inputRight;
                uint lastInputBottom = m_inputBottom;
                uint lastInputLeft = m_inputLeft;
                m_inputTop = 0u;
                m_inputRight = 0u;
                m_inputBottom = 0u;
                m_inputLeft = 0u;
                foreach (GVElectricConnection connection in Connections) {
                    if (connection.ConnectorType != GVElectricConnectorType.Output
                        && connection.NeighborConnectorType != GVElectricConnectorType.Input) {
                        GVElectricConnectorDirection? connectorDirection = SubsystemGVElectricity.GetConnectorDirection(CellFaces[0].Face, electricRotation, connection.ConnectorFace);
                        if (connectorDirection.HasValue) {
                            if (connectorDirection.Value == GVElectricConnectorDirection.In) {
                                m_inputIn = connection.NeighborGVElectricElement.GetOutputVoltage(connection.NeighborConnectorFace);
                            }
                            else if (connectorDirection.Value == GVElectricConnectorDirection.Top) {
                                m_inputTop = connection.NeighborGVElectricElement.GetOutputVoltage(connection.NeighborConnectorFace);
                            }
                            else if (connectorDirection.Value == GVElectricConnectorDirection.Right) {
                                m_inputRight = connection.NeighborGVElectricElement.GetOutputVoltage(connection.NeighborConnectorFace);
                            }
                            else if (connectorDirection.Value == GVElectricConnectorDirection.Bottom) {
                                m_inputBottom = connection.NeighborGVElectricElement.GetOutputVoltage(connection.NeighborConnectorFace);
                            }
                            else if (connectorDirection.Value == GVElectricConnectorDirection.Left) {
                                m_inputLeft = connection.NeighborGVElectricElement.GetOutputVoltage(connection.NeighborConnectorFace);
                            }
                        }
                    }
                }
                bool changed = false;
                Vector3 newViewPosition = m_camera.ViewPosition;
                Vector3 newViewDirection = m_camera.ViewDirection;
                Vector3 newViewUp = m_camera.ViewUp;
                if (m_inputRight != lastInputRight) {
                    changed = true;
                    newViewPosition.X = m_originalPosition.X + (m_inputRight & 0x7FFFu) / (((m_inputRight >> 15) & 1u) == 1u ? -8f : 8f);
                    newViewPosition.Z = m_originalPosition.Z + ((m_inputRight >> 16) & 0x7FFFu) / (((m_inputRight >> 31) & 1u) == 1u ? -8f : 8f);
                }
                if (m_inputTop != lastInputTop) {
                    changed = true;
                    newViewPosition.Y = m_originalPosition.Y + ((m_inputTop >> 16) & 0x7FFFu) / (((m_inputTop >> 31) & 1u) == 1u ? -8f : 8f);
                    m_camera.m_viewAngel = MathUtils.DegToRad(m_inputTop & 0xFFu);
                }
                if (m_inputLeft != lastInputLeft) {
                    changed = true;
                    m_camera.m_viewSize.X = (int)(m_inputLeft & 0xFFFFu);
                    m_camera.m_viewSize.Y = (int)((m_inputLeft >> 16) & 0xFFFFu);
                }
                if (m_inputBottom != lastInputBottom) {
                    changed = true;
                    Quaternion quaternion = Quaternion.CreateFromYawPitchRoll((m_inputBottom & 0xFFu) * 0.017453292f * (((m_inputBottom >> 26) & 1u) == 1u ? -1f : 1f), ((m_inputBottom >> 8) & 0xFFu) * 0.017453292f * (((m_inputBottom >> 25) & 1u) == 1u ? -1f : 1f), ((m_inputBottom >> 16) & 0xFFu) * 0.017453292f * (((m_inputBottom >> 24) & 1u) == 1u ? -1f : 1f));
                    newViewDirection = quaternion.GetForwardVector();
                    newViewUp = quaternion.GetUpVector();
                }
                if (changed) {
                    m_camera.PrepareForDrawing();
                    m_camera.SetupPerspectiveCamera(newViewPosition, newViewDirection, newViewUp);
                    if (m_inputIn != lastInputIn) {
                        if (m_inputIn > 0) {
                            GVStaticStorage.GVMBIDDataDictionary.TryGetValue(m_inputIn, out data);
                        }
                    }
                }
            }
            else {
                foreach (GVElectricConnection connection in Connections) {
                    if (connection.ConnectorType != GVElectricConnectorType.Output
                        && connection.NeighborConnectorType != GVElectricConnectorType.Input) {
                        if (SubsystemGVElectricity.GetConnectorDirection(CellFaces[0].Face, electricRotation, connection.ConnectorFace).HasValue) {
                            m_inputIn = connection.NeighborGVElectricElement.GetOutputVoltage(connection.NeighborConnectorFace) | m_inputIn;
                        }
                    }
                }
                if (m_inputIn != lastInputIn) {
                    if (m_inputIn > 0) {
                        GVStaticStorage.GVMBIDDataDictionary.TryGetValue(m_inputIn, out data);
                    }
                }
            }
            if (data != null
                && m_camera.m_viewSize.X > 0
                && m_camera.m_viewSize.Y > 0) {
                RenderTarget2D lastRenderTarget = Display.RenderTarget;
                if (m_renderTarget == null
                    || m_renderTarget.Width != m_camera.m_viewSize.X
                    || m_renderTarget.Height != m_camera.m_viewSize.Y) {
                    Utilities.Dispose(ref m_renderTarget);
                    m_renderTarget = new RenderTarget2D(
                        m_camera.m_viewSize.X,
                        m_camera.m_viewSize.Y,
                        1,
                        ColorFormat.Rgba8888,
                        DepthFormat.Depth24Stencil8
                    );
                }
                Display.RenderTarget = m_renderTarget;
                Display.Clear(Color.Black, 1f, 0);
                try {
                    m_subsystemDrawing.Draw(m_camera);
                }
                finally {
                    Display.RenderTarget = lastRenderTarget;
                }
                Image image = new(m_renderTarget.Width, m_renderTarget.Height);
                m_renderTarget.GetData(image.Pixels, 0, new Rectangle(0, 0, m_renderTarget.Width, m_renderTarget.Height));
                data.Image2Data(image);
            }
            return false;
        }
    }

    public class GVGameWidget : GameWidget {
        public GVGameWidget() : base(null, -1) {
            m_activeCamera = new GVCamera(this);
            m_cameras = [m_activeCamera];
        }
    }

    public class GVCamera : BasePerspectiveCamera {
        public float m_viewAngel = MathUtils.PI / 2;
        public Point2 m_viewSize = new(1920, 1080);
        public override bool UsesMovementControls => false;

        public override bool IsEntityControlEnabled => false;

        public GVCamera(GameWidget gameWidget) : base(gameWidget) { }

        public void Activate() { }

        public void Activate(Vector3 viewPosition, Vector3 viewDirection, Vector3 viewUp) {
            SetupPerspectiveCamera(viewPosition, viewDirection, viewUp);
        }

        public override void Activate(Camera previousCamera) { }

        public override void Update(float dt) { }

        public static Matrix GVCalculateBaseProjectionMatrix(Vector2 wh, float viewAngle) {
            float num3 = wh.X / wh.Y;
            float num4 = MathUtils.Min(viewAngle * num3, viewAngle);
            float num5 = num4 * num3;
            if (num5 < 90f) {
                num4 *= 90f / num5;
            }
            else if (num5 > 175f) {
                num4 *= 175f / num5;
            }
            return Matrix.CreatePerspectiveFieldOfView(num4, num3, 0.1f, 2048f);
        }

        public override Matrix ProjectionMatrix {
            get {
                if (m_projectionMatrix == null) {
                    m_projectionMatrix = GVCalculateBaseProjectionMatrix(new Vector2(m_viewSize), m_viewAngel);
                    m_projectionMatrix *= CreateScaleTranslation(0.5f * m_viewSize.X, -0.5f * m_viewSize.Y, m_viewSize.X / 2f, m_viewSize.Y / 2f) * Matrix.Identity * CreateScaleTranslation(2f / m_viewSize.X, -2f / m_viewSize.Y, -1f, 1f);
                }
                return m_projectionMatrix.Value;
            }
        }

        public override Matrix ScreenProjectionMatrix {
            get {
                if (m_screenProjectionMatrix == null) {
                    Point2 size = Window.Size;
                    m_screenProjectionMatrix = GVCalculateBaseProjectionMatrix(new Vector2(m_viewSize), m_viewAngel) * CreateScaleTranslation(0.5f * m_viewSize.X, -0.5f * m_viewSize.Y, m_viewSize.X / 2f, m_viewSize.Y / 2f) * Matrix.Identity * CreateScaleTranslation(2f / m_viewSize.X, -2f / m_viewSize.Y, -1f, 1f);
                }
                return m_screenProjectionMatrix.Value;
            }
        }

        public override Vector2 ViewportSize {
            get {
                if (m_viewportSize == null) {
                    m_viewportSize = new Vector2(m_viewSize);
                }
                return m_viewportSize.Value;
            }
        }

        public static Matrix CreateScaleTranslation(float sx, float sy, float tx, float ty) => new(
            sx,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            sy,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            1f,
            0.0f,
            tx,
            ty,
            0.0f,
            1f
        );
    }
}