﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using WowCharComparerWebApp.Configuration;
using WowCharComparerWebApp.Data.Database;
using WowCharComparerWebApp.Data.Database.Repository.Users;
using WowCharComparerWebApp.Logic.Security;
using WowCharComparerWebApp.Models.Internal;

namespace WowCharComparerWebApp.Logic.Users
{
    public class PasswordValidationManager
    {
        private readonly ComparerDatabaseContext _comparerDatabaseContext;
        private readonly DbAccessUser _dbAccessUser;

        public PasswordValidationManager(ComparerDatabaseContext comparerDatabaseContext, DbAccessUser dbAccessUser)
        {
            _comparerDatabaseContext = comparerDatabaseContext;
            _dbAccessUser = dbAccessUser;
        }

        /// <summary>
        /// Determines if any of user input is null or empty 
        /// </summary>
        /// <param name="username">User Name typed by user</param>
        /// <param name="userPassword">Password typed by user</param>
        /// <param name="confirmUserPassword">Password confirmation typed by user</param>
        /// <param name="userEmail">Email address typed by user</param>
        /// <returns>Validation list where first param (bool) is validation correct, second param (string) is message </returns>
        internal List<(bool, string)> ValidateEmptyUserInput(string userName, string userPassword, string confirmUserPassword, string userEmail)
        {
            List<(bool, string)> emptyFieldsValidator = new List<(bool, string)>()
            {
                (true, string.Empty)
            };

            if (string.IsNullOrEmpty(userName))
            {
                emptyFieldsValidator.Add((false, UserMessages.UserNameIsRequired));
            }

            if (string.IsNullOrEmpty(userPassword))
            {
                emptyFieldsValidator.Add((false, UserMessages.PasswordIsRequired));
            }

            if (string.IsNullOrEmpty(confirmUserPassword))
            {
                emptyFieldsValidator.Add((false, UserMessages.PasswordConfirmationIsRequired));
            }

            if (string.IsNullOrEmpty(userEmail))
            {
                emptyFieldsValidator.Add((false, UserMessages.EmailIsRequired));
            }

            return emptyFieldsValidator;
        }

        /// <summary>
        /// Determines if user password field match the confirm password field 
        /// </summary>
        /// <param name="password">Primary password passed by user</param>
        /// <param name="confirmPasword">Secondary (confirmation) password passed by user</param>
        /// <returns>First param (bool) - is validation correct, Second param (string) - message</returns>
        internal (bool, string) CheckPasswordMatch(string password, string confirmPassword)
        {
            return password.Equals(confirmPassword) ? (true, string.Empty) : (false, UserMessages.ConfirmPasswordNoMatch);
        }

        /// <summary>
        /// Checks password validation from user input
        /// </summary>
        /// <param name="password">Password assigned by user</param>
        /// <returns>Validation list where first param (bool) is validation correct, second param (string) is message </returns>
        internal List<(bool, string)> CheckPassword(string password)
        {
            List<(bool, string)> passValidator = new List<(bool, string)>()
            {
                (true, string.Empty)
            };

            if (!password.Any(char.IsDigit))
            {
                passValidator.Add((false, UserMessages.PasswordHasNoNumbers));
            }

            if (!password.Any(char.IsUpper))
            {
                passValidator.Add((false, UserMessages.PasswordHasNoCapitalLetter));
            }

            if (!(password.Length >= 8))
            {
                passValidator.Add((false, UserMessages.PasswordLenghtTooShort));
            }

            return passValidator;
        }

        /// <summary>
        /// Checks user name validation for user input
        /// </summary>
        /// <param name="username">User name typed by user</param>
        /// <param name="db">Database context</param>
        /// <returns>Validation list where first param (bool) is validation correct, second param (string) is message </returns>
        internal List<(bool, string)> CheckUsername(string userName)
        {
            List<(bool, string)> userNameValidator = new List<(bool, string)>()
            {
                (true, string.Empty)
            };

            if (!(userName.Length >= 6))
            {
                userNameValidator.Add((false, UserMessages.NameLengthTooShort));
            }

            if (_comparerDatabaseContext != null && !_comparerDatabaseContext.Users.All(x => x.Nickname != userName))
            {
                userNameValidator.Add((false, UserMessages.AlreadyExists));
            }

            return userNameValidator;
        }

        /// <summary>
        /// Checks email validation format for user input
        /// </summary>
        /// <param name="email">User input email</param>
        /// <param name="db">Database context</param>
        /// <returns>First param (bool) - is validation correct, Second param (string) - message</returns>
        internal (bool, string) CheckEmail(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                return (true, string.Empty);
            }
            catch (Exception)
            {
                //TODO write to user that mail format is incorrect or already is in database
                return (false, UserMessages.EmailInvalidFormat);
            }
        }

        /// <summary>
        /// Using password cryprography checks password match
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="plainPassword">Password entered by user</param>
        /// <returns>Password match status</returns>
        internal bool UserLoginPasswordMatch(User user, string plainPassword)
        {
            using (var userPassCrypto = new UserPasswordCryptography(plainPassword))
            {
                return userPassCrypto.AuthenticateUserPassword(user);
            }
        }
    }
}
