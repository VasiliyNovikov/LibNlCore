using System;
using System.Diagnostics.CodeAnalysis;

namespace NetlinkCore.Protocol;

[SuppressMessage("Design", "CA1069:Enums values should not be duplicated")]
[Flags]
internal enum NetlinkMessageFlags
{
    None = 0,
    Request          = 0x001, // NLM_F_REQUEST       - It is request message
    MultiPart        = 0x002, // NLM_F_MULTI         - Multipart message, terminated by NLMSG_DONE
    Ack              = 0x004, // NLM_F_ACK           - Reply with ack, with zero or error code
    Echo             = 0x008, // NLM_F_ECHO          - Echo this request
    DumpInconsistent = 0x010, // NLM_F_DUMP_INTR     - Dump was inconsistent due to sequence change
    DumpFiltered     = 0x020, // NLM_F_DUMP_FILTERED - Dump was filtered as requested
    // Modifiers to GET request
    Root             = 0x100, // NLM_F_ROOT          - Specify tree root
    Match            = 0x200, // NLM_F_MATCH         - Return all matching
    Atomic           = 0x400, // NLM_F_ATOMIC        - Atomic GET
    Dump      = Root | Match, // NLM_F_DUMP          - NLM_F_ROOT | NLM_F_MATCH
    // Modifiers to NEW request
    Replace          = 0x100, // NLM_F_REPLACE      - Override existing
    Exclusive        = 0x200, // NLM_F_EXCL         - Do not touch, if it exists
    Create           = 0x400, // NLM_F_CREATE       - Create, if it does not exist
    Append           = 0x800, // NLM_F_APPEND       - Add to end of list
    // Modifiers to DELETE request
    NonRecursive     = 0x100, // NLM_F_NONREC       - Do not delete recursively
    // Flags for ACK message
    Capped           = 0x100, // NLM_F_CAPPED       - capped request
    ExtendedAck      = 0x200, // NLM_F_ACK_TLVS     - extended ACK TLVs
}