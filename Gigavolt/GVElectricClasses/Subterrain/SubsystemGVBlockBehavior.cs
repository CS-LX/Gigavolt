namespace Game {
    public abstract class SubsystemGVBlockBehavior : SubsystemBlockBehavior {
        public virtual void OnBlockRemoved(int value, int newValue, int x, int y, int z, GVSubterrainSystem system) { }
        public virtual void OnBlockAdded(int value, int oldValue, int x, int y, int z, GVSubterrainSystem system) { }
        public virtual void OnBlockModified(int value, int oldValue, int x, int y, int z, GVSubterrainSystem system) { }
        public virtual void OnNeighborBlockChanged(int x, int y, int z, int neighborX, int neighborY, int neighborZ, GVSubterrainSystem system) { }
        public virtual void OnChunkInitialized(TerrainChunk chunk, GVSubterrainSystem system) { }
        public virtual void OnBlockGenerated(int value, int x, int y, int z, bool isLoaded, GVSubterrainSystem system) { }
    }
}