using System.Text.RegularExpressions;

namespace BoardGameTools.Domain.ValueObjects
{
    public class Email : IEquatable<Email>
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string value)
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("L'email ne peut pas être vide", nameof(value));

            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if(!regex.IsMatch(value))
                throw new ArgumentException("L'email n'est pas dans un format valide", nameof(value));

            return new Email(value.Trim().ToLowerInvariant());
        }

        public bool Equals(Email? other)
        {
            if(other is null)
                return false;
            return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj) => Equals(obj as Email);
        public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
        public override string ToString() => Value;

        public static implicit operator string(Email email) => email.Value;
    }
}
