using CommunityToolkit.Mvvm.Messaging.Messages;
using FireBrigade.Domain.Entities;

namespace FireBrigade.Libraries.Messages;

public class EmergencyBrigadeMessage : ValueChangedMessage<EmergencyBrigade>
{
    public EmergencyBrigade emergencyBrigade { get; private set; }

    public EmergencyBrigadeMessage(EmergencyBrigade value) : base(value)
    {
    }
}
