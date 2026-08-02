using System;
using System.Linq;
using System.Collections.Generic;
using BestBurgerManager.Entities;
using BestBurgerManager.Interfaces;
using BestBurgerManager.Business.Extenstions.Exceptions;

namespace BestBurgerManager.Business
{
    /// <summary>
    /// Manages operations with users.
    /// </summary>
    public class UserManager : IUserManager
    {
        /// <summary>
        /// List of current users enabled for the API.
        /// </summary>
        private List<User> ApiUsers { get; set; } = new List<User>();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UserManager()
        { }

        /// <summary>
        /// Initialize the API users by populating the main list of users.
        /// </summary>
        /// <param name="users">The list of users that will use the API.</param>
        public void InitializeApiUsers(List<User> users)
        {
            ApiUsers = users ?? throw new ArgumentException("Users list cannot be null.");
        }

        /// <summary>
        /// Get an user from API user's list by user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUserById(int userId)
        {
            User user = ApiUsers.Where(u => u.Id == userId).FirstOrDefault();

            if (user == null)
                throw new UserNotFoundException(string.Format("User (POS or Kitchen) does not exist. Given User Id: {0}", userId));

            return user; 
        }

        /// <summary>
        /// Adds an user to the list of current users.
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            if (user == null)
                throw new ArgumentException("User to be added cannot be null.");

            // Checks if there is already a user with the same ID on the list. Throws a customized exceptions if the user already exists.
            if (ApiUsers.Exists(u => u.Id == user.Id))
                throw new UserAlreadyExistsException("User already exists in the API user's list.");

            ApiUsers.Add(user);
        }
    }
}
