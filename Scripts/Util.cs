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

    // Generic vector parser that leverages C# generics
    public static T ParseVector<T>(string value) where T : struct {
        string[] values = value.Trim().Trim('(').Trim(')').Split(',');

        if (typeof(T) == typeof(Vector2)) {
            if (values.Length != 2)
                throw new ArgumentException("Vector2 requires exactly 2 components");
            return (T)(object)new Vector2(float.Parse(values[0]), float.Parse(values[1]));
        } else if (typeof(T) == typeof(Vector3)) {
            if (values.Length != 3)
                throw new ArgumentException("Vector3 requires exactly 3 components");
            return (T)(object)new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
        } else if (typeof(T) == typeof(Vector4)) {
            if (values.Length != 4)
                throw new ArgumentException("Vector4 requires exactly 4 components");
            return (T)(object)new Vector4(float.Parse(values[0]), float.Parse(values[1]),
                float.Parse(values[2]), float.Parse(values[3]));
        } else {
            throw new ArgumentException($"Unsupported vector type: {typeof(T).Name}");
        }
    }

    // Parse vector with automatic type detection based on component count
    public static object ParseVectorAuto(string value) {
        string[] values = value.Trim().Trim('(').Trim(')').Split(',');

        return values.Length switch {
            2 => new Vector2(float.Parse(values[0]), float.Parse(values[1])),
            3 => new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2])),
            4 => new Vector4(float.Parse(values[0]), float.Parse(values[1]),
                float.Parse(values[2]), float.Parse(values[3])),
            _ => throw new ArgumentException($"Unsupported number of vector components: {values.Length}")
        };
    }
}