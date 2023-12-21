using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2023.Day20
{
    internal abstract class PulseBase : BaseSolution<ButtonModule>
    {
        private static readonly Regex _moduleRegex = new(@"^(?<type>[\&\%])(?<name>[^\s]+) -> (?<outputModules>.+)");

        internal override ButtonModule ReadInputFromFile()
        {
            var lines = ReadFileAsLines(@".\Puzzles\2023\Day20\Input.txt");
            var modules = new Dictionary<string, Module>();
            var destModules = new Dictionary<string, List<string>>();
            var buttonModule = new ButtonModule();
            foreach (var line in lines)
            {
                if (line.StartsWith("broadcaster"))
                {
                    var broadcastModule = new Broadcast();
                    modules.Add("broadcaster", broadcastModule);
                    buttonModule.AddOuput(broadcastModule);
                    broadcastModule.AddInput(buttonModule);
                    var destModuleNames = line.Split(" -> ")[1].Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    if (!destModules.ContainsKey("broadcaster"))
                    {
                        destModules.Add("broadcaster", new List<string>());
                    }
                    foreach (var destModuleName in destModuleNames)
                    {
                        destModules["broadcaster"].Add(destModuleName);
                    }
                }
                else if (_moduleRegex.Match(line) is Match match && match.Success)
                {
                    var type = match.Groups["type"].Value;
                    var name = match.Groups["name"].Value;
                    var outputModuleNames = match.Groups["outputModules"].Value.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    Module module = type switch
                    {
                        "&" => new Conjunction(name),
                        "%" => new FlipFlop(name),
                        _ => throw new InvalidDataException($"Invalid type: {type}")
                    };
                    modules.Add(name, module);
                    if (!destModules.ContainsKey(name))
                    {
                        destModules.Add(name, new List<string>());
                    }
                    foreach (var outputModuleName in outputModuleNames)
                    {
                        destModules[name].Add(outputModuleName);
                    }
                }
                else
                {
                    throw new InvalidDataException($"Invalid input: {line}");
                }
            }
            foreach (var destModule in destModules)
            {
                foreach (var destModuleName in destModule.Value)
                {
                    if (modules.ContainsKey(destModuleName))
                    {
                        modules[destModule.Key].AddOuput(modules[destModuleName]);
                        modules[destModuleName].AddInput(modules[destModule.Key]);
                    }
                    else
                    {
                        var newMod = new Simple(destModuleName);
                        modules.Add(destModuleName, newMod);
                        modules[destModule.Key].AddOuput(newMod);
                        newMod.AddInput(modules[destModule.Key]);
                    }
                }
            }
            return buttonModule;
        }
    }

    public abstract class Module
    {
        public static event EventHandler HighPulse;
        public static event EventHandler LowPulse;

        protected static Queue<Action> PendingPulses { get; } = new Queue<Action>();

        public string Name { get; set; }
        public bool Status { get; set; }
        public bool ShouldPropegate { get; set; } = true;
        protected List<Module> Inputs { get; }
        protected List<Module> Outputs { get; }

        public Module(string name)
        {
            Name = name;
            Inputs = new List<Module>();
            Outputs = new List<Module>();
        }

        public virtual void AddInput(Module input) => Inputs.Add(input);
        public virtual void AddOuput(Module output) => Outputs.Add(output);
        public virtual void TakePulse(bool pulseType, string sourceName)
        {
            if (pulseType)
            {
                HighPulse?.Invoke(this, null);
            }
            else
            {
                LowPulse?.Invoke(this, null);
            }
            //Console.WriteLine($"{sourceName} -{(pulseType ? "high" : "low")}-> {Name}");
        }
        protected void EmitPulse()
        {
            if (ShouldPropegate)
            {
                foreach (var output in Outputs)
                {
                    PendingPulses.Enqueue(() => output.TakePulse(Status, Name));
                }
            }
            while (PendingPulses.Count > 0)
            {
                PendingPulses.Dequeue()();
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is Module other)
            {
                return Name == other.Name;
            }
            return false;
        }
    }

    public class Simple : Module
    {
        public Simple(string name) : base(name) { ShouldPropegate = false; }
    }

    public class FlipFlop : Module
    {
        public FlipFlop(string name) : base(name) { }
        public override void TakePulse(bool pulseType, string sourceName)
        {
            base.TakePulse(pulseType, sourceName);
            ShouldPropegate = !pulseType;
            if (!pulseType)
            {
                Status = !Status;
            }
            EmitPulse();
        }
    }

    public class Conjunction : Module
    {
        public Conjunction(string name) : base(name)
        {
            _InputMemory = new List<bool>();
        }
        private List<bool> _InputMemory { get; set; }

        public override void AddInput(Module input)
        {
            base.AddInput(input);
            _InputMemory.Add(false);
        }

        public override void TakePulse(bool pulseType, string sourceName)
        {
            base.TakePulse(pulseType, sourceName);
            _InputMemory[Inputs.FindIndex(m => m.Name == sourceName)] = pulseType;
            Status = !_InputMemory.All(p => p);
            EmitPulse();
        }
    }

    public class ButtonModule : Module
    {
        public ButtonModule() : base("button") { }

        public void Press()
        {
            EmitPulse();
        }
    }

    public class Broadcast : Module
    {
        public Broadcast() : base("broadcaster") { }

        public override void TakePulse(bool pulseType, string sourceName)
        {
            base.TakePulse(pulseType, sourceName);
            Status = pulseType;
            EmitPulse();
        }
    }
}
