using GameEntitySystem;

namespace Game {
    public class GigavoltModLoader : ModLoader {
        public override void __ModInitialize() {
            ModsManager.RegisterHook("OnProjectDisposed", this);
            ModsManager.RegisterHook("ToFreeChunks", this);
            ModsManager.RegisterHook("OnProjectLoaded", this);
        }

        public override void OnProjectDisposed() {
            foreach (SoundGeneratorGVElectricElement element in GVStaticStorage.GVSGCFEEList) {
                if (element.m_sound != null) {
                    if (element.m_playing) {
                        element.m_sound.Stop();
                    }
                    element.m_sound.Dispose();
                }
            }
            GVStaticStorage.GVSGCFEEList.Clear();
            GVStaticStorage.PreventChunkFromBeingFree = false;
            GVStaticStorage.GVUsingChunks.Clear();
            IGVCustomWheelPanelBlock.BasicElementsValues.Clear();
            IGVCustomWheelPanelBlock.WireThroughValues.Clear();
            IGVCustomWheelPanelBlock.TransformerValues.Clear();
            IGVCustomWheelPanelBlock.MemoryBankValues.Clear();
            IGVCustomWheelPanelBlock.LedValues.Clear();
        }

        public override void ToFreeChunks(TerrainUpdater terrainUpdater, TerrainChunk chunk, out bool KeepWorking) {
            KeepWorking = GVStaticStorage.PreventChunkFromBeingFree && GVStaticStorage.GVUsingChunks.Contains(chunk.Coords);
        }

        public override void OnProjectLoaded(Project project) {
            TerrainSerializer23 serializer = project.FindSubsystem<SubsystemTerrain>(true).TerrainSerializer;
            foreach (TerrainChunk chunk in GVStaticStorage.EditableItemBehaviorChangedChunks) {
                serializer.SaveChunkData(chunk);
            }
            GVStaticStorage.EditableItemBehaviorChangedChunks.Clear();
            IGVCustomWheelPanelBlock.BasicElementsValues.Clear();
            IGVCustomWheelPanelBlock.BasicElementsValues.AddRange(
                [
                    GVBlocksManager.GetBlockIndex<GVNotGateBlock>(),
                    GVBlocksManager.GetBlockIndex<GVAndGateBlock>(),
                    GVBlocksManager.GetBlockIndex<GVOrGateBlock>(),
                    GVBlocksManager.GetBlockIndex<GVXorGateBlock>(),
                    GVBlocksManager.GetBlockIndex<GVNandGateBlock>(),
                    GVBlocksManager.GetBlockIndex<GVNorGateBlock>(),
                    GVBlocksManager.GetBlockIndex<GVDelayGateBlock>()
                ]
            );
            IGVCustomWheelPanelBlock.WireThroughValues.Clear();
            IGVCustomWheelPanelBlock.WireThroughValues.AddRange(
                [
                    GVBlocksManager.GetBlockIndex<GVWireThroughPlanksBlock>(),
                    GVBlocksManager.GetBlockIndex<GVWireThroughStoneBlock>(),
                    GVBlocksManager.GetBlockIndex<GVWireThroughBricksBlock>(),
                    GVBlocksManager.GetBlockIndex<GVWireThroughSemiconductorBlock>(),
                    GVBlocksManager.GetBlockIndex<GVWireThroughCobblestoneBlock>()
                ]
            );
            IGVCustomWheelPanelBlock.TransformerValues.Clear();
            IGVCustomWheelPanelBlock.TransformerValues.AddRange([GVBlocksManager.GetBlockIndex<GV2OTransformerBlock>(), GVBlocksManager.GetBlockIndex<O2GVTransformerBlock>()]);
            IGVCustomWheelPanelBlock.MemoryBankValues.Clear();
            IGVCustomWheelPanelBlock.MemoryBankValues.Add(GVBlocksManager.GetBlockIndex<GVMemoryBankBlock>());
            IGVCustomWheelPanelBlock.LedValues.Clear();
            IGVCustomWheelPanelBlock.LedValues.AddRange([GVBlocksManager.GetBlockIndex<GVMulticoloredLedBlock>(), GVBlocksManager.GetBlockIndex<GV8NumberLedBlock>(), GVBlocksManager.GetBlockIndex<GVOneLedBlock>()]);
            IGVCustomWheelPanelBlock.LedValues.AddRange(GVBlocksManager.GetBlock<GV8x4LedBlock>().GetCreativeValues());
        }
    }
}