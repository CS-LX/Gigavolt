namespace Game {
    public abstract class MountedGVElectricElementBlock : GenerateGVWireVerticesBlock, IGVElectricElementBlock {
        public abstract int GetFace(int value);

        public abstract GVElectricElement CreateGVElectricElement(SubsystemGVElectricity subsystemGVElectricity, int value, int x, int y, int z);

        public abstract GVElectricConnectorType? GetGVConnectorType(SubsystemTerrain terrain, int value, int face, int connectorFace, int x, int y, int z);

        public virtual int GetConnectionMask(int value) => int.MaxValue;
    }
}