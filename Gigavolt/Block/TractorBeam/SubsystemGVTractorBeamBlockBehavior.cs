using System.Collections.Generic;
using System.Linq;
using Engine;
using TemplatesDatabase;

namespace Game {
    public class SubsystemGVTractorBeamBlockBehavior : SubsystemBlockBehavior, IUpdateable, IDrawable {
        public readonly Dictionary<Point3, GVSubterrainSystem> m_subterrainSystems = new();

        public override int[] HandledBlocks => [GVTractorBeamBlock.Index];
        public UpdateOrder UpdateOrder => UpdateOrder.Terrain;
        public int[] DrawOrders => [0, 100];
        bool firstUpdate = true;
        public override void Load(ValuesDictionary valuesDictionary) { }

        public void Update(float dt) {
            if (firstUpdate) {
                firstUpdate = false;
                Vector3 playerPosition = Project.FindSubsystem<SubsystemPlayers>(true).m_componentPlayers[0].ComponentBody.Position;
                GVSubterrainSystem subterrainSystem1 = new(
                    Project,
                    Matrix.Identity,
                    Terrain.ToCell(playerPosition),
                    null,
                    Point2.Zero,
                    new Point2(1, 1)
                );
                subterrainSystem1.UseParentLight = true;
                m_subterrainSystems.Add(Terrain.ToCell(playerPosition), subterrainSystem1);
                subterrainSystem1.ChangeCell(0, 1, 0, BedrockBlock.Index);
                subterrainSystem1.ChangeCell(1, 1, 1, DirtBlock.Index);
                GVSubterrainSystem subterrainSystem2 = new(
                    Project,
                    Matrix.Identity,
                    new Point3(3, 1, 1),
                    subterrainSystem1,
                    Point2.Zero,
                    Point2.One
                );
                subterrainSystem2.UseParentLight = false;
                subterrainSystem2.Light = 15;
                subterrainSystem1.Children.Add(subterrainSystem2);
                subterrainSystem2.ChangeCell(0, 1, 0, PlanksBlock.Index);
            }
            foreach (GVSubterrainSystem subterrainSystem in m_subterrainSystems.Values) {
                subterrainSystem.BaseTransform = Matrix.CreateRotationY(0.005f) * subterrainSystem.BaseTransform;
                GVSubterrainSystem first = subterrainSystem.Children.First();
                first.BaseTransform *= Matrix.CreateRotationY(0.01f);
                subterrainSystem.Update();
            }
        }

        public void Draw(Camera camera, int drawOrder) {
            foreach (GVSubterrainSystem subterrainSystem in m_subterrainSystems.Values) {
                subterrainSystem.Draw(camera, drawOrder);
            }
        }

        public override void Dispose() {
            foreach (GVSubterrainSystem subterrainSystem in m_subterrainSystems.Values) {
                subterrainSystem.Dispose();
            }
        }
    }
}