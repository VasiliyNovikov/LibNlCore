using System;

using LinuxCore;

namespace LibNlCore;

public class NetlinkException(int error, string? message) : Exception(message ?? ((LinuxErrorNumber)(-error)).Message)
{
    public LinuxErrorNumber ErrorNumber => (LinuxErrorNumber)(-error);
}