using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Engine;
using Engine.Graphics;
using Engine.Input;
using GameEntitySystem;
using TemplatesDatabase;

// ReSharper disable RedundantExplicitArraySize

namespace Game {
    public class SubsystemGVElectricity : Subsystem, IUpdateable, IDrawable {
        public static GVElectricConnectionPath[] m_connectionPathsTable = new GVElectricConnectionPath[120] {
            new(
                0,
                1,
                -1,
                4,
                4,
                0
            ),
            new(
                0,
                1,
                0,
                0,
                4,
                5
            ),
            new(
                0,
                1,
                -1,
                2,
                4,
                5
            ),
            new(
                0,
                0,
                0,
                5,
                4,
                2
            ),
            new(
                -1,
                0,
                -1,
                3,
                3,
                0
            ),
            new(
                -1,
                0,
                0,
                0,
                3,
                1
            ),
            new(
                -1,
                0,
                -1,
                2,
                3,
                1
            ),
            new(
                0,
                0,
                0,
                1,
                3,
                2
            ),
            new(
                0,
                -1,
                -1,
                5,
                5,
                0
            ),
            new(
                0,
                -1,
                0,
                0,
                5,
                4
            ),
            new(
                0,
                -1,
                -1,
                2,
                5,
                4
            ),
            new(
                0,
                0,
                0,
                4,
                5,
                2
            ),
            new(
                1,
                0,
                -1,
                1,
                1,
                0
            ),
            new(
                1,
                0,
                0,
                0,
                1,
                3
            ),
            new(
                1,
                0,
                -1,
                2,
                1,
                3
            ),
            new(
                0,
                0,
                0,
                3,
                1,
                2
            ),
            new(
                0,
                0,
                -1,
                2,
                2,
                0
            ),
            null,
            null,
            null,
            new(
                -1,
                1,
                0,
                4,
                4,
                1
            ),
            new(
                0,
                1,
                0,
                1,
                4,
                5
            ),
            new(
                -1,
                1,
                0,
                3,
                4,
                5
            ),
            new(
                0,
                0,
                0,
                5,
                4,
                3
            ),
            new(
                -1,
                0,
                1,
                0,
                0,
                1
            ),
            new(
                0,
                0,
                1,
                1,
                0,
                2
            ),
            new(
                -1,
                0,
                1,
                3,
                0,
                2
            ),
            new(
                0,
                0,
                0,
                2,
                0,
                3
            ),
            new(
                -1,
                -1,
                0,
                5,
                5,
                1
            ),
            new(
                0,
                -1,
                0,
                1,
                5,
                4
            ),
            new(
                -1,
                -1,
                0,
                3,
                5,
                4
            ),
            new(
                0,
                0,
                0,
                4,
                5,
                3
            ),
            new(
                -1,
                0,
                -1,
                2,
                2,
                1
            ),
            new(
                0,
                0,
                -1,
                1,
                2,
                0
            ),
            new(
                -1,
                0,
                -1,
                3,
                2,
                0
            ),
            new(
                0,
                0,
                0,
                0,
                2,
                3
            ),
            new(
                -1,
                0,
                0,
                3,
                3,
                1
            ),
            null,
            null,
            null,
            new(
                0,
                1,
                1,
                4,
                4,
                2
            ),
            new(
                0,
                1,
                0,
                2,
                4,
                5
            ),
            new(
                0,
                1,
                1,
                0,
                4,
                5
            ),
            new(
                0,
                0,
                0,
                5,
                4,
                0
            ),
            new(
                1,
                0,
                1,
                1,
                1,
                2
            ),
            new(
                1,
                0,
                0,
                2,
                1,
                3
            ),
            new(
                1,
                0,
                1,
                0,
                1,
                3
            ),
            new(
                0,
                0,
                0,
                3,
                1,
                0
            ),
            new(
                0,
                -1,
                1,
                5,
                5,
                2
            ),
            new(
                0,
                -1,
                0,
                2,
                5,
                4
            ),
            new(
                0,
                -1,
                1,
                0,
                5,
                4
            ),
            new(
                0,
                0,
                0,
                4,
                5,
                0
            ),
            new(
                -1,
                0,
                1,
                3,
                3,
                2
            ),
            new(
                -1,
                0,
                0,
                2,
                3,
                1
            ),
            new(
                -1,
                0,
                1,
                0,
                3,
                1
            ),
            new(
                0,
                0,
                0,
                1,
                3,
                0
            ),
            new(
                0,
                0,
                1,
                0,
                0,
                2
            ),
            null,
            null,
            null,
            new(
                1,
                1,
                0,
                4,
                4,
                3
            ),
            new(
                0,
                1,
                0,
                3,
                4,
                5
            ),
            new(
                1,
                1,
                0,
                1,
                4,
                5
            ),
            new(
                0,
                0,
                0,
                5,
                4,
                1
            ),
            new(
                1,
                0,
                -1,
                2,
                2,
                3
            ),
            new(
                0,
                0,
                -1,
                3,
                2,
                0
            ),
            new(
                1,
                0,
                -1,
                1,
                2,
                0
            ),
            new(
                0,
                0,
                0,
                0,
                2,
                1
            ),
            new(
                1,
                -1,
                0,
                5,
                5,
                3
            ),
            new(
                0,
                -1,
                0,
                3,
                5,
                4
            ),
            new(
                1,
                -1,
                0,
                1,
                5,
                4
            ),
            new(
                0,
                0,
                0,
                4,
                5,
                1
            ),
            new(
                1,
                0,
                1,
                0,
                0,
                3
            ),
            new(
                0,
                0,
                1,
                3,
                0,
                2
            ),
            new(
                1,
                0,
                1,
                1,
                0,
                2
            ),
            new(
                0,
                0,
                0,
                2,
                0,
                1
            ),
            new(
                1,
                0,
                0,
                1,
                1,
                3
            ),
            null,
            null,
            null,
            new(
                0,
                -1,
                -1,
                2,
                2,
                4
            ),
            new(
                0,
                0,
                -1,
                4,
                2,
                0
            ),
            new(
                0,
                -1,
                -1,
                5,
                2,
                0
            ),
            new(
                0,
                0,
                0,
                0,
                2,
                5
            ),
            new(
                -1,
                -1,
                0,
                3,
                3,
                4
            ),
            new(
                -1,
                0,
                0,
                4,
                3,
                1
            ),
            new(
                -1,
                -1,
                0,
                5,
                3,
                1
            ),
            new(
                0,
                0,
                0,
                1,
                3,
                5
            ),
            new(
                0,
                -1,
                1,
                0,
                0,
                4
            ),
            new(
                0,
                0,
                1,
                4,
                0,
                2
            ),
            new(
                0,
                -1,
                1,
                5,
                0,
                2
            ),
            new(
                0,
                0,
                0,
                2,
                0,
                5
            ),
            new(
                1,
                -1,
                0,
                1,
                1,
                4
            ),
            new(
                1,
                0,
                0,
                4,
                1,
                3
            ),
            new(
                1,
                -1,
                0,
                5,
                1,
                3
            ),
            new(
                0,
                0,
                0,
                3,
                1,
                5
            ),
            new(
                0,
                -1,
                0,
                5,
                5,
                4
            ),
            null,
            null,
            null,
            new(
                0,
                1,
                -1,
                2,
                2,
                5
            ),
            new(
                0,
                0,
                -1,
                5,
                2,
                0
            ),
            new(
                0,
                1,
                -1,
                4,
                2,
                0
            ),
            new(
                0,
                0,
                0,
                0,
                2,
                4
            ),
            new(
                1,
                1,
                0,
                1,
                1,
                5
            ),
            new(
                1,
                0,
                0,
                5,
                1,
                3
            ),
            new(
                1,
                1,
                0,
                4,
                1,
                3
            ),
            new(
                0,
                0,
                0,
                3,
                1,
                4
            ),
            new(
                0,
                1,
                1,
                0,
                0,
                5
            ),
            new(
                0,
                0,
                1,
                5,
                0,
                2
            ),
            new(
                0,
                1,
                1,
                4,
                0,
                2
            ),
            new(
                0,
                0,
                0,
                2,
                0,
                4
            ),
            new(
                -1,
                1,
                0,
                3,
                3,
                5
            ),
            new(
                -1,
                0,
                0,
                5,
                3,
                1
            ),
            new(
                -1,
                1,
                0,
                4,
                3,
                1
            ),
            new(
                0,
                0,
                0,
                1,
                3,
                4
            ),
            new(
                0,
                1,
                0,
                4,
                4,
                5
            ),
            null,
            null,
            null
        };

        public static GVElectricConnectorDirection?[] m_connectorDirectionsTable = new GVElectricConnectorDirection?[36] {
            null,
            GVElectricConnectorDirection.Right,
            GVElectricConnectorDirection.In,
            GVElectricConnectorDirection.Left,
            GVElectricConnectorDirection.Top,
            GVElectricConnectorDirection.Bottom,
            GVElectricConnectorDirection.Left,
            null,
            GVElectricConnectorDirection.Right,
            GVElectricConnectorDirection.In,
            GVElectricConnectorDirection.Top,
            GVElectricConnectorDirection.Bottom,
            GVElectricConnectorDirection.In,
            GVElectricConnectorDirection.Left,
            null,
            GVElectricConnectorDirection.Right,
            GVElectricConnectorDirection.Top,
            GVElectricConnectorDirection.Bottom,
            GVElectricConnectorDirection.Right,
            GVElectricConnectorDirection.In,
            GVElectricConnectorDirection.Left,
            null,
            GVElectricConnectorDirection.Top,
            GVElectricConnectorDirection.Bottom,
            GVElectricConnectorDirection.Bottom,
            GVElectricConnectorDirection.Right,
            GVElectricConnectorDirection.Top,
            GVElectricConnectorDirection.Left,
            null,
            GVElectricConnectorDirection.In,
            GVElectricConnectorDirection.Top,
            GVElectricConnectorDirection.Right,
            GVElectricConnectorDirection.Bottom,
            GVElectricConnectorDirection.Left,
            GVElectricConnectorDirection.In,
            null
        };

        public static int[] m_connectorFacesTable = new int[30] {
            4,
            3,
            5,
            1,
            2,
            4,
            0,
            5,
            2,
            3,
            4,
            1,
            5,
            3,
            0,
            4,
            2,
            5,
            0,
            1,
            2,
            1,
            0,
            3,
            5,
            0,
            1,
            2,
            3,
            4
        };

        public float m_remainingSimulationTime;

        public Dictionary<Point3, uint> m_persistentElementsVoltages = new();
        public Dictionary<GVElectricElement, bool> m_GVElectricElements = new();
        public Dictionary<GVCellFace, GVElectricElement> m_GVElectricElementsByCellFace = new();
        public Dictionary<Point3, bool> m_pointsToUpdate = new();
        public Dictionary<Point3, GVElectricElement> m_GVElectricElementsToAdd = new();
        public Dictionary<GVElectricElement, bool> m_GVElectricElementsToRemove = new();
        public Dictionary<GVCellFace, bool> m_wiresToUpdate = new();
        public List<Dictionary<GVElectricElement, bool>> m_listsCache = new();
        public Dictionary<int, Dictionary<GVElectricElement, bool>> m_futureSimulateLists = new();
        public Dictionary<GVElectricElement, bool> m_nextStepSimulateList;
        public DynamicArray<GVElectricConnectionPath> m_tmpConnectionPaths = new();
        public Dictionary<GVCellFace, bool> m_tmpVisited = new();
        public Dictionary<GVCellFace, bool> m_tmpResult = new();

        public static int SimulatedGVElectricElements;

        public float CircuitStepDuration = 0.01f;
        public float SpeedFactor = 1f;

        public bool debugMode;
        public bool keyboardDebug = false;
        public Dictionary<ComponentPlayer, GVStepFloatingButtons> m_debugButtonsDictionary = new();
        public DateTime lastUpdate;
        public Queue<DateTime> last1000Updates = new(1002);
        public PrimitivesRenderer3D m_primitivesRenderer = new();
        public FlatBatch3D m_flatBatch;
        public TexturedBatch3D m_8NumberBatch;
        public List<Vector3> m_debugDrawPositions = [];

        public SubsystemTime SubsystemTime { get; set; }
        public SubsystemTerrain SubsystemTerrain { get; set; }

        public SubsystemAudio SubsystemAudio { get; set; }
        public SubsystemPlayers m_subsystemPlayers;
        public bool m_isCreativeMode;


        public int FrameStartCircuitStep { get; set; }

        public int CircuitStep { get; set; }

        public UpdateOrder UpdateOrder => UpdateOrder.Default;
        public static int[] m_drawOrders = [113];

        public static GVFurnitureBlock GVFurnitureBlock = new();

        public void OnGVElectricElementBlockGenerated(int x, int y, int z) {
            m_pointsToUpdate[new Point3(x, y, z)] = false;
        }

        public void OnGVElectricElementBlockAdded(int x, int y, int z) {
            m_pointsToUpdate[new Point3(x, y, z)] = true;
        }

        public void OnGVElectricElementBlockRemoved(int x, int y, int z) {
            m_pointsToUpdate[new Point3(x, y, z)] = true;
        }

        public void OnGVElectricElementBlockModified(int x, int y, int z) {
            m_pointsToUpdate[new Point3(x, y, z)] = true;
        }

        public void OnChunkDiscarding(TerrainChunk chunk) {
            foreach (GVCellFace key in m_GVElectricElementsByCellFace.Keys) {
                if (key.X >= chunk.Origin.X
                    && key.X < chunk.Origin.X + 16
                    && key.Z >= chunk.Origin.Y
                    && key.Z < chunk.Origin.Y + 16) {
                    m_pointsToUpdate[new Point3(key.X, key.Y, key.Z)] = false;
                }
            }
        }

        public static GVElectricConnectorDirection? GetConnectorDirection(int mountingFace, int rotation, int connectorFace) {
            GVElectricConnectorDirection? result = m_connectorDirectionsTable[6 * mountingFace + connectorFace];
            if (result.HasValue) {
                if (result.Value < GVElectricConnectorDirection.In) {
                    return (GVElectricConnectorDirection)((int)(result.Value + rotation) % 4);
                }
                return result;
            }
            return null;
        }

        public static int GetConnectorFace(int mountingFace, GVElectricConnectorDirection connectionDirection) => m_connectorFacesTable[(int)(5 * mountingFace + connectionDirection)];

        public void GetAllConnectedNeighbors(int x, int y, int z, int mountingFace, DynamicArray<GVElectricConnectionPath> list) {
            int cellValue = SubsystemTerrain.Terrain.GetCellValue(x, y, z);
            Block block = BlocksManager.Blocks[Terrain.ExtractContents(cellValue)];
            if (block is not IGVElectricElementBlock GVElectricElementBlock) {
                if (block is FurnitureBlock) {
                    GVElectricElementBlock = GVFurnitureBlock;
                }
                else {
                    return;
                }
            }
            IGVElectricWireElementBlock wireBlock = GVElectricElementBlock as IGVElectricWireElementBlock;
            for (GVElectricConnectorDirection GVElectricConnectorDirection = GVElectricConnectorDirection.Top; GVElectricConnectorDirection < (GVElectricConnectorDirection)5; GVElectricConnectorDirection++) {
                for (int i = 0; i < 4; i++) {
                    GVElectricConnectionPath GVElectricConnectionPath = m_connectionPathsTable[20 * mountingFace + 4 * (int)GVElectricConnectorDirection + i];
                    if (GVElectricConnectionPath == null) {
                        break;
                    }
                    GVElectricConnectorType? connectorType = GVElectricElementBlock.GetGVConnectorType(
                        SubsystemTerrain,
                        cellValue,
                        mountingFace,
                        GVElectricConnectionPath.ConnectorFace,
                        x,
                        y,
                        z
                    );
                    if (!connectorType.HasValue) {
                        break;
                    }
                    int x2 = x + GVElectricConnectionPath.NeighborOffsetX;
                    int y2 = y + GVElectricConnectionPath.NeighborOffsetY;
                    int z2 = z + GVElectricConnectionPath.NeighborOffsetZ;
                    int cellValue2 = SubsystemTerrain.Terrain.GetCellValue(x2, y2, z2);
                    Block block2 = BlocksManager.Blocks[Terrain.ExtractContents(cellValue2)];
                    if (block2 is not IGVElectricElementBlock GVElectricElementBlock2) {
                        if (block2 is FurnitureBlock) {
                            GVElectricElementBlock2 = GVFurnitureBlock;
                        }
                        else {
                            continue;
                        }
                    }
                    IGVElectricWireElementBlock wireBlock2 = GVElectricElementBlock2 as IGVElectricWireElementBlock;
                    GVElectricConnectorType? connectorType2 = GVElectricElementBlock2?.GetGVConnectorType(
                        SubsystemTerrain,
                        cellValue2,
                        GVElectricConnectionPath.NeighborFace,
                        GVElectricConnectionPath.NeighborConnectorFace,
                        x2,
                        y2,
                        z2
                    );
                    if (connectorType2.HasValue
                        && ((connectorType.Value != 0 && connectorType2.Value != GVElectricConnectorType.Output) || (connectorType.Value != GVElectricConnectorType.Output && connectorType2.Value != 0))) {
                        int connectionMask = GVElectricElementBlock.GetConnectionMask(cellValue);
                        int connectionMask2 = GVElectricElementBlock2.GetConnectionMask(cellValue2);
                        if (connectionMask2 == int.MaxValue
                            && wireBlock != null
                            && wireBlock.IsWireHarness(cellValue)
                            && (wireBlock2 == null || !wireBlock2.IsWireHarness(cellValue2))) {
                            continue;
                        }
                        if (connectionMask == int.MaxValue
                            && wireBlock2 != null
                            && wireBlock2.IsWireHarness(cellValue2)
                            && (wireBlock == null || !wireBlock.IsWireHarness(cellValue))) {
                            continue;
                        }
                        if ((connectionMask & connectionMask2) != 0) {
                            list.Add(GVElectricConnectionPath);
                        }
                    }
                }
            }
        }

        public GVElectricElement GetGVElectricElement(int x, int y, int z, int mountingFace, int mask = int.MaxValue) {
            m_GVElectricElementsByCellFace.TryGetValue(
                new GVCellFace(
                    x,
                    y,
                    z,
                    mountingFace,
                    mask
                ),
                out GVElectricElement value
            );
            if (value == null) {
                if (mask == int.MaxValue) {
                    for (int i = 0; i < 16; i++) {
                        m_GVElectricElementsByCellFace.TryGetValue(
                            new GVCellFace(
                                x,
                                y,
                                z,
                                mountingFace,
                                1 << i
                            ),
                            out GVElectricElement value2
                        );
                        if (value2 != null) {
                            return value2;
                        }
                    }
                }
                return null;
            }
            return value;
        }

        public void QueueGVElectricElementForSimulation(GVElectricElement GVElectricElement, int circuitStep) {
            if (circuitStep == CircuitStep + 1) {
                if (m_nextStepSimulateList == null
                    && !m_futureSimulateLists.TryGetValue(CircuitStep + 1, out m_nextStepSimulateList)) {
                    m_nextStepSimulateList = GetListFromCache();
                    m_futureSimulateLists.Add(CircuitStep + 1, m_nextStepSimulateList);
                }
                m_nextStepSimulateList[GVElectricElement] = true;
            }
            else if (circuitStep > CircuitStep + 1) {
                if (!m_futureSimulateLists.TryGetValue(circuitStep, out Dictionary<GVElectricElement, bool> value)) {
                    value = GetListFromCache();
                    m_futureSimulateLists.Add(circuitStep, value);
                }
                value[GVElectricElement] = true;
            }
        }

        public void QueueGVElectricElementConnectionsForSimulation(GVElectricElement GVElectricElement, int circuitStep) {
            foreach (GVElectricConnection connection in GVElectricElement.Connections) {
                if (connection.ConnectorType != 0
                    && connection.NeighborConnectorType != GVElectricConnectorType.Output) {
                    QueueGVElectricElementForSimulation(connection.NeighborGVElectricElement, circuitStep);
                }
            }
        }

        public uint? ReadPersistentVoltage(Point3 point) {
            if (m_persistentElementsVoltages.TryGetValue(point, out uint value)) {
                return value;
            }
            return null;
        }

        public void WritePersistentVoltage(Point3 point, uint voltage) {
            m_persistentElementsVoltages[point] = voltage;
        }

        public void Update(float dt) {
            try {
                if (keyboardDebug) {
                    if (Keyboard.IsKeyDownOnce(Key.F5)) {
                        debugMode = !debugMode;
                        if (debugMode) {
                            last1000Updates.Clear();
                        }
                    }
                    if (debugMode) {
                        if (Keyboard.IsKeyDownOnce(Key.F6)) {
                            StepUpdate();
                        }
                        else if (Keyboard.IsKeyDownOnce(Key.F7)) {
                            JumpUpdate();
                        }
                    }
                }
                if (!debugMode) {
                    StepUpdateSkip();
                    FrameStartCircuitStep = CircuitStep;
                    m_debugDrawPositions.Clear();
                    SimulatedGVElectricElements = 0;
                    m_remainingSimulationTime = MathUtils.Min(m_remainingSimulationTime + dt, 0.1f);
                    while (m_remainingSimulationTime >= CircuitStepDuration) {
                        UpdateGVElectricElements();
                        m_remainingSimulationTime -= CircuitStepDuration;
                        m_nextStepSimulateList = null;
                        if (m_futureSimulateLists.TryGetValue(++CircuitStep, out Dictionary<GVElectricElement, bool> value)) {
                            m_futureSimulateLists.Remove(CircuitStep);
                            SimulatedGVElectricElements += value.Count;
                            foreach (GVElectricElement key in value.Keys) {
                                if (m_GVElectricElements.ContainsKey(key)) {
                                    SimulateGVElectricElement(key);
                                }
                            }
                            ReturnListToCache(value);
                        }
                        while (last1000Updates.Count >= 1001) {
                            last1000Updates.Dequeue();
                        }
                        DateTime now = DateTime.Now;
                        lastUpdate = now;
                        last1000Updates.Enqueue(now);
                    }
                }
                if (m_isCreativeMode) {
                    foreach (ComponentPlayer componentPlayer in m_subsystemPlayers.ComponentPlayers) {
                        if (componentPlayer.ComponentGui.ModalPanelWidget is CreativeInventoryWidget widget) {
                            foreach (CreativeInventoryWidget.Category c in widget.m_categories) {
                                if (c.Name.StartsWith("GV ")) {
                                    if (c.Color.B == 243) {
                                        break;
                                    }
                                    if (c.Name.EndsWith("Expand")
                                        || c.Name.EndsWith("Multiple")) {
                                        c.Color = new Color(233, 85, 227);
                                    }
                                    else {
                                        c.Color = new Color(30, 213, 243);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e) {
                Log.Error(e);
            }
        }

        public void JumpUpdate() {
            if (debugMode) {
                StepUpdateSkip();
                FrameStartCircuitStep = CircuitStep;
                m_debugDrawPositions.Clear();
                SimulatedGVElectricElements = 0;
                UpdateGVElectricElements();
                m_nextStepSimulateList = null;
                if (m_futureSimulateLists.TryGetValue(++CircuitStep, out Dictionary<GVElectricElement, bool> value)) {
                    m_futureSimulateLists.Remove(CircuitStep);
                    SimulatedGVElectricElements += value.Count;
                    foreach (GVElectricElement key in value.Keys) {
                        if (m_GVElectricElements.ContainsKey(key)) {
                            SimulateGVElectricElement(key);
                            AddElement2DebugDraw(key);
                        }
                    }
                    ReturnListToCache(value);
                }
                while (last1000Updates.Count >= 1001) {
                    last1000Updates.Dequeue();
                }
                lastUpdate = DateTime.Now;
                last1000Updates.Clear();
            }
        }

        public bool inStepping;
        public List<GVElectricElement> steppingElements;
        public int steppingIndex;
        Dictionary<GVElectricElement, bool> lastWhat;

        public void StepUpdate() {
            if (debugMode) {
                m_debugDrawPositions.Clear();
                if (inStepping) {
                    StepUpdateRun();
                }
                else {
                    StepUpdateInitiate();
                }
                lastUpdate = DateTime.Now;
                last1000Updates.Clear();
            }
        }

        public void StepUpdateInitiate() {
            FrameStartCircuitStep = CircuitStep;
            SimulatedGVElectricElements = 0;
            UpdateGVElectricElements();
            m_nextStepSimulateList = null;
            if (m_futureSimulateLists.TryGetValue(++CircuitStep, out Dictionary<GVElectricElement, bool> value)) {
                if (lastWhat != null) {
                    ReturnListToCache(lastWhat);
                }
                m_futureSimulateLists.Remove(CircuitStep);
                SimulatedGVElectricElements += value.Count;
                steppingElements = value.Keys.ToList();
                steppingIndex = 0;
                if (steppingElements.Count > 0) {
                    inStepping = true;
                    StepUpdateRun();
                }
                lastWhat = value;
            }
        }

        public void StepUpdateRun() {
            GVElectricElement nowElement = steppingElements[steppingIndex++];
            if (m_GVElectricElements.ContainsKey(nowElement)) {
                SimulateGVElectricElement(nowElement);
                AddElement2DebugDraw(nowElement);
            }
            if (steppingElements.Count <= steppingIndex) {
                inStepping = false;
            }
        }

        public void StepUpdateSkip() {
            while (inStepping) {
                StepUpdateRun();
            }
        }

        public void AddElement2DebugDraw(GVElectricElement element) {
            GVCellFace cellFace = element.CellFaces[0];
            int face = cellFace.Face;
            Point3 point = cellFace.Point;
            if (element is MountedGVElectricElement) {
                Vector3 faceDirection = GVCellFace.FaceToVector3(face);
                m_debugDrawPositions.Add(new Vector3(point.X - faceDirection.X * 0.48f + 0.5f, point.Y - faceDirection.Y * 0.48f + 0.5f, point.Z - faceDirection.Z * 0.48f + 0.5f));
            }
            else {
                m_debugDrawPositions.Add(new Vector3(point.X + 0.5f, point.Y + 0.5f, point.Z + 0.5f));
            }
        }

        public override void Load(ValuesDictionary valuesDictionary) {
            try {
                SubsystemTerrain = Project.FindSubsystem<SubsystemTerrain>(true);
                SubsystemTime = Project.FindSubsystem<SubsystemTime>(true);
                SubsystemAudio = Project.FindSubsystem<SubsystemAudio>(true);
                m_isCreativeMode = Project.FindSubsystem<SubsystemGameInfo>(true).WorldSettings.GameMode == GameMode.Creative;
                if (m_isCreativeMode) {
                    m_subsystemPlayers = Project.FindSubsystem<SubsystemPlayers>(true);
                }
                m_flatBatch = m_primitivesRenderer.FlatBatch(0, DepthStencilState.DepthRead, null, BlendState.Additive);
                string[] array = valuesDictionary.GetValue<string>("GigaVoltagesByCell").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                int num = 0;
                while (true) {
                    if (num < array.Length) {
                        string[] array2 = array[num].Split(new[] { "," }, StringSplitOptions.None);
                        if (array2.Length != 4) {
                            break;
                        }
                        int x = int.Parse(array2[0], NumberStyles.HexNumber, null);
                        int y = int.Parse(array2[1], NumberStyles.HexNumber, null);
                        int z = int.Parse(array2[2], NumberStyles.HexNumber, null);
                        uint value = uint.Parse(array2[3], NumberStyles.HexNumber, null);
                        m_persistentElementsVoltages[new Point3(x, y, z)] = value;
                        num++;
                        continue;
                    }
                    return;
                }
                throw new InvalidOperationException("Invalid number of tokens.");
            }
            catch (Exception e) {
                Log.Error(e);
            }
        }

        public override void Save(ValuesDictionary valuesDictionary) {
            StringBuilder stringBuilder = new();
            foreach (KeyValuePair<Point3, uint> persistentElementsVoltage in m_persistentElementsVoltages) {
                stringBuilder.Append(persistentElementsVoltage.Key.X.ToString("X", null));
                stringBuilder.Append(',');
                stringBuilder.Append(persistentElementsVoltage.Key.Y.ToString("X", null));
                stringBuilder.Append(',');
                stringBuilder.Append(persistentElementsVoltage.Key.Z.ToString("X", null));
                stringBuilder.Append(',');
                stringBuilder.Append(persistentElementsVoltage.Value.ToString("X", null));
                stringBuilder.Append(';');
            }
            valuesDictionary.SetValue("GigaVoltagesByCell", stringBuilder.ToString());
        }

        public static GVElectricConnectionPath GetConnectionPath(int mountingFace, GVElectricConnectorDirection localConnector, int neighborIndex) => m_connectionPathsTable[16 * mountingFace + 4 * (int)localConnector + neighborIndex];

        public void SimulateGVElectricElement(GVElectricElement GVElectricElement) {
            if (GVElectricElement.Simulate()) {
                QueueGVElectricElementConnectionsForSimulation(GVElectricElement, CircuitStep + 1);
            }
        }

        public void AddGVElectricElement(GVElectricElement GVElectricElement) {
            if (GVElectricElement.CellFaces.Intersect(m_GVElectricElementsByCellFace.Keys).Any()) {
                return;
            }
            m_GVElectricElements.Add(GVElectricElement, true);
            foreach (GVCellFace cellFace2 in GVElectricElement.CellFaces) {
                m_GVElectricElementsByCellFace.Add(cellFace2, GVElectricElement);
                m_tmpConnectionPaths.Clear();
                GetAllConnectedNeighbors(
                    cellFace2.X,
                    cellFace2.Y,
                    cellFace2.Z,
                    cellFace2.Face,
                    m_tmpConnectionPaths
                );
                foreach (GVElectricConnectionPath tmpConnectionPath in m_tmpConnectionPaths) {
                    GVCellFace cellFace = new(cellFace2.X + tmpConnectionPath.NeighborOffsetX, cellFace2.Y + tmpConnectionPath.NeighborOffsetY, cellFace2.Z + tmpConnectionPath.NeighborOffsetZ, tmpConnectionPath.NeighborFace);
                    GVElectricElement value = GetGVElectricElement(
                        cellFace.X,
                        cellFace.Y,
                        cellFace.Z,
                        cellFace.Face,
                        cellFace2.Mask
                    );
                    if (value == null) {
                        value = GetGVElectricElement(cellFace.X, cellFace.Y, cellFace.Z, cellFace.Face);
                        if (value == null
                            || value == GVElectricElement
                            || value.CellFaces[0].Mask != int.MaxValue) {
                            continue;
                        }
                    }
                    if (value == GVElectricElement
                        || (GVElectricElement is WireDomainGVElectricElement && value is WireDomainGVElectricElement)) {
                        continue;
                    }
                    int cellValue = SubsystemTerrain.Terrain.GetCellValue(cellFace2.X, cellFace2.Y, cellFace2.Z);
                    Block block = BlocksManager.Blocks[Terrain.ExtractContents(cellValue)];
                    if (block is not IGVElectricElementBlock GVElectricElementBlock) {
                        if (block is FurnitureBlock) {
                            GVElectricElementBlock = GVFurnitureBlock;
                        }
                        else {
                            continue;
                        }
                    }
                    GVElectricConnectorType? value2 = GVElectricElementBlock.GetGVConnectorType(
                        SubsystemTerrain,
                        cellValue,
                        cellFace2.Face,
                        tmpConnectionPath.ConnectorFace,
                        cellFace2.X,
                        cellFace2.Y,
                        cellFace2.Z
                    );
                    if (!value2.HasValue) {
                        continue;
                    }
                    int cellValue2 = SubsystemTerrain.Terrain.GetCellValue(cellFace.X, cellFace.Y, cellFace.Z);
                    Block block2 = BlocksManager.Blocks[Terrain.ExtractContents(cellValue2)];
                    if (block2 is not IGVElectricElementBlock GVElectricElementBlock2) {
                        if (block2 is FurnitureBlock) {
                            GVElectricElementBlock2 = GVFurnitureBlock;
                        }
                        else {
                            continue;
                        }
                    }
                    GVElectricConnectorType? value3 = GVElectricElementBlock2.GetGVConnectorType(
                        SubsystemTerrain,
                        cellValue2,
                        cellFace.Face,
                        tmpConnectionPath.NeighborConnectorFace,
                        cellFace.X,
                        cellFace.Y,
                        cellFace.Z
                    );
                    if (!value3.HasValue) {
                        continue;
                    }
                    GVElectricElement.Connections.Add(
                        new GVElectricConnection {
                            CellFace = cellFace2,
                            ConnectorFace = tmpConnectionPath.ConnectorFace,
                            ConnectorType = value2.Value,
                            NeighborGVElectricElement = value,
                            NeighborCellFace = cellFace,
                            NeighborConnectorFace = tmpConnectionPath.NeighborConnectorFace,
                            NeighborConnectorType = value3.Value
                        }
                    );
                    value.Connections.Add(
                        new GVElectricConnection {
                            CellFace = cellFace,
                            ConnectorFace = tmpConnectionPath.NeighborConnectorFace,
                            ConnectorType = value3.Value,
                            NeighborGVElectricElement = GVElectricElement,
                            NeighborCellFace = cellFace2,
                            NeighborConnectorFace = tmpConnectionPath.ConnectorFace,
                            NeighborConnectorType = value2.Value
                        }
                    );
                }
            }
            QueueGVElectricElementForSimulation(GVElectricElement, CircuitStep + 1);
            QueueGVElectricElementConnectionsForSimulation(GVElectricElement, CircuitStep + 2);
            GVElectricElement.OnAdded();
        }

        public void RemoveGVElectricElement(GVElectricElement GVElectricElement) {
            GVElectricElement.OnRemoved();
            QueueGVElectricElementConnectionsForSimulation(GVElectricElement, CircuitStep + 1);
            m_GVElectricElements.Remove(GVElectricElement);
            foreach (GVCellFace cellFace in GVElectricElement.CellFaces) {
                m_GVElectricElementsByCellFace.Remove(cellFace);
            }
            foreach (GVElectricConnection connection in GVElectricElement.Connections) {
                connection.NeighborGVElectricElement.Connections.RemoveAll(connection2 => connection2.NeighborGVElectricElement == GVElectricElement);
            }
        }

        public void UpdateGVElectricElements() {
            foreach (KeyValuePair<Point3, bool> item in m_pointsToUpdate) {
                Point3 key = item.Key;
                int cellValue = SubsystemTerrain.Terrain.GetCellValue(key.X, key.Y, key.Z);
                for (int i = 0; i < 6; i++) {
                    GVElectricElement GVElectricElement = GetGVElectricElement(key.X, key.Y, key.Z, i);
                    if (GVElectricElement != null) {
                        if (GVElectricElement is WireDomainGVElectricElement) {
                            m_wiresToUpdate[new GVCellFace(key) { Mask = GVElectricElement.CellFaces[0].Mask }] = true;
                        }
                        else {
                            m_GVElectricElementsToRemove[GVElectricElement] = true;
                        }
                    }
                }
                if (item.Value) {
                    m_persistentElementsVoltages.Remove(key);
                }
                int num = Terrain.ExtractContents(cellValue);
                Block block = BlocksManager.Blocks[num];
                if (block is IGVElectricWireElementBlock wire) {
                    m_wiresToUpdate[new GVCellFace(key) { Mask = wire.GetConnectionMask(cellValue) }] = true;
                }
                else {
                    IGVElectricElementBlock GVElectricElementBlock = block as IGVElectricElementBlock;
                    if (GVElectricElementBlock == null) {
                        if (block is FurnitureBlock) {
                            GVElectricElementBlock = GVFurnitureBlock;
                        }
                    }
                    GVElectricElement GVElectricElement2 = GVElectricElementBlock?.CreateGVElectricElement(
                        this,
                        cellValue,
                        key.X,
                        key.Y,
                        key.Z
                    );
                    if (GVElectricElement2 != null) {
                        m_GVElectricElementsToAdd[key] = GVElectricElement2;
                    }
                }
            }
            RemoveWireDomains();
            foreach (KeyValuePair<GVElectricElement, bool> item2 in m_GVElectricElementsToRemove) {
                RemoveGVElectricElement(item2.Key);
            }
            AddWireDomains();
            foreach (GVElectricElement value in m_GVElectricElementsToAdd.Values) {
                AddGVElectricElement(value);
            }
            m_pointsToUpdate.Clear();
            m_wiresToUpdate.Clear();
            m_GVElectricElementsToAdd.Clear();
            m_GVElectricElementsToRemove.Clear();
        }

        public void AddWireDomains() {
            m_tmpVisited.Clear();
            foreach (GVCellFace key in m_wiresToUpdate.Keys) {
                for (int i = key.X - 1; i <= key.X + 1; i++) {
                    for (int j = key.Y - 1; j <= key.Y + 1; j++) {
                        for (int k = key.Z - 1; k <= key.Z + 1; k++) {
                            for (int l = 0; l < 6; l++) {
                                for (int m = 0; m < 16; m++) {
                                    m_tmpResult.Clear();
                                    ScanWireDomain(
                                        new GVCellFace(
                                            i,
                                            j,
                                            k,
                                            l,
                                            1 << m
                                        ),
                                        m_tmpVisited,
                                        m_tmpResult
                                    );
                                    if (m_tmpResult.Count > 0) {
                                        WireDomainGVElectricElement GVElectricElement = new(this, m_tmpResult.Keys);
                                        AddGVElectricElement(GVElectricElement);
                                    }
                                }
                                m_tmpResult.Clear();
                                ScanWireDomain(new GVCellFace(i, j, k, l), m_tmpVisited, m_tmpResult);
                                if (m_tmpResult.Count > 0) {
                                    WireDomainGVElectricElement GVElectricElement = new(this, m_tmpResult.Keys);
                                    AddGVElectricElement(GVElectricElement);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void RemoveWireDomains() {
            foreach (GVCellFace key in m_wiresToUpdate.Keys) {
                for (int i = key.X - 1; i <= key.X + 1; i++) {
                    for (int j = key.Y - 1; j <= key.Y + 1; j++) {
                        for (int k = key.Z - 1; k <= key.Z + 1; k++) {
                            for (int l = 0; l < 6; l++) {
                                for (int m = 0; m < 16; m++) {
                                    if (m_GVElectricElementsByCellFace.TryGetValue(
                                            new GVCellFace(
                                                i,
                                                j,
                                                k,
                                                l,
                                                1 << m
                                            ),
                                            out GVElectricElement value
                                        )
                                        && value is WireDomainGVElectricElement) {
                                        RemoveGVElectricElement(value);
                                    }
                                }
                                if (m_GVElectricElementsByCellFace.TryGetValue(new GVCellFace(i, j, k, l), out GVElectricElement value2)
                                    && value2 is WireDomainGVElectricElement) {
                                    RemoveGVElectricElement(value2);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ScanWireDomain(GVCellFace startCellFace, Dictionary<GVCellFace, bool> visited, Dictionary<GVCellFace, bool> result) {
            DynamicArray<GVCellFace> dynamicArray = [startCellFace];
            while (dynamicArray.Count > 0) {
                GVCellFace key = dynamicArray.Array[--dynamicArray.Count];
                if (visited.ContainsKey(key)) {
                    continue;
                }
                TerrainChunk chunkAtCell = SubsystemTerrain.Terrain.GetChunkAtCell(key.X, key.Z);
                if (chunkAtCell is not {
                        AreBehaviorsNotified: true
                    }) {
                    continue;
                }
                int cellValue = SubsystemTerrain.Terrain.GetCellValue(key.X, key.Y, key.Z);
                int num = Terrain.ExtractContents(cellValue);
                IGVElectricWireElementBlock GVElectricWireElementBlock = BlocksManager.Blocks[num] as IGVElectricWireElementBlock;
                if (GVElectricWireElementBlock == null) {
                    continue;
                }
                int newMask = GVElectricWireElementBlock.GetConnectionMask(cellValue);
                if (GVElectricWireElementBlock.IsWireHarness(cellValue)) {
                    if (key.Mask == int.MaxValue) {
                        continue;
                    }
                    newMask = key.Mask;
                }
                else if ((newMask & key.Mask) == 0) {
                    continue;
                }
                int connectedWireFacesMask = GVElectricWireElementBlock.GetConnectedWireFacesMask(cellValue, key.Face);
                if (connectedWireFacesMask == 0) {
                    continue;
                }
                for (int i = 0; i < 6; i++) {
                    if ((connectedWireFacesMask & (1 << i)) != 0) {
                        GVCellFace key2 = new(
                            key.X,
                            key.Y,
                            key.Z,
                            i,
                            newMask
                        );
                        if (!visited.TryAdd(key2, true)) {
                            continue;
                        }
                        result.Add(key2, true);
                        m_tmpConnectionPaths.Clear();
                        GetAllConnectedNeighbors(
                            key2.X,
                            key2.Y,
                            key2.Z,
                            key2.Face,
                            m_tmpConnectionPaths
                        );
                        foreach (GVElectricConnectionPath tmpConnectionPath in m_tmpConnectionPaths) {
                            dynamicArray.Add(
                                new GVCellFace(
                                    key2.X + tmpConnectionPath.NeighborOffsetX,
                                    key2.Y + tmpConnectionPath.NeighborOffsetY,
                                    key2.Z + tmpConnectionPath.NeighborOffsetZ,
                                    tmpConnectionPath.NeighborFace,
                                    newMask
                                )
                            );
                        }
                    }
                }
            }
        }

        public Dictionary<GVElectricElement, bool> GetListFromCache() {
            if (m_listsCache.Count > 0) {
                Dictionary<GVElectricElement, bool> result = m_listsCache[m_listsCache.Count - 1];
                m_listsCache.RemoveAt(m_listsCache.Count - 1);
                return result;
            }
            return new Dictionary<GVElectricElement, bool>();
        }

        public void ReturnListToCache(Dictionary<GVElectricElement, bool> list) {
            list.Clear();
            m_listsCache.Add(list);
        }

        public void SetSpeed(float speed) {
            SpeedFactor = speed;
            CircuitStepDuration = 0.01f / speed;
        }

        public void Draw(Camera camera, int drawOrder) {
            if (m_debugDrawPositions.Count > 0) {
                m_8NumberBatch ??= Project.FindSubsystem<SubsystemGV8NumberLedGlow>(true).batchCache;
                int i = 0;
                Vector3 right = -Vector3.Normalize(Vector3.Cross(camera.ViewDirection, camera.ViewUp));
                foreach (Vector3 blockPosition in m_debugDrawPositions) {
                    Vector3 origin = new(camera.ViewPosition.X, camera.ViewPosition.Y - 0.5f, camera.ViewPosition.Z);
                    m_flatBatch.QueueLine(origin, blockPosition, Color.Cyan);
                    Draw4DecNumber(
                        ++i,
                        origin + Vector3.Normalize(blockPosition - origin) * (0.6f + i * 0.05f),
                        0.04f,
                        right,
                        Vector3.UnitY,
                        Color.Cyan
                    );
                }
                m_flatBatch.Flush(camera.ViewProjectionMatrix);
                m_8NumberBatch.Flush(camera.ViewProjectionMatrix);
            }
        }

        public int[] DrawOrders => m_drawOrders;

        public void Draw4DecNumber(int number, Vector3 position, float numberWidth, Vector3 right, Vector3 up, Color color) {
            number %= 10000;
            int maxPlace = number switch {
                >= 1000 => 4,
                >= 100 => 3,
                >= 10 => 2,
                _ => 1
            };
            up *= numberWidth / 0.6f;
            Vector3 p = position - up / 2f - right * numberWidth / 2f;
            right *= maxPlace * numberWidth;
            float startX = 4 * maxPlace - 4;
            float pixelWidth = 4 * maxPlace - 1;
            for (int i = 0; i < maxPlace; i++) {
                int digit = number % 10;
                number /= 10;
                float px1 = (startX - i * 4) / pixelWidth;
                float px2 = px1 + 3 / pixelWidth;
                float tx1 = digit % 4 * 3f / 12f;
                float tx2 = tx1 + 3f / 12f;
                float ty1 = digit / 4 * 5f / 20f;
                float ty2 = ty1 + 5f / 20f;
                m_8NumberBatch.QueueQuad(
                    p - right * px1,
                    p - right * px2,
                    p - (right * px2 + up),
                    p - (right * px1 + up),
                    new Vector2(tx1, ty1),
                    new Vector2(tx2, ty1),
                    new Vector2(tx2, ty2),
                    new Vector2(tx1, ty2),
                    color
                );
            }
        }
    }
}