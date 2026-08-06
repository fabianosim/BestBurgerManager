# 06-aggregate-packages-update: Apply package updates that affect multiple projects and remove framework-provided packages.

Apply package updates that affect multiple projects and remove framework-provided packages.

Affected items: All projects that reference packages flagged in assessment (Microsoft.AspNetCore.Mvc.Testing, Microsoft.VisualStudio.Web.CodeGeneration.Design, others).

Description: For each package flagged in assessment:
- If functionality is provided by the target framework, remove the explicit package reference and rely on framework reference.
- If a newer package version that supports net9.0 is available, update the package to the suggested version.
- If a package is deprecated or incompatible, document alternatives and replace where feasible.

**Done when**: Package versions are updated or removed per assessment recommendations and all affected projects restore and build without errors.
