using System;
using System.Linq;
using Godot;

namespace silver;

public partial class Util : Node {
    public static Util Instance { get; private set; }

    public override void _Ready() {
        Instance = this;
    }

    public static string GetFileName(Resource rsc) {
        var path = rsc.GetPath();
        var filename = path.Remove(0, 6).Split('/').Last();
        return filename.Remove(filename.IndexOf('.'));
    }
}