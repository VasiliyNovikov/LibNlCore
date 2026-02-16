namespace NetlinkCore.Generic.EthTool;

public enum EthernetFeature
{
    TxScatterGather             =  0, // NETIF_F_SG_BIT                  - tx-scatter-gather              - Scatter/gather IO
    TxChecksumIPv4              =  1, // NETIF_F_IP_CSUM_BIT             - tx-checksum-ipv4               - Can checksum TCP/UDP over IPv4
                                      // __UNUSED_NETIF_F_1
    TxChecksumIpGeneric         =  3, // NETIF_F_HW_CSUM_BIT             - tx-checksum-ip-generic         - Can checksum all the packets
    TxChecksumIPv6              =  4, // NETIF_F_IPV6_CSUM_BIT           - tx-checksum-ipv6               - Can checksum TCP/UDP over IPV6
    HighDma                     =  5, // NETIF_F_HIGHDMA_BIT             - highdma                        - Can DMA to high memory
    TxScatterGatherFraglist     =  6, // NETIF_F_FRAGLIST_BIT            - tx-scatter-gather-fraglist     - Scatter/gather IO
    TxVlanHwInsert              =  7, // NETIF_F_HW_VLAN_CTAG_TX_BIT     - tx-vlan-hw-insert              - Transmit VLAN CTAG HW acceleration
    RxVlanHwParse               =  8, // NETIF_F_HW_VLAN_CTAG_RX_BIT     - rx-vlan-hw-parse               - Receive VLAN CTAG HW acceleration
    RxVlanFilter                =  9, // NETIF_F_HW_VLAN_CTAG_FILTER_BIT - rx-vlan-filter                 - Receive filtering on VLAN CTAGs
    VlanChallenged              = 10, // NETIF_F_VLAN_CHALLENGED_BIT     - vlan-challenged                - Device cannot handle VLAN packets
    TxGenericSegmentation       = 11, // NETIF_F_GSO_BIT                 - tx-generic-segmentation        - Enable software GSO
                                      // __UNUSED_NETIF_F_12
                                      // __UNUSED_NETIF_F_13
    RxGro                       = 14, // NETIF_F_GRO_BIT                 - rx-gro                         - Generic receive offload
    RxLro                       = 15, // NETIF_F_LRO_BIT                 - rx-lro                         - large receive offload

    TxTcpSegmentation           = 16, // NETIF_F_TSO_BIT                 - tx-tcp-segmentation            - TCPv4 segmentation
    TxGsoRobust                 = 17, // NETIF_F_GSO_ROBUST_BIT          - tx-gso-robust                  - ->SKB_GSO_DODGY
    TxTcpEcnSegmentation        = 18, // NETIF_F_TSO_ECN_BIT             - tx-tcp-ecn-segmentation        - TCP ECN support
    TxTcpMangleIdSegmentation   = 19, // NETIF_F_TSO_MANGLEID_BIT        - tx-tcp-mangleid-segmentation   - IPV4 ID mangling allowed
    TxTcp6Segmentation          = 20, // NETIF_F_TSO6_BIT                - tx-tcp6-segmentation           - TCPv6 segmentation
    TxFcoeSegmentation          = 21, // NETIF_F_FSO_BIT                 - tx-fcoe-segmentation           - FCoE segmentation
    TxGreSegmentation           = 22, // NETIF_F_GSO_GRE_BIT             - tx-gre-segmentation            - GRE with TSO
    TxGreCsumSegmentation       = 23, // NETIF_F_GSO_GRE_CSUM_BIT        - tx-gre-csum-segmentation       - GRE with csum with TSO
    TxIpXip4Segmentation        = 24, // NETIF_F_GSO_IPXIP4_BIT          - tx-ipxip4-segmentation         - IP4 or IP6 over IP4 with TSO
    TxIpXip6Segmentation        = 25, // NETIF_F_GSO_IPXIP6_BIT          - tx-ipxip6-segmentation         - IP4 or IP6 over IP6 with TSO
    TxUdpTunnelSegmentation     = 26, // NETIF_F_GSO_UDP_TUNNEL_BIT      - tx-udp_tnl-segmentation        - UDP TUNNEL with TSO
    TxUdpTunnelCsumSegmentation = 27, // NETIF_F_GSO_UDP_TUNNEL_CSUM_BIT - tx-udp_tnl-csum-segmentation   - DP TUNNEL with TSO & CSUM
    TxGsoPartial                = 28, // NETIF_F_GSO_PARTIAL_BIT         - tx-gso-partial                 - Only segment inner-most L4 in hardware; all other headers in software
    TxTunnelRemCsumSegmentation = 29, // NETIF_F_GSO_TUNNEL_REMCSUM_BIT  - tx-tunnel-remcsum-segmentation - TUNNEL with TSO & REMCSUM
    TxSctpSegmentation          = 30, // NETIF_F_GSO_SCTP_BIT            - tx-sctp-segmentation           - SCTP fragmentation
    TxEspSegmentation           = 31, // NETIF_F_GSO_ESP_BIT             - tx-esp-segmentation            - ESP with TSO
    TxUdpUfoDeprecated          = 32, // NETIF_F_GSO_UDP_BIT                                              - UFO, deprecated except tuntap
    TxUdpSegmentation           = 33, // NETIF_F_GSO_UDP_L4_BIT          - tx-udp-segmentation            - UDP payload GSO
    TxGsoList                   = 34, // NETIF_F_GSO_FRAGLIST_BIT        - tx-gso-list                    - Fraglist GSO
    TxTcpAccEcnSegmentation     = 35, // NETIF_F_GSO_ACCECN_BIT          - tx-tcp-accecn-segmentation     - TCP AccECN w/ TSO

    TxChecksumFcoeCrc           = 36, // NETIF_F_FCOE_CRC_BIT            - tx-checksum-fcoe-crc           - FCoE CRC32
    TxChecksumSctp              = 37, // NETIF_F_SCTP_CRC_BIT            - tx-checksum-sctp               - SCTP checksum offload
    RxNtupleFilter              = 38, // NETIF_F_NTUPLE_BIT              - rx-ntuple-filter               - N-tuple filters supported
    RxHashing                   = 39, // NETIF_F_RXHASH_BIT              - rx-hashing                     - Receive hashing offload
    RxChecksum                  = 40, // NETIF_F_RXCSUM_BIT              - rx-checksum                    - Receive checksumming offload
    TxNoCacheCopy               = 41, // NETIF_F_NOCACHE_COPY_BIT        - tx-nocache-copy                - Use no-cache copyfromuser
    Loopback                    = 42, // NETIF_F_LOOPBACK_BIT            - loopback                       - Enable loopback
    RxFcs                       = 43, // NETIF_F_RXFCS_BIT               - rx-fcs                         - Append FCS to skb pkt data
    RxAll                       = 44, // NETIF_F_RXALL_BIT               - rx-all                         - Receive errored frames too
    TxVlanStagHwInsert          = 45, // NETIF_F_HW_VLAN_STAG_TX_BIT     - tx-vlan-stag-hw-insert         - Transmit VLAN STAG HW acceleration
    RxVlanStagHwParse           = 46, // NETIF_F_HW_VLAN_STAG_RX_BIT     - rx-vlan-stag-hw-parse          - Receive VLAN STAG HW acceleration
    RxVlanStagFilter            = 47, // NETIF_F_HW_VLAN_STAG_FILTER_BIT - rx-vlan-stag-filter            - Receive filtering on VLAN STAGs
    L2FwdOffload                = 48, // NETIF_F_HW_L2FW_DOFFLOAD_BIT    - l2-fwd-offload                 - Allow L2 Forwarding in Hardware

    HwTcOffload                 = 49, // NETIF_F_HW_TC_BIT               - hw-tc-offload                  - Offload TC infrastructure
    EspHwOffload                = 50, // NETIF_F_HW_ESP_BIT              - esp-hw-offload                 - Hardware ESP transformation offload
    EspTxCsumHwOffload          = 51, // NETIF_F_HW_ESP_TX_CSUM_BIT      - esp-tx-csum-hw-offload         - ESP with TX checksum offload
    RxUdpTunnelPortOffload      = 52, // NETIF_F_RX_UDP_TUNNEL_PORT_BIT  - rx-udp_tunnel-port-offload     - Offload of RX port for UDP tunnels
    TlsHwTxOffload              = 53, // NETIF_F_HW_TLS_TX_BIT           - tls-hw-tx-offload              - Hardware TLS TX offload
    TlsHwRxOffload              = 54, // NETIF_F_HW_TLS_RX_BIT           - tls-hw-rx-offload              - Hardware TLS RX offload

    RxGroHw                     = 55, // NETIF_F_GRO_HW_BIT              - rx-gro-hw                      - Hardware Generic receive offload
    TlsHwRecord                 = 56, // NETIF_F_HW_TLS_RECORD_BIT       - tls-hw-record                  - Offload TLS record
    RxGroList                   = 57, // NETIF_F_GRO_FRAGLIST_BIT        - rx-gro-list                    - Fraglist GRO

    MacsecHwOffload             = 58, // NETIF_F_HW_MACSEC_BIT           - macsec-hw-offload              - Offload MACsec operations
    RxUdpGroForwarding          = 59, // NETIF_F_GRO_UDP_FWD_BIT         - rx-udp-gro-forwarding          - Allow UDP GRO for forwarding

    HsrTagInsertOffload         = 60, // NETIF_F_HW_HSR_TAG_INS_BIT      - hsr-tag-ins-offload            - Offload HSR tag insertion
    HsrTagRemoveOffload         = 61, // NETIF_F_HW_HSR_TAG_RM_BIT       - hsr-tag-rm-offload             - Offload HSR tag removal
    HsrForwardOffload           = 62, // NETIF_F_HW_HSR_FWD_BIT          - hsr-fwd-offload                - Offload HSR forwarding
    HsrDupOffload               = 63, // NETIF_F_HW_HSR_DUP_BIT          - hsr-dup-offload                - Offload HSR duplication
}