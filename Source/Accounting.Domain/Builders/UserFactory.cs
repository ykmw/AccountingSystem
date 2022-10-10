using System;
using Accounting.Domain;

namespace Accounting.Data
{
    public static class UserFactory
    {
        public static User Create()
        {
            return GenerateUser();
        }

        public static User Create(string email)
        {
            var user = GenerateUser();
            user.UserName = email;
            user.Email = email;

            return user;
        }
        public static User GenerateUser()
        {
            return new User(
                    GenerateProperty("Email"),
                    GenerateProperty("Name"),
                    GenerateProperty("LastName"),
                    GeneratePhone());
        }
        public static string GenerateProperty(string property)
        {
            switch (property)
            {
                case "Name":
                    return $"{(UserNames)new Random().Next(0, 11)}";
                case "LastName":
                    return $"{(UserLastName)new Random().Next(0, 11)}";
                case "Email":
                    return $"Tester{new Random().Next(0, 1000)}@test.com";
                default:
                    return "Unknown property";
            }
        }
        public static Phone GeneratePhone()
        {
            return new Phone("04", "7654321");
        }
        private enum UserNames
        {
            William,
            Emily,
            ArthurConan,
            Leo,
            John,
            Sarah,
            Oscar,
            Bill,
            Charles,
            Jon,
            Mary,
            Ernest
        }
        private enum UserLastName
        {
            Shakespeare,
            Dickinson,
            Doyle,
            Tolstoy,
            Donne,
            Williams,
            Wilde,
            Blake,
            Dickens,
            Keats,
            Shelley,
            Hemingway
        }
    }
}
