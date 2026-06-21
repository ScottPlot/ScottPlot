---
name: build-full
description: Build the full ScottPlot 5 solution. Use only for solution-wide validation or changes affecting controls, demos, sandboxes, workloads, or platform projects. It may require workload restore and may be very slow. For core library or unit-test-only changes, favor build-core.
---

# Full Build

Prefer `build-core` unless the change reaches platform projects or the user asks for a full build.

Run from `src/ScottPlot5`:

```powershell
dotnet workload restore
dotnet restore
dotnet build --configuration Release
```

If the full build succeeds and tests are requested, run:

```powershell
dotnet test --configuration Release --no-build --verbosity minimal
```

Expect this to touch many targets. If workload restore fails due to missing SDK/workload/network access, report that directly rather than substituting a narrower build.
