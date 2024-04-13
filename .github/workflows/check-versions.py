"""
This script scans all workflows to ensure dependent actions have consistent versions
"""

import pathlib

required_versions = [
    "checkout@v4",
    "setup-dotnet@v4",
    "setup-msbuild@v2",
    "codeql-action/init@v3",
    "codeql-analyze/init@v3",
]

errors = 0
for path in pathlib.Path(__file__).parent.glob("*.yaml"):
    lines = path.read_text(errors="ignore").split("\n")
    for required_version in required_versions:
        package_name = required_version.split("@")[0]
        version_expected = required_version.split("@")[1]
        for line in lines:
            if not "uses:" in line:
                continue
            if not package_name+"@" in line:
                continue
            version_found = line.split("@", 1)[1].strip()
            if version_found == version_expected:
                print(f"{path.name:<35}\t{required_version:<20}\t")
            else:
                errors += 1
                print(f"{path.name:<35}\t{required_version:<20}\t" +
                      f"(expected {version_expected})")
print(f"ERRORS: {errors}")
