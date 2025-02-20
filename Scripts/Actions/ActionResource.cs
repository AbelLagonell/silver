using System;
using Godot;
using Godot.Collections;

namespace silver.Scripts.Actions;

[GlobalClass, Tool]
public abstract partial class ActionResource : Resource {
    private readonly Dictionary<string, Variant> _attributes = new();

    [Export]
    private Dictionary<string, Variant> Attributes {
        get => _attributes;
        set { return; }
    }

    protected void RegisterProperty(string propertyName, Variant value) {
        _attributes[propertyName] = value;
    }

    public T GetValue<[MustBeVariant] T>(string propertyName) {
        PopulateDictionary();
        if (!_attributes.ContainsKey(propertyName)) {
            GD.PushError($"Property '{propertyName}' not found in resource.");
            return default;
        }

        try {
            return _attributes[propertyName].As<T>();
        } catch (InvalidCastException) {
            GD.PushError($"Cannot convert property '{propertyName}' to requested type.");
            return default;
        }
    }

    protected abstract void PopulateDictionary();
}