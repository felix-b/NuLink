using NUnit.Framework;

namespace NuLink.Tests.Acceptance
{
    public class UnlinkTests : AcceptanceTestBase
    {
        [Test]
        public void AllPatchedAndLinked_DoNotUnLink_AllPatchesReflected()
        {
            ExecuteTestCase(new AcceptanceTestCase {
                Given = {
                    Packages = {
                        ["NuLink.TestCase.FirstPackage"] = new PackageEntry("0.1.0-beta1", PackageStates.PatchedBuiltAndLinked),
                        ["NuLink.TestCase.SecondPackage"] = new PackageEntry("0.2.0-beta2", PackageStates.PatchedBuiltAndLinked)
                    }
                },
                Then = {
                    Packages = {
                        ["NuLink.TestCase.FirstPackage"] = new PackageEntry("0.1.0-beta1", PackageStates.Linked),
                        ["NuLink.TestCase.SecondPackage"] = new PackageEntry("0.2.0-beta2", PackageStates.Linked)
                    },
                    ExpectedValues = {
                        ["ClassOneShouldUseLocallyLinkedPackage"] = "FIRST-CLASS-SYMLINKED",
                        ["ClassTwoShouldUseLocallyLinkedPackage"] = "SECOND-CLASS-SYMLINKED(FIRST-CLASS-SYMLINKED)"
                    }
                }
            });
        }

        [Test]
        public void AllPatchedAndLinked_UnLinkLeaf_UnlinkedPatchesNotReflected()
        {
            ExecuteTestCase(new AcceptanceTestCase {
                Given = {
                    Packages = {
                        ["NuLink.TestCase.FirstPackage"] = new PackageEntry("0.1.0-beta1", PackageStates.PatchedBuiltAndLinked),
                        ["NuLink.TestCase.SecondPackage"] = new PackageEntry("0.2.0-beta2", PackageStates.PatchedBuiltAndLinked)
                    }
                },
                When = () => {
                    ExecNuLinkIn(
                        ConsumerSolutionFolder,
                        "unlink",
                        "-p", "NuLink.TestCase.SecondPackage");
                },
                Then = {
                    Packages = {
                        ["NuLink.TestCase.FirstPackage"] = new PackageEntry("0.1.0-beta1", PackageStates.Linked),
                        ["NuLink.TestCase.SecondPackage"] = new PackageEntry("0.2.0-beta2", PackageStates.Original)
                    },
                    ExpectedValues = {
                        ["ClassOneShouldUseLocallyLinkedPackage"] = "FIRST-CLASS-SYMLINKED",
                        ["ClassTwoShouldUseLocallyLinkedPackage"] = "SECOND-CLASS-ORIGINAL(FIRST-CLASS-SYMLINKED)"
                    }
                }
            });
        }

        [Test]
        public void AllPatchedAndLinked_UnLinkNonLeaf_UnlinkedPatchesNotReflected()
        {
            ExecuteTestCase(new AcceptanceTestCase {
                Given = {
                    Packages = {
                        ["NuLink.TestCase.FirstPackage"] = new PackageEntry("0.1.0-beta1", PackageStates.PatchedBuiltAndLinked),
                        ["NuLink.TestCase.SecondPackage"] = new PackageEntry("0.2.0-beta2", PackageStates.PatchedBuiltAndLinked)
                    }
                },
                When = () => {
                    ExecNuLinkIn(
                        ConsumerSolutionFolder,
                        "unlink",
                        "-p", "NuLink.TestCase.FirstPackage");
                },
                Then = {
                    Packages = {
                        ["NuLink.TestCase.FirstPackage"] = new PackageEntry("0.1.0-beta1", PackageStates.Original),
                        ["NuLink.TestCase.SecondPackage"] = new PackageEntry("0.2.0-beta2", PackageStates.Linked)
                    },
                    ExpectedValues = {
                        ["ClassOneShouldUseLocallyLinkedPackage"] = "FIRST-CLASS-ORIGINAL",
                        ["ClassTwoShouldUseLocallyLinkedPackage"] = "SECOND-CLASS-SYMLINKED(FIRST-CLASS-ORIGINAL)"
                    }
                }
            });
        }

        [Test]
        public void AllPatchedAndLinked_UnLinkAll_PatchesNotReflected()
        {
            ExecuteTestCase(new AcceptanceTestCase {
                Given = {
                    Packages = {
                        ["NuLink.TestCase.FirstPackage"] = new PackageEntry("0.1.0-beta1", PackageStates.PatchedBuiltAndLinked),
                        ["NuLink.TestCase.SecondPackage"] = new PackageEntry("0.2.0-beta2", PackageStates.PatchedBuiltAndLinked)
                    }
                },
                When = () => {
                    ExecNuLinkIn(
                        ConsumerSolutionFolder,
                        "unlink",
                        "-p", "NuLink.TestCase.FirstPackage");
                    ExecNuLinkIn(
                        ConsumerSolutionFolder,
                        "unlink",
                        "-p", "NuLink.TestCase.SecondPackage");
                },
                Then = {
                    Packages = {
                        ["NuLink.TestCase.FirstPackage"] = new PackageEntry("0.1.0-beta1", PackageStates.Original),
                        ["NuLink.TestCase.SecondPackage"] = new PackageEntry("0.2.0-beta2", PackageStates.Original)
                    },
                    ExpectedValues = {
                        ["ClassOneShouldUseLocallyLinkedPackage"] = "FIRST-CLASS-ORIGINAL",
                        ["ClassTwoShouldUseLocallyLinkedPackage"] = "SECOND-CLASS-ORIGINAL(FIRST-CLASS-ORIGINAL)"
                    }
                }
            });
        }
    }
}