namespace NetlinkCore.Generic;

public class EthernetFeatures
{
    private readonly ulong _supported;
    private readonly ulong _wanted;
    private readonly ulong _noChange;

    internal ulong Active;

    public bool this[EthernetFeature feature]
    {
        get => (Active & FeatureBit(feature)) != 0;
        set
        {
            if (value)
                Active |= FeatureBit(feature);
            else
                Active &= ~FeatureBit(feature);
        }
    }

    public bool IsSupported(EthernetFeature feature) => (_supported & FeatureBit(feature)) != 0;
    public bool IsWanted(EthernetFeature feature) => (_wanted & FeatureBit(feature)) != 0;
    public bool IsNoChange(EthernetFeature feature) => (_noChange & FeatureBit(feature)) != 0;

    public EthernetFeatures(EthernetFeatures other)
    {
        Active = other.Active;
        _supported = other._supported;
        _wanted = other._wanted;
        _noChange = other._noChange;
    }

    internal EthernetFeatures(ulong active, ulong supported, ulong wanted, ulong noChange)
    {
        Active = active;
        _supported = supported;
        _wanted = wanted;
        _noChange = noChange;
    }

    private static ulong FeatureBit(EthernetFeature feature) => 1UL << (int)feature;
}