using System.Collections.Generic;

namespace Game
{
    public class GVAnalogToDigitalConverterBlock : RotateableMountedGVElectricElementBlock
    {
        public const int Index = 881;

        public GVAnalogToDigitalConverterBlock()
            : base("Models/Gates", "AnalogToDigitalConverter", 0.375f)
        {
        }

        public override GVElectricElement CreateGVElectricElement(SubsystemGVElectricity subsystemGVElectricity, int value, int x, int y, int z)
        {
            return new AnalogToDigitalConverterGVElectricElement(subsystemGVElectricity, new CellFace(x, y, z, GetFace(value)), value);
        }

        public override GVElectricConnectorType? GetConnectorType(SubsystemTerrain terrain, int value, int face, int connectorFace, int x, int y, int z)
        {
            int data = Terrain.ExtractData(value);
            if (GetFace(value) == face)
            {
                GVElectricConnectorDirection? connectorDirection = SubsystemGVElectricity.GetConnectorDirection(GetFace(value), GetRotation(data), connectorFace);
                if (connectorDirection == GVElectricConnectorDirection.In)
                {
                    return GVElectricConnectorType.Input;
                }
                if (connectorDirection == GVElectricConnectorDirection.Bottom || connectorDirection == GVElectricConnectorDirection.Top || connectorDirection == GVElectricConnectorDirection.Right || connectorDirection == GVElectricConnectorDirection.Left)
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
                    return "GV 8λ���2λ��";
                case 2:
                    return "GV 16λ���4λ��";
                case 3:
                    return "GV 32λ���8λ��";
                default:
                    return "GV 4λ���1λ��";
            }
        }
        public override IEnumerable<int> GetCreativeValues()
        {
            for (int i = 0; i < 4; i++)
            {
                yield return Terrain.MakeBlockValue(Index, 0, SetType(0, i));
            }
        }
        public static int GetType(int data)
        {
            return (data >> 5) & 3;
        }

        public static int SetType(int data, int color)
        {
            return (data & -97) | ((color & 3) << 5);
        }
    }
}
