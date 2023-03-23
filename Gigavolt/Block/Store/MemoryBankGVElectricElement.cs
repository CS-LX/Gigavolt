using Engine;
using Engine.Media;
using System;

namespace Game
{
    public class MemoryBankGVElectricElement : RotateableGVElectricElement
    {
        public SubsystemGVMemoryBankBlockBehavior m_SubsystemGVMemoryBankBlockBehavior;
        public SubsystemGameInfo m_subsystemGameInfo;

        public uint m_voltage;

        public bool m_writeAllowed;

        public bool m_clockAllowed;

        public MemoryBankGVElectricElement(SubsystemGVElectricity subsystemGVElectricity, CellFace cellFace)
            : base(subsystemGVElectricity, cellFace)
        {
            m_SubsystemGVMemoryBankBlockBehavior = subsystemGVElectricity.Project.FindSubsystem<SubsystemGVMemoryBankBlockBehavior>(throwOnError: true);
            m_subsystemGameInfo = subsystemGVElectricity.Project.FindSubsystem<SubsystemGameInfo>(true);
            GVMemoryBankData blockData = m_SubsystemGVMemoryBankBlockBehavior.GetBlockData(cellFace.Point);
            if (blockData != null)
            {
                m_voltage = blockData.LastOutput;
            }
        }

        public override uint GetOutputVoltage(int face)
        {
            return m_voltage;
        }

        public override bool Simulate()
        {
            float voltage = m_voltage;
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            uint num = 0u;
            uint num2 = 0u;
            uint num3 = 0u;
            int rotation = Rotation;
            foreach (GVElectricConnection connection in Connections)
            {
                if (connection.ConnectorType != GVElectricConnectorType.Output && connection.NeighborConnectorType != 0)
                {
                    GVElectricConnectorDirection? connectorDirection = SubsystemGVElectricity.GetConnectorDirection(CellFaces[0].Face, rotation, connection.ConnectorFace);
                    if (connectorDirection.HasValue)
                    {
                        if (connectorDirection == GVElectricConnectorDirection.Right)
                        {
                            num2 = connection.NeighborGVElectricElement.GetOutputVoltage(connection.NeighborConnectorFace);
                        }
                        else if (connectorDirection == GVElectricConnectorDirection.Left)
                        {
                            num3 = connection.NeighborGVElectricElement.GetOutputVoltage(connection.NeighborConnectorFace);
                        }
                        else if (connectorDirection == GVElectricConnectorDirection.Bottom)
                        {
                            uint num4 = connection.NeighborGVElectricElement.GetOutputVoltage(connection.NeighborConnectorFace);
                            flag = (num4 >= 8u);
                            flag3 = (num4 > 0u && num4 < 8u);
                            flag2 = true;
                        }
                        else if (connectorDirection == GVElectricConnectorDirection.In)
                        {
                            num = connection.NeighborGVElectricElement.GetOutputVoltage(connection.NeighborConnectorFace);
                        }
                    }
                }
            }
            GVMemoryBankData memoryBankData = m_SubsystemGVMemoryBankBlockBehavior.GetBlockData(CellFaces[0].Point);
            if (memoryBankData != null&&memoryBankData.m_worldDirectory == null)
            {
                memoryBankData.m_worldDirectory = m_subsystemGameInfo.DirectoryName;
                memoryBankData.LoadData();
            }
            if (flag2)
            {
                if (flag && m_clockAllowed)
                {
                    m_clockAllowed = false;
                    m_voltage = ((memoryBankData != null) ? memoryBankData.Read(num2,num3) : 0);
                }
                else if (flag3 && m_writeAllowed)
                {
                    m_writeAllowed = false;
                    if (memoryBankData != null)
                    {
                        memoryBankData.Write(num2, num3, num);
                    }
                }
            }
            else
            {
                m_voltage = ((memoryBankData != null) ? memoryBankData.Read(num2, num3) : 0);
            }
            if (!flag)
            {
                m_clockAllowed = true;
            }
            if (!flag3)
            {
                m_writeAllowed = true;
            }
            if (memoryBankData != null)
            {
                memoryBankData.LastOutput = m_voltage;
            }
            return m_voltage != voltage;
        }
    }
}