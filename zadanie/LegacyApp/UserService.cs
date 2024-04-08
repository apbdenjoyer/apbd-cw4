using System;
using System.ComponentModel.DataAnnotations;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth,
            int clientId)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }

            
            if (!new EmailAddressAttribute().IsValid(email))
            {

                return false;
            }
            
            if (!IsUserAbove21(dateOfBirth))
            {
                return false;
            }

            var client = new ClientRepository().GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else
            {
                using var userCreditService = new UserCreditService();
                var creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);

                if (client.Type == "ImportantClient")
                {
                    creditLimit *= 2;
                }

                user.HasCreditLimit = true;
                user.CreditLimit = creditLimit;
            }


            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);

            return true;
        }

        private bool IsUserAbove21(DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (DateTime.Today.Month < dateOfBirth.Month ||
                (DateTime.Today.Month == dateOfBirth.Month && DateTime.Today.Day < dateOfBirth.Day))
            {
                age--;
            }

            return age >= 21;
        }
    }
}