# 05-update-tests-project: Update BestBurgerManagerAPI.Test to target net9.0 and update test packages.

Update BestBurgerManagerAPI.Test to target net9.0 and update test packages.

Affected items: BestBurgerManagerAPI.Test\BestBurgerManagerAPI.Test.csproj; test projects and test host dependencies

Description: Update TargetFramework to net9.0. Upgrade test-related NuGet packages where assessment recommended newer versions (e.g., Microsoft.AspNetCore.Mvc.Testing -> 9.x, Microsoft.VisualStudio.Web.CodeGeneration.Design -> 9.x). Replace deprecated or incompatible test packages (assessment flagged xunit deprecated—review and upgrade to supported test SDK and runners). Restore, build, and run tests, fixing any test compile failures.

**Done when**: Test project targets net9.0, builds cleanly, and unit tests run (or test discovery succeeds) with no compile errors.
