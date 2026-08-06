# .NET Version Upgrade Plan

## Overview

**Target**: Upgrade all projects in the BestBurgerManager solution from netcoreapp3.1 to net9.0.
**Scope**: 5 projects (Entities, Interfaces, Business, BestBurgerManagerAPI, BestBurgerManagerAPI.Test). Changes include updating each project's TargetFramework, updating or removing packages that are now provided by the framework, and applying small source fixes for source-incompatible APIs identified in the assessment.

## Tasks

### 01-update-entities-project
Update BestBurgerManager.Entities to target net9.0.

Affected items: BestBurgerManager.Entities\BestBurgerManager.Entities.csproj

Description: Change the project's TargetFramework to net9.0 and restore packages (if any). Build the project and fix any compilation or API issues that surface.

**Done when**: The Entities project targets net9.0, restores successfully, and builds without errors or warnings in the modified project.

---

### 02-update-interfaces-project
Update BestBurgerManager.Interfaces to target net9.0.

Affected items: BestBurgerManager.Interfaces\BestBurgerManager.Interfaces.csproj

Description: Change TargetFramework to net9.0, restore, and build. Resolve any source-level incompatibilities (these should be minimal for interface-only projects).

**Done when**: The Interfaces project targets net9.0 and builds cleanly (no errors, no warnings in modified files).

---

### 03-update-business-project
Update BestBurgerManager.Business to target net9.0 and address source incompatibilities.

Affected items: BestBurgerManager.Business\BestBurgerManager.Business.csproj; code files flagged in assessment (approx. 9 files)

Description: Update TargetFramework to net9.0, update NuGet references if required, restore, build, and fix compilation errors and source-incompatible APIs (the assessment flagged about ~16 source-incompatible API hits). Prioritize small code fixes (e.g., remove/replace obsolete constructors, update serialization patterns).

**Done when**: The Business project targets net9.0 and builds without errors; all warnings introduced or present in files touched by this task have been fixed.

---

### 04-update-api-project
Update BestBurgerManagerAPI to target net9.0, update ASP.NET Core package references, and address Web API compatibility issues.

Affected items: BestBurgerManagerAPI\BestBurgerManagerAPI.csproj; Startup/Program and any compatibility-version usage flagged in assessment

Description: Change TargetFramework to net9.0. Replace package references that are now provided by the framework (e.g., Microsoft.AspNetCore.* packages) and update packages that require upgrades (e.g., Microsoft.VisualStudio.Web.CodeGeneration.Design to a supported version). Update Startup/Program code to the recommended hosting model if required and remove obsolete CompatibilityVersion usages. Restore, build, and fix any API incompatibilities.

**Done when**: API project targets net9.0, restores and builds with no errors; the application starts locally (if runnable) and modified files contain no outstanding warnings.

---

### 05-update-tests-project
Update BestBurgerManagerAPI.Test to target net9.0 and update test packages.

Affected items: BestBurgerManagerAPI.Test\BestBurgerManagerAPI.Test.csproj; test projects and test host dependencies

Description: Update TargetFramework to net9.0. Upgrade test-related NuGet packages where assessment recommended newer versions (e.g., Microsoft.AspNetCore.Mvc.Testing -> 9.x, Microsoft.VisualStudio.Web.CodeGeneration.Design -> 9.x). Replace deprecated or incompatible test packages (assessment flagged xunit deprecated—review and upgrade to supported test SDK and runners). Restore, build, and run tests, fixing any test compile failures.

**Done when**: Test project targets net9.0, builds cleanly, and unit tests run (or test discovery succeeds) with no compile errors.

---

### 06-aggregate-packages-update
Apply package updates that affect multiple projects and remove framework-provided packages.

Affected items: All projects that reference packages flagged in assessment (Microsoft.AspNetCore.Mvc.Testing, Microsoft.VisualStudio.Web.CodeGeneration.Design, others).

Description: For each package flagged in assessment:
- If functionality is provided by the target framework, remove the explicit package reference and rely on framework reference.
- If a newer package version that supports net9.0 is available, update the package to the suggested version.
- If a package is deprecated or incompatible, document alternatives and replace where feasible.

**Done when**: Package versions are updated or removed per assessment recommendations and all affected projects restore and build without errors.

---

### 07-build-solution-and-fix
Build the entire solution and resolve remaining compilation issues and warnings across projects modified in this flow.

Description: Perform a full solution restore and build. Iterate on compilation fixes until the solution builds without errors. Fix all warnings in projects changed by this upgrade (do not suppress warnings without approval).

**Done when**: Solution builds successfully with zero errors and no warnings in projects modified by these tasks.

---

### 08-run-tests-and-validate
Run unit and integration tests, validate expected behavior, and verify API surface.

Description: Execute the test suite (BestBurgerManagerAPI.Test) and any other automated checks. Investigate test failures and fix source issues introduced by upgrades.

**Done when**: Tests run and pass, or failing tests are documented and resolved. Test run completes with expected pass rate for the suite.

---

### 09-finalize-commit-and-cleanup
Finalize changes, run code cleanup, and commit per commit strategy.

Description: Ensure scenario-instructions.md reflects any decisions made during planning, run a final code-format/cleanup pass on modified files, and commit all changes according to the confirmed commit strategy (After Each Task by default). Push changes only if user preference allows.

**Done when**: All task commits are created according to the commit strategy, tasks.md is updated to reflect completion, and the user is notified of the result.

---


*Notes*: Follow scenario-instructions.md for branch, commit, and sync behavior. For safety, update projects in dependency order (Entities → Interfaces → Business → API → Tests). Fixing compilation and API incompatibilities should be done in small, testable commits.
