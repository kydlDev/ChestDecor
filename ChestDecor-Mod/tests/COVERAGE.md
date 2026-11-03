# ChestDecor - test coverage (spec 12)

**Automated** = an automated test that ran green; **Manual** = needs a person (`ChestDecor-Manual`); **Gap** = not covered.
Status "Automated" is only true after the TestLab run named in `_agent\TESTLOG.md`.

| Feature / requirement | Unit | Integration | TestLab | Manual | Status |
|---|---|---|---|---|---|
| Loads with its exact KydlCore, spec build, no errors | - | ManifestTests, ArchitectureRulesTests | selftest | - | Automated |
| Four features run, hub tab declared | - | ArchitectureRulesTests | features-running, hub-page | hub-look | Automated + Manual |
| Settings layout and defaults, config migration | FunctionsTests | SettingsTests, ConfigMigrationTests, SuiteRulesTests | (configOverrides) | hub-look | Automated |
| Chest gets a decorator, can be decorated | ChestAttachmentTests | - | chest-gets-decorator | editor-look | Automated + Manual |
| Colours save and read | - | - | colours-save-and-read | editor-look | Automated + Manual |
| Attachments save and read, limits, catalog | ChestAttachmentTests, PlacementLimitsTests, ModItemsTests | - | attachments-save-and-read | editor-look | Automated + Manual |
| One editor per chest (edit lock), ownership change | - | - | edit-lock, other-player-takes-ownership | - | Automated |
| Decoration survives a relog | - | - | decoration-survives-relog | editor-look | Automated + Manual |
| Editor window opens | CameraOrbitTests | - | editor-window-opens | editor-look | Automated + Manual |
| Refund of paid materials on remove / destroy | - | - | - | editor-look | Manual (Gap: no lab test) |
| Harmony patch targets still exist | - | PatchTargetsTests | - | - | Automated |
