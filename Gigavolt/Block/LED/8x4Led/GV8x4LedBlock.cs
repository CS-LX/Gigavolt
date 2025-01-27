using System;
using System.Collections.Generic;
using Engine;
using Engine.Graphics;

namespace Game {
    public class GV8x4LedBlock : MountedGVElectricElementBlock, IGVCustomWheelPanelBlock {
        public const int Index = 857;

        public BlockMesh m_standaloneBlockMesh;

        public readonly BlockMesh[] m_blockMeshesByFace = new BlockMesh[6];

        public readonly BoundingBox[][] m_collisionBoxesByFace = new BoundingBox[6][];
        public Texture2D emptyTexture;
        readonly Texture2D[] fullTexture = new Texture2D[3];

        public override void Initialize() {
            ModelMesh modelMesh = ContentManager.Get<Model>("Models/GigavoltGates").FindMesh("OneLed");
            Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(modelMesh.ParentBone);
            for (int i = 0; i < 6; i++) {
                Matrix m = i >= 4 ? i != 4 ? Matrix.CreateRotationX((float)Math.PI) * Matrix.CreateTranslation(0.5f, 1f, 0.5f) : Matrix.CreateTranslation(0.5f, 0f, 0.5f) : Matrix.CreateRotationX((float)Math.PI / 2f) * Matrix.CreateTranslation(0f, 0f, -0.5f) * Matrix.CreateRotationY(i * (float)Math.PI / 2f) * Matrix.CreateTranslation(0.5f, 0.5f, 0.5f);
                m_blockMeshesByFace[i] = new BlockMesh();
                m_blockMeshesByFace[i]
                .AppendModelMeshPart(
                    modelMesh.MeshParts[0],
                    boneAbsoluteTransform * m,
                    false,
                    false,
                    false,
                    false,
                    Color.White
                );
                m_collisionBoxesByFace[i] = [m_blockMeshesByFace[i].CalculateBoundingBox()];
            }
            Matrix m2 = Matrix.CreateRotationY(-(float)Math.PI / 2f) * Matrix.CreateRotationZ((float)Math.PI / 2f);
            m_standaloneBlockMesh = new BlockMesh();
            m_standaloneBlockMesh.AppendModelMeshPart(
                modelMesh.MeshParts[0],
                boneAbsoluteTransform * m2,
                false,
                false,
                false,
                false,
                Color.White
            );
            emptyTexture = ContentManager.Get<Texture2D>("Textures/GVOneLedBlockEmpty");
            fullTexture[0] = ContentManager.Get<Texture2D>("Textures/GV4x2LedBlockFull");
            fullTexture[1] = ContentManager.Get<Texture2D>("Textures/GV4x4LedBlockFull");
            fullTexture[2] = ContentManager.Get<Texture2D>("Textures/GV8x4LedBlockFull");
        }

        public override bool IsFaceTransparent(SubsystemTerrain subsystemTerrain, int face, int value) {
            int mountingFace = GetMountingFace(Terrain.ExtractData(value));
            return face != CellFace.OppositeFace(mountingFace);
        }

        public override int GetFace(int value) => GetMountingFace(Terrain.ExtractData(value));

        public override BlockPlacementData GetPlacementValue(SubsystemTerrain subsystemTerrain, ComponentMiner componentMiner, int value, TerrainRaycastResult raycastResult) {
            int data = SetMountingFace(Terrain.ExtractData(value), raycastResult.CellFace.Face);
            int value2 = Terrain.ReplaceData(value, data);
            BlockPlacementData result = default;
            result.Value = value2;
            result.CellFace = raycastResult.CellFace;
            return result;
        }

        public override BoundingBox[] GetCustomCollisionBoxes(SubsystemTerrain terrain, int value) {
            int mountingFace = GetMountingFace(Terrain.ExtractData(value));
            if (mountingFace >= m_collisionBoxesByFace.Length) {
                return null;
            }
            return m_collisionBoxesByFace[mountingFace];
        }

        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z) {
            int mountingFace = GetMountingFace(Terrain.ExtractData(value));
            if (mountingFace < m_blockMeshesByFace.Length) {
                generator.GenerateMeshVertices(
                    this,
                    x,
                    y,
                    z,
                    m_blockMeshesByFace[mountingFace],
                    Color.White,
                    null,
                    geometry.GetGeometry(emptyTexture).SubsetOpaque
                );
                GVBlockGeometryGenerator.GenerateGVWireVertices(
                    generator,
                    value,
                    x,
                    y,
                    z,
                    mountingFace,
                    1f,
                    Vector2.Zero,
                    geometry.SubsetOpaque
                );
            }
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData) {
            BlocksManager.DrawMeshBlock(
                primitivesRenderer,
                m_standaloneBlockMesh,
                fullTexture[GetType(Terrain.ExtractData(value))],
                color,
                2f * size,
                ref matrix,
                environmentData
            );
        }

        public override GVElectricElement CreateGVElectricElement(SubsystemGVElectricity subsystemGVElectricity, int value, int x, int y, int z, uint subterrainId) => new _8x4LedGVElectricElement(subsystemGVElectricity, new GVCellFace(x, y, z, GetFace(value)), subterrainId);

        public override GVElectricConnectorType? GetGVConnectorType(SubsystemGVSubterrain subsystem, int value, int face, int connectorFace, int x, int y, int z, Terrain terrain) {
            int face2 = GetFace(value);
            if (face == face2
                && SubsystemGVElectricity.GetConnectorDirection(face2, 0, connectorFace).HasValue) {
                return GVElectricConnectorType.Input;
            }
            return null;
        }

        public static int GetMountingFace(int data) => data & 7;

        public static int SetMountingFace(int data, int face) => (data & -8) | (face & 7);

        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value) => LanguageControl.Get(GetType().Name, GetType(Terrain.ExtractData(value)));

        public override IEnumerable<int> GetCreativeValues() {
            for (int i = 0; i < 3; i++) {
                yield return Terrain.MakeBlockValue(BlockIndex, 0, SetType(0, i));
            }
        }

        public override void GetDropValues(SubsystemTerrain subsystemTerrain, int oldValue, int newValue, int toolLevel, List<BlockDropValue> dropValues, out bool showDebris) {
            int data = Terrain.ExtractData(oldValue);
            dropValues.Add(new BlockDropValue { Value = Terrain.MakeBlockValue(BlockIndex, 0, SetType(0, GetType(data))), Count = 1 });
            showDebris = true;
        }

        public static int GetType(int data) => (data >> 5) & 3;

        public static int SetType(int data, int color) => (data & -97) | ((color & 3) << 5);
        public List<int> GetCustomWheelPanelValues(int centerValue) => IGVCustomWheelPanelBlock.LedValues;
    }
}