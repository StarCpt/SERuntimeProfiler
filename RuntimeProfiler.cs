using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class RuntimeProfiler
        {
            private readonly Program Program;

            public double AverageRuntimeMs => RuntimeCollection.Average();
            /// <summary>Use <see cref="MaxRuntimeMsFast">MaxRuntimeMsFast</see> if performance is a major concern</summary>
            public double MaxRuntimeMs => RuntimeCollection.Max();
            public double MaxRuntimeMsFast { get; private set; }
            public double MinRuntimeMs => RuntimeCollection.Min();

            private readonly double[] RuntimeCollection;
            private int Counter = 0;
            private readonly int BufferSize;

            /// <summary></summary>
            /// <param name="Program"></param>
            /// <param name="BufferSize">Runtime buffer size. Must be 1 or higher.</param>
            public RuntimeProfiler(Program Program, int BufferSize = 300)
            {
                this.Program = Program;
                this.MaxRuntimeMsFast = Program.Runtime.LastRunTimeMs;
                this.BufferSize = MathHelper.Clamp(BufferSize, 1, int.MaxValue);
                this.RuntimeCollection = new double[BufferSize];
                this.RuntimeCollection[Counter] = Program.Runtime.LastRunTimeMs;
            }
            
            public void Run()
            {
                RuntimeCollection[Counter] = Program.Runtime.LastRunTimeMs;

                if (Program.Runtime.LastRunTimeMs > MaxRuntimeMsFast)
                {
                    MaxRuntimeMsFast = Program.Runtime.LastRunTimeMs;
                }

                Counter++;

                if (Counter >= BufferSize)
                {
                    Counter = 0;
                    MaxRuntimeMsFast = Program.Runtime.LastRunTimeMs;
                }
            }
        }
    }
}
