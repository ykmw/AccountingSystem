
// IDE Rules

// SonarQube Rules

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "Do not know why code analysis is reporting this type cannot be serialized", Scope = "type", Target = "~T:Accounting.Domain.CustomApplicationException")]
[assembly: SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "Do not know why code analysis is reporting this type cannot be serialized", Scope = "type", Target = "~T:Accounting.Domain.InvalidRangeException")]
[assembly: SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = "Switch Expression just looks horrible and unreadable!", Scope = "member", Target = "~M:Accounting.Data.AddressFactory.GenerateProperty(System.String)~System.String")]
