# Service Queue Simulator

Simulate first-come-first-served jobs across identical service desks.

![Sample run from Service Queue Simulator](docs/demo.webp)

## Run

Install .NET 8 SDK and runtime. Run these commands from the repository root.

```sh
dotnet build --nologo
dotnet bin/Debug/net8.0/App.dll sample.json
```

Replace `sample.json` with a JSON file following the same structure. The app prints a JSON report to standard output.

Jobs are arranged by nondecreasing arrival time and use [arrival, duration] pairs. Tied desks are assigned by desk order.

## Verify

Run `python check.py` with Python 3.11 or newer after the build step when applicable. The check verifies the bundled scenario and confirms that an invalid input returns an error.
