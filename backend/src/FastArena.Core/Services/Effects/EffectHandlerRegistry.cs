using FastArena.Core.Domain.Effects;
using FastArena.Core.Interfaces.Effects;

namespace FastArena.Core.Services.Effects;

/// <summary>
/// Registry mapping EffectType to their corresponding IEffectHandler implementations.
/// Registered as a singleton in the DI container.
/// Handlers are retrieved by type and used to execute effect logic at specific fight phases.
/// </summary>
public class EffectHandlerRegistry
{
    private readonly Dictionary<EffectType, IEffectHandler> _handlers;
    
    public EffectHandlerRegistry(IEnumerable<IEffectHandler> handlers)
    {
        _handlers = handlers.ToDictionary(handler => handler.EffectType);
    }
    
    /// <summary>
    /// Retrieve the handler for a given effect type.
    /// Throws if no handler is registered for this type.
    /// </summary>
    public IEffectHandler GetHandler(EffectType effectType)
    {
        if (!_handlers.TryGetValue(effectType, out var handler))
        {
            throw new InvalidOperationException($"No handler registered for effect type: {effectType}");
        }
        return handler;
    }
    
    /// <summary>
    /// Check if a handler exists for a given effect type.
    /// </summary>
    public bool HasHandler(EffectType effectType) => _handlers.ContainsKey(effectType);
}
