﻿using GenerateTokenDemo.Models;
using Microsoft.Extensions.Configuration;

namespace GenerateTokenDemo.Services;

internal static class UsersStore
{
    private readonly static List<User> _users = new();
    static UsersStore()
    {
        _users.AddRange(new List<User>()
        {
            new User { UserName="Bart Allen", PasswordHash=BCrypt.Net.BCrypt.HashPassword("password")},
            new User { UserName="Bart Allen", PasswordHash=BCrypt.Net.BCrypt.HashPassword("password")},
        });
    }

    public static void AddNewUser(User user) => _users.Add(user);

    public static IEnumerable<User> GetAllUsers() => _users;
}
