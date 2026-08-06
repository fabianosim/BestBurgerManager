# 04-update-api-project: Update BestBurgerManagerAPI to target net9.0, update ASP.NET Core package references, and address Web API compatibility issues.

Update BestBurgerManagerAPI to target net9.0, update ASP.NET Core package references, and address Web API compatibility issues.

Affected items: BestBurgerManagerAPI\BestBurgerManagerAPI.csproj; Startup/Program and any compatibility-version usage flagged in assessment

Description: Change TargetFramework to net9.0. Replace package references that are now provided by the framework (e.g., Microsoft.AspNetCore.* packages) and update packages that require upgrades (e.g., Microsoft.VisualStudio.Web.CodeGeneration.Design to a supported version). Update Startup/Program code to the recommended hosting model if required and remove obsolete CompatibilityVersion usages. Restore, build, and fix any API incompatibilities.

**Done when**: API project targets net9.0, restores and builds with no errors; the application starts locally (if runnable) and modified files contain no outstanding warnings.
