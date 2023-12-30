﻿using System.Collections.Generic;
using Engine;
using Engine.Graphics;
using Engine.Media;

namespace Game {
    public class GVCopperHammerBlock : Block {
        public const int Index = 867;
        public readonly Texture2D WhiteTexture = Texture2D.Load(new Image(1, 1) { Pixels = { [0] = Color.White } });

        public int m_handleTextureSlot = 47;
        public int m_headTextureSlot = 77;
        public BlockMesh m_standaloneBlockMesh_Handle = new();
        public BlockMesh m_standaloneBlockMesh_Head = new();

        public override void Initialize() {
            Model model = ContentManager.Get<Model>("Models/Hammer");
            Matrix absoluteTransform1 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Handle").ParentBone);
            Matrix absoluteTransform2 = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Head").ParentBone);
            BlockMesh blockMesh1 = new();
            blockMesh1.AppendModelMeshPart(
                model.FindMesh("Handle").MeshParts[0],
                absoluteTransform1 * Matrix.CreateTranslation(0.0f, -0.5f, 0.0f),
                false,
                false,
                false,
                false,
                Color.White
            );
            blockMesh1.TransformTextureCoordinates(Matrix.CreateTranslation(m_handleTextureSlot % 16 / 16f, m_handleTextureSlot / 16 / 16f, 0.0f));
            BlockMesh blockMesh2 = new();
            blockMesh2.AppendModelMeshPart(
                model.FindMesh("Head").MeshParts[0],
                absoluteTransform2 * Matrix.CreateTranslation(0.0f, -0.5f, 0.0f),
                false,
                false,
                false,
                false,
                Color.White
            );
            blockMesh2.TransformTextureCoordinates(Matrix.CreateTranslation(m_headTextureSlot % 16 / 16f, m_headTextureSlot / 16 / 16f, 0.0f));
            m_standaloneBlockMesh_Handle.AppendBlockMesh(blockMesh1);
            m_standaloneBlockMesh_Head.AppendBlockMesh(blockMesh2);
            base.Initialize();
        }

        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z) { }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData) {
            environmentData = environmentData ?? BlocksManager.m_defaultEnvironmentData;
            int? blockColor = GetColor(Terrain.ExtractData(value));
            BlocksManager.DrawMeshBlock(
                primitivesRenderer,
                m_standaloneBlockMesh_Handle,
                environmentData.SubsystemTerrain == null ? BlocksTexturesManager.DefaultBlocksTexture : environmentData.SubsystemTerrain.SubsystemAnimatedTextures.AnimatedBlocksTexture,
                color,
                2f * size,
                ref matrix,
                environmentData
            );
            BlocksManager.DrawMeshBlock(
                primitivesRenderer,
                m_standaloneBlockMesh_Head,
                blockColor.HasValue ? WhiteTexture :
                environmentData.SubsystemTerrain == null ? BlocksTexturesManager.DefaultBlocksTexture : environmentData.SubsystemTerrain.SubsystemAnimatedTextures.AnimatedBlocksTexture,
                blockColor.HasValue ? color * SubsystemPalette.GetColor(environmentData, blockColor) : color,
                2f * size,
                ref matrix,
                environmentData
            );
        }

        public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value) {
            int? paintColor = GetColor(Terrain.ExtractData(value));
            return paintColor.HasValue ? SubsystemPalette.GetName(subsystemTerrain, paintColor, "遥控器") : base.GetDisplayName(subsystemTerrain, value);
        }

        public override string GetCategory(int value) => GetColor(Terrain.ExtractData(value)).HasValue ? "GV Electrics Multiple" : "GV Electrics Expand";

        public override string GetDescription(int value) => GetColor(Terrain.ExtractData(value)).HasValue ? "远程操作和遥控器同色的开关、按钮" : base.GetDescription(value);

        public override IEnumerable<int> GetCreativeValues() {
            yield return Terrain.MakeBlockValue(Index);
            yield return Terrain.MakeBlockValue(Index, 0, SetColor(0, 0));
            yield return Terrain.MakeBlockValue(Index, 0, SetColor(0, 8));
            yield return Terrain.MakeBlockValue(Index, 0, SetColor(0, 15));
            yield return Terrain.MakeBlockValue(Index, 0, SetColor(0, 11));
            yield return Terrain.MakeBlockValue(Index, 0, SetColor(0, 12));
            yield return Terrain.MakeBlockValue(Index, 0, SetColor(0, 13));
            yield return Terrain.MakeBlockValue(Index, 0, SetColor(0, 14));
            yield return Terrain.MakeBlockValue(Index, 0, SetColor(0, 1));
            yield return Terrain.MakeBlockValue(Index, 0, SetColor(0, 2));
            yield return Terrain.MakeBlockValue(Index, 0, SetColor(0, 3));
            yield return Terrain.MakeBlockValue(Index, 0, SetColor(0, 4));
            yield return Terrain.MakeBlockValue(Index, 0, SetColor(0, 5));
            yield return Terrain.MakeBlockValue(Index, 0, SetColor(0, 6));
            yield return Terrain.MakeBlockValue(Index, 0, SetColor(0, 10));
        }

        public override int GetDisplayOrder(int value) => GetColor(Terrain.ExtractData(value)).HasValue ? 20 : base.GetDisplayOrder(value);

        public override bool IsEditable_(int value) => !GetColor(Terrain.ExtractData(value)).HasValue;

        public static int? GetColor(int data) {
            if ((data & 16) != 0) {
                return data & 0xF;
            }
            return null;
        }

        public static int SetColor(int data, int? color) {
            if (color.HasValue) {
                return (data & -32) | 16 | (color.Value & 0xF);
            }
            return data & -32;
        }
    }
}