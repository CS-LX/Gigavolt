using Engine;
using Engine.Graphics;
using System.Collections.Generic;
using static Game.CommunityContentScreen;

namespace Game
{
    public class GVMoreTwoInTwoOutBlock : RotateableMountedGVElectricElementBlock
    {
        public const int Index = 1022;
        public Texture2D[] textures = new Texture2D[16];
        public string[] textureNames = new string[] { "Adder", "Subtracter", "Multiplier", "Divider" , "Remainder", "Equaler" , "Greater" , "NoLesser", "Lesser", "NoGreater", "Maximumer","Minimumer", "LeftShifter", "RightShifter" , "Power", "Logarithmer" };
        public override void Initialize()
        {
            base.Initialize();
            for (int i = 0; i < 16; i++)
            {
                textures[i] = ContentManager.Get<Texture2D>($"Textures/GVMoreTwoInTwoOutBlock/{textureNames[i]}");
            }
        }

        public GVMoreTwoInTwoOutBlock()
            : base("Models/GVMoreTwoInTwoOutBlock", "AndGate", 0.5f)
        {
        }
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            BlocksManager.DrawMeshBlock(primitivesRenderer, m_standaloneBlockMesh, textures[GetType(Terrain.ExtractData(value))], color, 2f * size, ref matrix, environmentData);
        }
        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
        {
            int num = Terrain.ExtractData(value) & 0x1F;
            generator.GenerateMeshVertices(this, x, y, z, m_blockMeshes[num], Color.White, null, geometry.GetGeometry(textures[GetType(Terrain.ExtractData(value))]).SubsetOpaque);
            GenerateGVWireVertices(generator, value, x, y, z, GetFace(value), m_centerBoxSize, Vector2.Zero, geometry.SubsetOpaque);
        }
        public override GVElectricElement CreateGVElectricElement(SubsystemGVElectricity subsystemGVElectricity, int value, int x, int y, int z)
        {
            return new MoreTwoInTwoOutGVElectricElement(subsystemGVElectricity, new CellFace(x, y, z, GetFace(value)), value);
        }

        public override GVElectricConnectorType? GetConnectorType(SubsystemTerrain terrain, int value, int face, int connectorFace, int x, int y, int z)
        {
            int data = Terrain.ExtractData(value);
            if (GetFace(value) == face)
            {
                GVElectricConnectorDirection? connectorDirection = SubsystemGVElectricity.GetConnectorDirection(GetFace(value), GetRotation(data), connectorFace);
                if (connectorDirection == GVElectricConnectorDirection.Right || connectorDirection == GVElectricConnectorDirection.Left)
                {
                    return GVElectricConnectorType.Input;
                }
                if (connectorDirection == GVElectricConnectorDirection.Top || connectorDirection == GVElectricConnectorDirection.In || connectorDirection == GVElectricConnectorDirection.Bottom)
                {
                    return GVElectricConnectorType.Output;
                }
            }
            return null;
        }
        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
        {
            int type = GetType(Terrain.ExtractData(value));
            switch (type)
            {
                case 1:
                    return "GV������";
                case 2:
                    return "GV�˷���";
                case 3:
                    return "GV������";
                case 4:
                    return "GVȡ����";
                case 5:
                    return "GV������";
                case 6:
                    return "GV������";
                case 7:
                    return "GV���ڵ�����";
                case 8:
                    return "GVС����";
                case 9:
                    return "GVС�ڵ�����";
                case 10:
                    return "GVȡ����";
                case 11:
                    return "GVȡС��";
                case 12:
                    return "GV������";
                case 13:
                    return "GV������";
                case 14:
                    return "GV�˷���";
                case 15:
                    return "GV������";
                default:
                    return "GV�ӷ���";
            }
        }
        public override IEnumerable<int> GetCreativeValues()
        {
            for (int i = 0; i < 16; i++)
            {
                yield return Terrain.MakeBlockValue(Index, 0, SetType(0, i));
            }
        }
        public static int GetType(int data)
        {
            return (data >> 5) & 15;
        }

        public static int SetType(int data, int type)
        {
            return (data & -481) | ((type & 15) << 5);
        }
    }
}