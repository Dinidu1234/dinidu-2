# FixBuildAndRun

1. Set `KickBlastStudentUI` as **Startup Project**.
2. In Visual Studio: **Build -> Clean Solution -> Rebuild Solution**.
3. If still failing, close Visual Studio, delete `bin` and `obj`, reopen and rebuild.
4. Verify `Properties/launchSettings.json` has only `commandName: Project` profile.
5. Ensure configuration is `Debug` and platform is `Any CPU`.
