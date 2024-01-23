Things I learned while fidgeting with GitHub Actions

* `dotnet format` doesn't work on .sln files on Linux but it does on csproj files

* GH_TOKEN is only available in workflows with some trigger sources (not call or PR)

* Define a `concurrency` `group` and set `cancel-in-progress` to allow a new workflow to cancel an existing running one

* Paths as arguments must be surrounded by double quotes on Linux but not always on Windows

* It's convenient when one workflow calls another workflow rather than having massive workflow files with lots of thick jobs