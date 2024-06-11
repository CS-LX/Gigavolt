namespace Game {
    public class GVBlockGeometryGenerator : BlockGeometryGenerator {
        public readonly SubsystemGVElectricity SubsystemGVElectricity;

        public GVBlockGeometryGenerator(Terrain terrain, SubsystemTerrain subsystemTerrain, SubsystemElectricity subsystemElectricity, SubsystemFurnitureBlockBehavior subsystemFurnitureBlockBehavior, SubsystemMetersBlockBehavior subsystemMetersBlockBehavior, SubsystemPalette subsystemPalette, SubsystemGVElectricity subsystemGVElectricity) : base(
            terrain,
            subsystemTerrain,
            subsystemElectricity,
            subsystemFurnitureBlockBehavior,
            subsystemMetersBlockBehavior,
            subsystemPalette
        ) => SubsystemGVElectricity = subsystemGVElectricity;
    }
}