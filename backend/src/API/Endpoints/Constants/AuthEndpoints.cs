﻿namespace API.Endpoints.Constants;

internal static class AuthEndpoints
{
    private const string BaseRoute = "api/auth";
    
    internal const string SignIn = $"{BaseRoute}/signin";
    internal const string SignUp = $"{BaseRoute}/signup";
    internal const string Refresh = $"{BaseRoute}/refresh-token";
    internal const string Lockout = $"{BaseRoute}/settings/lockout";
    internal const string Revoke = $"{BaseRoute}/revoke";
    internal const string ForgotPassword = $"{BaseRoute}/forgot-password";
    internal const string ResetPassword = $"{BaseRoute}/reset-password";
}

//todo: add user endpoints: edit-user, update user role...

//todo: finish endpoints

