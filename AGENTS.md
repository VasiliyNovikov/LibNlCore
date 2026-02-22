# AGENTS.md

This file provides guidance to AI coding agents when working with code in this repository.

## Project Overview

LibNlCore is a high-performance .NET wrapper for Linux netlink sockets. It provides type-safe, zero-allocation abstractions over the kernel's netlink protocol for network interface management, ethtool features, and network namespace operations. Linux-only (`[SupportedOSPlatform("linux")]`).

## Build and Test Commands

```bash
dotnet build                # Build entire solution

# Tests must run as root (they create virtual network interfaces)
dotnet build && sudo -E --preserve-env=PATH bash -c "dotnet test --no-build"

# Run a single test
dotnet build && sudo -E --preserve-env=PATH bash -c "dotnet test --no-build --filter 'FullyQualifiedName~ClassName.MethodName'"
```

Tests are globally marked `[DoNotParallelize]` and run sequentially.

## Build Configuration

- **Target**: net10.0, C# 14
- **Strict mode**: `TreatWarningsAsErrors`, `AnalysisMode=Recommended`, `EnforceCodeStyleInBuild`, `Nullable=enable`
- **Unsafe code**: Enabled (required for ref struct protocol handling)
- Central package versioning via `Directory.Packages.props`

## Architecture

Layered protocol wrapper with two socket families:

```
RouteNetlinkSocket / EthToolNetlinkSocket    ← High-level API
        ↓                    ↓
        ↓         GenericNetlinkSocket<TCmd>  ← Generic family base (templated on command enum)
        ↓                    ↓
   NetlinkSocket (abstract base)             ← Socket lifecycle, send/receive, Get<>/Post<>
        ↓
   Protocol Layer                            ← Message/attribute builders
   (NetlinkMessageWriter, NetlinkAttributeWriter, SpanReader/SpanWriter)
        ↓
   LinuxSocketBase (from LinuxCore)          ← Raw syscalls
```

**Key directories:**
- `LibNlCore/Route/` — NETLINK_ROUTE: link and address management
- `LibNlCore/Generic/` — NETLINK_GENERIC: generic netlink base + ethtool
- `LibNlCore/Protocol/` — Zero-allocation message/attribute parsing and writing (ref structs), plus kernel constant enums/structs (C-style naming, relaxed rules in .editorconfig)

**Core patterns:**
- **Ref struct builders**: `NetlinkMessageWriter<THeader, TAttr>` and `NetlinkAttributeWriter<TAttr>` are stack-only, generic over `unmanaged` header structs and `Enum` attribute types
- **Lazy collections**: `NetlinkMessageCollection` and `NetlinkAttributeCollection` implement iterator protocol for zero-copy parsing
- **Pooled buffers**: `NetlinkBuffer` wraps `ArrayPool<byte>` for send/receive buffers
- **Family resolution**: `GenericNetlinkFamilyResolver` caches generic netlink family IDs (thread-safe via `Lock`)
- **Nested attributes**: Scoped `WriteNested<>()` for complex hierarchical request/response structures

## Testing Patterns

- Tests follow create-test-delete with try/finally for resource cleanup (virtual interfaces, bridges)
- Socket lifecycle managed via `using` statements
- `InternalsVisibleTo("LibNlCore.Tests")` exposes internals to the test project

## Code Style

Configured in `.editorconfig`:
- 4-space indent for C#, 2-space for XML/csproj
- LF line endings, no final newline
- File-scoped namespaces
- `var` usage allowed (IDE0008 suppressed)
- Expression-bodied members allowed (IDE0021/0022/0023 suppressed)
- Kernel constant files in `Protocol/`
- Test files allow underscores in names (CA1707 suppressed)

## Dependencies

- **LinuxCore**: P/Invoke bindings to Linux syscalls (socket, bind, sendmsg, etc.)
- **NetNsCore**: Network namespace operations
- **NetworkingPrimitivesCore**: `MACAddress` and IP utilities
- **MSTest**: Testing framework (test project only)
