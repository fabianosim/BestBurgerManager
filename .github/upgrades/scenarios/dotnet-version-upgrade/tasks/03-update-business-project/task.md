# 03-update-business-project: Update BestBurgerManager.Business to target net9.0 and address source incompatibilities.

Update BestBurgerManager.Business to target net9.0 and address source incompatibilities.

Affected items: BestBurgerManager.Business\BestBurgerManager.Business.csproj; code files flagged in assessment (approx. 9 files)

Description: Update TargetFramework to net9.0, update NuGet references if required, restore, build, and fix compilation errors and source-incompatible APIs (the assessment flagged about ~16 source-incompatible API hits). Prioritize small code fixes (e.g., remove/replace obsolete constructors, update serialization patterns).

**Done when**: The Business project targets net9.0 and builds without errors; all warnings introduced or present in files touched by this task have been fixed.
