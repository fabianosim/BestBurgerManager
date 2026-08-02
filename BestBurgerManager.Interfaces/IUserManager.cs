using BestBurgerManager.Entities;
using System;

namespace BestBurgerManager.Interfaces
{
    /// <summary>
    /// Represents the methods needed to manage users.
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Gets an user by user Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        User GetUserById(int userId);

        /// <summary>
        /// Adds an user to the API users list.
        /// </summary>
        /// <param name="user"></param>
        void AddUser(User user);
    }
}
