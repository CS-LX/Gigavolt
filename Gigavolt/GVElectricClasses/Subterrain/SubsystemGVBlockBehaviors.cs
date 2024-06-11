using System.Collections.Generic;
using GameEntitySystem;
using TemplatesDatabase;

namespace Game {
    public class SubsystemGVBlockBehaviors : Subsystem {
        public List<SubsystemGVBlockBehavior>[] m_blockBehaviorsByContents;
        public readonly HashSet<SubsystemGVBlockBehavior> BlockBehaviors = [];
        public List<SubsystemGVBlockBehavior> GetBlockBehaviors(int contents) => m_blockBehaviorsByContents[contents];

        public override void Load(ValuesDictionary valuesDictionary) {
            m_blockBehaviorsByContents = new List<SubsystemGVBlockBehavior>[BlocksManager.Blocks.Length];
            SubsystemBlockBehaviors originalSubsystem = Project.FindSubsystem<SubsystemBlockBehaviors>(true);
            for (int i = 0; i < BlocksManager.Blocks.Length; i++) {
                m_blockBehaviorsByContents[i] = [];
                foreach (SubsystemBlockBehavior originalBehavior in originalSubsystem.m_blockBehaviorsByContents[i]) {
                    if (originalBehavior is SubsystemGVBlockBehavior blockBehavior) {
                        m_blockBehaviorsByContents[i].Add(blockBehavior);
                        BlockBehaviors.Add(blockBehavior);
                    }
                }
            }
        }
    }
}