namespace Game {
    public interface IGVElectricElementBlock {
        GVElectricElement CreateGVElectricElement(SubsystemGVElectricity subsystemGVElectricity, int value, int x, int y, int z);

        GVElectricConnectorType? GetGVConnectorType(SubsystemTerrain terrain, int value, int face, int connectorFace, int x, int y, int z);

        int GetConnectionMask(int value);
    }
}