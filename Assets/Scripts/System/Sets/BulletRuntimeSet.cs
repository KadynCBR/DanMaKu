using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CherryTeaGames.Core.Sets;

// namespace CherryTeaGames.Core.Sets
// {
//     [CreateAssetMenu(menuName = "RuntimeSet/BulletRuntimeSet")]
//     public class BulletRuntimeSet : RuntimeSet<BulletBase>
//     {
//         private bool isSlow = false;

//         public override void Add(BulletBase thing)
//         {
//             if (isSlow) thing.OnSlowDown();
//             base.Add(thing);
//         }

//         public override void Remove(BulletBase thing)
//         {
//             thing.OnResumeSpeed();
//             base.Remove(thing);
//         }

//         public void SlowAll()
//         {
//             isSlow = true;
//             foreach (BulletBase bb in Items)
//             {
//                 bb.OnSlowDown();
//             }
//         }

//         public void ResumeAll()
//         {
//             isSlow = false;
//             foreach (BulletBase bb in Items)
//             {
//                 bb.OnResumeSpeed();
//             }
//         }
//     }
// }
