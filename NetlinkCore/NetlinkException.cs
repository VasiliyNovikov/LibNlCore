using System;

using LinuxCore;

namespace NetlinkCore;

public class NetlinkException(int error, string? message) : Exception(message ?? ((LinuxErrorNumber)(-error)).Message)
{
    public LinuxErrorNumber ErrorNumber => (LinuxErrorNumber)(-error);
}