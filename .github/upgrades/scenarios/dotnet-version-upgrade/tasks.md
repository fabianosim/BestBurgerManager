# .NET Version Upgrade Progress

## Overview

Upgrading BestBurgerManager solution projects from netcoreapp3.1 to net9.0 following the plan in plan.md. The plan updates TargetFrameworks, upgrades/removes packages, and fixes source incompatibilities detected in the assessment.

**Progress**: 8/9 tasks complete <progress value="89" max="100"></progress> 89%

## Tasks

- ✅ 01-update-entities-project: Update BestBurgerManager.Entities to net9.0 ([Content](tasks/01-update-entities-project/task.md), [Progress](tasks/01-update-entities-project/progress-details.md))
- ✅ 02-update-interfaces-project: Update BestBurgerManager.Interfaces to net9.0 ([Content](tasks/02-update-interfaces-project/task.md), [Progress](tasks/02-update-interfaces-project/progress-details.md))
- ✅ 03-update-business-project: Update BestBurgerManager.Business to net9.0 and fix API issues ([Content](tasks/03-update-business-project/task.md), [Progress](tasks/03-update-business-project/progress-details.md))
- ✅ 04-update-api-project: Update BestBurgerManagerAPI to net9.0 and migrate ASP.NET Core references ([Content](tasks/04-update-api-project/task.md), [Progress](tasks/04-update-api-project/progress-details.md))
- ✅ 05-update-tests-project: Update BestBurgerManagerAPI.Test to net9.0 and update test packages ([Content](tasks/05-update-tests-project/task.md), [Progress](tasks/05-update-tests-project/progress-details.md))
- ✅ 06-aggregate-packages-update: Apply package upgrades and remove framework-provided packages ([Content](tasks/06-aggregate-packages-update/task.md), [Progress](tasks/06-aggregate-packages-update/progress-details.md))
- ✅ 07-build-solution-and-fix: Full solution build and fix remaining issues ([Content](tasks/07-build-solution-and-fix/task.md), [Progress](tasks/07-build-solution-and-fix/progress-details.md))
- ✅ 08-run-tests-and-validate: Run unit/integration tests and verify ([Content](tasks/08-run-tests-and-validate/task.md), [Progress](tasks/08-run-tests-and-validate/progress-details.md))
- 🔄 09-finalize-commit-and-cleanup: Final code cleanup and commits per strategy ([Content](tasks/09-finalize-commit-and-cleanup/task.md))
