using Engine;
using System.Collections.Generic;

namespace Game
{
    public class GVMoreTwoInTwoOutBlock : RotateableMountedGVElectricElementBlock
    {
        public const int Index = 1022;

        public GVMoreTwoInTwoOutBlock()
            : base("Models/Detonator", "Detonator", 0.125f)
        {
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