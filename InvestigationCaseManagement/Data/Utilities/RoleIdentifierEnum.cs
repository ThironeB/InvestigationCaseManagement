using System.Runtime.Serialization;

namespace InvestigationCaseManagement.Data.Utilities
{
    public class RoleIdentifier
    {
        public static readonly RoleIdentifier Administrador = new RoleIdentifier("Administrador", "91790a12-a288-4bf4-a56a-ca29740d28a5");
        public static readonly RoleIdentifier Investigador = new RoleIdentifier("Investigador", "68a24bd8-6f5d-4951-9e41-45b232780e1a");

        private static readonly Dictionary<string, RoleIdentifier> _rolesByValue = new Dictionary<string, RoleIdentifier>
        {
            { "91790a12-a288-4bf4-a56a-ca29740d28a5", Administrador },
            { "68a24bd8-6f5d-4951-9e41-45b232780e1a", Investigador }
        };

        private static readonly Dictionary<string, RoleIdentifier> _rolesByName = new Dictionary<string, RoleIdentifier>
        {
            { "Administrador", Administrador },
            { "Investigador", Investigador }
        };

        public string Value { get; }
        public string Name { get; }

        private RoleIdentifier(string name, string value)
        {
            Name = name;
            Value = value;          
        }

        public override string ToString()
        {
            return Value;
        }

        public static RoleIdentifier FromValue(string value)
        {
            return _rolesByValue.TryGetValue(value, out var role) ? role : null;
        }

        public static RoleIdentifier FromName(string name)
        {
            return _rolesByName.TryGetValue(name, out var role) ? role : null;
        }
    }
}
