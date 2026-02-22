using NetlinkCore.Generic.EthTool;

namespace NetlinkCore.Links;

public class LinkFeatures
{
    private readonly EthToolNetlinkSocket _socket;
    private readonly int _linkIndex;
    private EthernetFeatures _features;

    public bool this[EthernetFeature feature]
    {
        get => _features[feature];
        set
        {
            var change = new EthernetFeatures(_features) { [feature] = value };
            _socket.SetFeatures(_linkIndex, _features, change);
            _features = change;
        }
    }

    internal LinkFeatures(EthToolNetlinkSocket socket, int linkIndex)
    {
        _socket = socket;
        _linkIndex = linkIndex;
        _features = _socket.GetFeatures(_linkIndex);
    }
}